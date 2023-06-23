using WeatherGuardian.Behaviours.Configs;
using UnityEngine;

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
        float verticalVelocity = 0.0f;

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

            animator.SetFloat(airVelocityHash, verticalVelocity);

            animator.SetBool(groundedHash, verticalVelocity < 0.0f && heightSpringBehaviour.GroundedInfo.grounded);

            if (!heightSpringBehaviour.GroundedInfo.grounded && moveDir.magnitude > 0.5f)
                AirMovement(animator, moveDir);
            else
                moveDir = Vector3.zero;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(airVelocityHash, 0);
        }

        public void Move(Vector3 moveDir)
        {
            if (heightSpringBehaviour != null && !heightSpringBehaviour.GroundedInfo.grounded)
                this.moveDir = Vector3.ClampMagnitude(moveDir, 1.0f);
            else
                this.moveDir = Vector3.zero;
        }

        public void AirMovement(Animator animator, Vector3 moveInput)
        {
            if (heightSpringBehaviour == null || heightSpringBehaviour.GroundedInfo.grounded || animator.GetBool(umbrellaHash)) return;

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