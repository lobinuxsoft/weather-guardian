using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class AirBehaviour : StateMachineBehaviour
    {
        int airVelocityHash = Animator.StringToHash("AirVelocity");
        int groundedHash = Animator.StringToHash("GROUNDED");
        int umbrellaHash = Animator.StringToHash("UMBRELLA");

        [Header("Movement:")]
        [SerializeField] private AirMoveConfig moveConfig;

        private HeightSpringBehaviour heightSpringBehaviour;

        private Vector3 moveDir = Vector3.zero;
        private float speedFactor = 1f;
        private float maxAccelForceFactor = 1f;
        private Vector3 goalVel = Vector3.zero;
        private float verticalVelocity = 0.0f;
        private float maxVerticalVel = 1.0f;


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            heightSpringBehaviour.ShouldMaintainHeight = false;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            verticalVelocity = Vector3.Dot(heightSpringBehaviour.Body.velocity, Vector3.up);

            if (maxVerticalVel < float.Epsilon)
                maxVerticalVel = Mathf.Abs(verticalVelocity);

            float deltaVerticalVel = verticalVelocity / maxVerticalVel;

             animator.SetFloat(airVelocityHash, deltaVerticalVel);

            animator.SetBool(groundedHash, deltaVerticalVel < 0.0f && heightSpringBehaviour.GroundedInfo.grounded);

            GravityInfluence(animator);

            if (!heightSpringBehaviour.GroundedInfo.grounded && moveDir.magnitude > 0.5f)
                AirMovement(animator, moveDir);
            else
                moveDir = Vector3.zero;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(airVelocityHash, 0);
            maxVerticalVel = 0.0f;
        }

        public void Move(Vector3 moveDir)
        {
            if (heightSpringBehaviour != null && !heightSpringBehaviour.GroundedInfo.grounded)
                this.moveDir = Vector3.ClampMagnitude(moveDir, 1.0f);
            else
                this.moveDir = Vector3.zero;
        }

        private void GravityInfluence(Animator animator)
        {
            if (heightSpringBehaviour == null || heightSpringBehaviour.GroundedInfo.grounded || animator.GetBool(umbrellaHash)) return;

            float gravityInfluence = moveConfig.GravityFactorFromDot.Evaluate(verticalVelocity) * moveConfig.GravityMultiplier;

            heightSpringBehaviour.Body.velocity += heightSpringBehaviour.GravitationalForce * gravityInfluence * Time.fixedDeltaTime;
        }

        private void AirMovement(Animator animator, Vector3 moveInput)
        {
            if (heightSpringBehaviour == null || heightSpringBehaviour.GroundedInfo.grounded || animator.GetBool(umbrellaHash)) return;

            bool hitWall = Physics.Raycast(heightSpringBehaviour.Body.worldCenterOfMass, moveInput, 1, moveConfig.RayLayerMask, QueryTriggerInteraction.Ignore);

            if (hitWall) return;

            Vector3 unitGoal = Vector3.ClampMagnitude(moveInput.normalized, 1.0f);
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
                heightSpringBehaviour.Body.worldCenterOfMass + new Vector3(0f, heightSpringBehaviour.Body.transform.localScale.y * moveConfig.LeanFactor, 0.0f)
            );
        }
    }
}