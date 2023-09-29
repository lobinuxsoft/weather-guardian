using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class AirUmbrellaBehaviour : StateMachineBehaviour
    {
        int enterTagHash = Animator.StringToHash("ENTER");
        int loopTagHash = Animator.StringToHash("LOOP");
        int exitTagHash = Animator.StringToHash("EXIT");

        int umbrellaHash = Animator.StringToHash("UMBRELLA");
        int umbrellaHVelocityHash = Animator.StringToHash("UmbrellaHVelocity");
        int umbrellaVVelocityHash = Animator.StringToHash("UmbrellaVVelocity");
        //int umbrellaStateHash = Animator.StringToHash("UmbrellaState");
        int groundedHash = Animator.StringToHash("GROUNDED");

        [Header("Movement:")]
        [SerializeField] private AirMoveConfig moveConfig;

        [Space(5)]
        [Header("Air Drag:")]
        [SerializeField] private DragConfig airDragConfig;

        private HeightSpringBehaviour heightSpringBehaviour;
        private UprightSpringBehaviour uprightSpringBehaviour;

        private float verticalVelocity = 0.0f;
        private float forwardVelocity = 0.0f;
        private float horizontalVelocity = 0.0f;

        private Vector3 moveDir = Vector3.zero;
        private float speedFactor = 1f;
        private float maxAccelForceFactor = 1f;
        private Vector3 goalVel = Vector3.zero;

        private Vector3 forwardDir;
        private Vector3 rightDir;
        private float umbrellaStateValue = 0.0f;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            if (uprightSpringBehaviour == null)
                uprightSpringBehaviour = animator.GetBehaviour<UprightSpringBehaviour>();

            if (stateInfo.tagHash != enterTagHash) return;

            forwardDir = animator.transform.forward;
            rightDir = animator.transform.right;

            heightSpringBehaviour.Body.drag = airDragConfig.Drag.Evaluate(0);
            uprightSpringBehaviour.LockDirection = true;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.tagHash != loopTagHash) return;

            uprightSpringBehaviour.ForwardDirection = forwardDir;
            uprightSpringBehaviour.RightDirection = rightDir;

            verticalVelocity = Vector3.Dot(heightSpringBehaviour.Body.velocity, Vector3.up);

            forwardVelocity = Vector3.Dot(heightSpringBehaviour.Body.velocity, uprightSpringBehaviour.ForwardDirection);
            horizontalVelocity = Vector3.Dot(heightSpringBehaviour.Body.velocity, uprightSpringBehaviour.RightDirection);

            heightSpringBehaviour.Body.drag = airDragConfig.Drag.Evaluate(forwardVelocity);

            umbrellaStateValue += animator.GetBool(umbrellaHash) ? Time.deltaTime : -Time.deltaTime;
            umbrellaStateValue = Mathf.Clamp01(umbrellaStateValue);

            //animator.SetFloat(umbrellaStateHash, umbrellaStateValue);

            animator.SetFloat(umbrellaHVelocityHash, horizontalVelocity);
            animator.SetFloat(umbrellaVVelocityHash, forwardVelocity);

            animator.SetBool(groundedHash, verticalVelocity < 0f && heightSpringBehaviour.GroundedInfo.grounded);

            GravityInfluence(animator);

            if (animator.GetBool(groundedHash))
                animator.SetBool(umbrellaHash, false);
            else
                AirMovement(moveDir);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.tagHash != exitTagHash) return;

            animator.SetBool(umbrellaHash, false);
            uprightSpringBehaviour.LockDirection = false;
            uprightSpringBehaviour.ForwardDirection = Vector3.zero;
            uprightSpringBehaviour.RightDirection = Vector3.zero;
            //heightSpringBehaviour.ShouldMaintainHeight = true;
            heightSpringBehaviour.Body.drag = 0.0f;
            heightSpringBehaviour.Body.velocity = Vector3.zero;
        }

        public void UseUmbrella(Animator animator)
        {
            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            if (heightSpringBehaviour.GroundedInfo.grounded) return;

            animator.SetBool(umbrellaHash, !animator.GetBool(umbrellaHash));
        }

        public void UmbrellaOpenForced(Animator animator) => animator.SetBool(umbrellaHash, true);

        public void UmbrellaCloseForced(Animator animator) => animator.SetBool(umbrellaHash, false);

        public void Move(Vector3 moveDir) => this.moveDir = Vector3.ClampMagnitude(moveDir, 1.0f);

        private void GravityInfluence(Animator animator)
        {
            if (animator.GetBool(groundedHash)) return;

            float gravityInfluence = moveConfig.GravityFactorFromDot.Evaluate(verticalVelocity) * moveConfig.GravityMultiplier;

            heightSpringBehaviour.Body.velocity += heightSpringBehaviour.GravitationalForce * gravityInfluence * Time.fixedDeltaTime;
        }

        private void AirMovement(in Vector3 moveInput)
        {
            if (heightSpringBehaviour == null || uprightSpringBehaviour == null) return;

            bool hitWall = Physics.Raycast(heightSpringBehaviour.Body.worldCenterOfMass, moveInput, 1, moveConfig.RayLayerMask, QueryTriggerInteraction.Ignore);

            if (hitWall) return;

            Vector3 unitGoal = moveInput;
            Vector3 unitVel = this.goalVel.normalized;
            float velDot = Vector3.Dot(unitGoal, unitVel);
            float accel = moveConfig.Acceleration * moveConfig.AccelerationFactorFromDot.Evaluate(velDot);
            Vector3 goalVel = unitGoal * moveConfig.MaxSpeed * speedFactor;

            this.goalVel = Vector3.MoveTowards(this.goalVel, goalVel, accel * Time.fixedDeltaTime);

            Vector3 neededAccel = (this.goalVel - heightSpringBehaviour.Body.velocity) / Time.fixedDeltaTime;
            float maxAccel = moveConfig.Acceleration * moveConfig.AccelerationFactorFromDot.Evaluate(velDot) * maxAccelForceFactor;
            neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

            /* Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input. */
            heightSpringBehaviour.Body.AddForceAtPosition(
                Vector3.Scale(neededAccel * heightSpringBehaviour.Body.mass, moveConfig.MoveForceScale),
                heightSpringBehaviour.Body.worldCenterOfMass + new Vector3(0f, heightSpringBehaviour.Body.transform.localScale.y * moveConfig.LeanFactor, 0f)
            );
        }
    }
}