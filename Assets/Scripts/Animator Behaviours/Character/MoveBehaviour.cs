using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class MoveBehaviour : StateMachineBehaviour
    {
        int groundVelocityHash = Animator.StringToHash("GroundVelocity");
        int groundedHash = Animator.StringToHash("GROUNDED");

        [Header("Movement:")]
        [SerializeField] private MoveConfig moveConfig;

        [Header("Jump:")]
        [SerializeField] JumpConfig jumpConfig;

        private HeightSpringBehaviour heightSpringBehaviour;
        private Vector3 moveDir = Vector3.zero;
        private float speedFactor = 1f;
        private float maxAccelForceFactor = 1f;
        private Vector3 goalVel = Vector3.zero;
        private bool isMoving = false;
        private bool canSprint = false;
        private float timerToSprint = 0f;
        private bool coyoteTimeExpired;

        public bool CanMove { get; set; } = true;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            heightSpringBehaviour.ShouldMaintainHeight = true;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!CanMove) return;

            float currentSpeed = Mathf.Clamp(heightSpringBehaviour.Body.velocity.magnitude / moveConfig.MaxSpeed, 0, 2);
            isMoving = currentSpeed > 0.1f;
            canSprint = currentSpeed > 0.75f;

            timerToSprint = canSprint ? timerToSprint + Time.deltaTime : 0;

            animator.SetFloat(groundVelocityHash, isMoving ? currentSpeed : 0);

            coyoteTimeExpired = heightSpringBehaviour.LastGroundedTime > jumpConfig.CoyoteTime;            

            animator.SetBool(groundedHash, !coyoteTimeExpired);

            if (heightSpringBehaviour.GroundedInfo.grounded)
                GroundMovement(moveDir);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(groundVelocityHash, 0);
        }

        public void Move(Vector3 moveDir) => this.moveDir = Vector3.ClampMagnitude(moveDir, 1.0f);

        /// <summary>
        /// Apply forces to move the character up to a maximum acceleration, with consideration to acceleration graphs.
        /// </summary>
        /// <param name="moveInput">The player movement input.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void GroundMovement(in Vector3 moveInput)
        {
            if (heightSpringBehaviour == null) return;

            bool hitWall = Physics.Raycast(heightSpringBehaviour.Body.worldCenterOfMass, moveInput, 1, moveConfig.RayLayerMask, QueryTriggerInteraction.Ignore);

            if (hitWall) return;

            Vector3 unitGoal = moveInput;
            Vector3 unitVel = this.goalVel.normalized;
            float velDot = Vector3.Dot(unitGoal, unitVel);
            float accel = moveConfig.Acceleration * moveConfig.AccelerationFactorFromDot.Evaluate(velDot);
            Vector3 goalVel = unitGoal * moveConfig.MaxSpeed * speedFactor * (timerToSprint < moveConfig.TimerToSprint ? 1 : moveConfig.SprintMultiplier);

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