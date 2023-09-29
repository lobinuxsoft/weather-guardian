using CryingOnion.Utils;
using UnityEngine;

namespace WeatherGuardian.Behaviours
{
    public class UprightSpringBehaviour : StateMachineBehaviour
    {
        [field: Header("Upright Spring:")]
        [SerializeField] private UprightSpringConfig uprightSpringConfig;

        public bool LockDirection { get; set; } = false;
        public Vector3 ForwardDirection { get; set; } = Vector3.zero;
        public Vector3 RightDirection { get; set; } = Vector3.zero;

        private HeightSpringBehaviour heightSpringBehaviour;
        private Quaternion uprightTargetRot = Quaternion.identity; // Adjust y value to match the desired direction to face.
        private Quaternion lastTargetRot = Quaternion.identity;
        private Vector3 platformInitRot = Vector3.zero;
        private bool didLastRayHit;
        private Vector3 lastLookDirection = Vector3.zero;

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CalculateTargetRotation(heightSpringBehaviour.Body, GetLookDirection(heightSpringBehaviour.Body, heightSpringBehaviour.GravitationalForce), heightSpringBehaviour.GroundedInfo);

            Quaternion currentRot = heightSpringBehaviour.Body.rotation;
            Quaternion toGoal = MathsUtils.ShortestRotation(uprightTargetRot, currentRot);

            Vector3 rotAxis;
            float rotDegrees;

            toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
            rotAxis.Normalize();

            float rotRadians = rotDegrees * Mathf.Deg2Rad;

            heightSpringBehaviour.Body.AddTorque((rotAxis * (rotRadians * uprightSpringConfig.Strength)) - (heightSpringBehaviour.Body.angularVelocity * uprightSpringConfig.Damper));
        }

        /// <summary>
        /// Determines the desired y rotation for the character, with account for platform rotation.
        /// </summary>
        /// <param name="yLookAt">The input look rotation.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void CalculateTargetRotation(in Rigidbody body, in Vector3 yLookAt, in (bool grounded, RaycastHit rayHit) groundedInfo)
        {
            if (didLastRayHit)
            {
                lastTargetRot = uprightTargetRot;

                try
                {
                    platformInitRot = body.transform.parent.rotation.eulerAngles;
                }
                catch
                {
                    platformInitRot = Vector3.zero;
                }
            }

            if (groundedInfo.rayHit.rigidbody == null)
                didLastRayHit = true;
            else
                didLastRayHit = false;

            if (yLookAt != Vector3.zero)
            {
                uprightTargetRot = Quaternion.LookRotation(yLookAt, groundedInfo.grounded ?
                    groundedInfo.rayHit.normal :
                    -heightSpringBehaviour.GravitationalForce.normalized);

                lastTargetRot = uprightTargetRot;
                try
                {
                    platformInitRot = body.transform.parent.rotation.eulerAngles;
                }
                catch
                {
                    platformInitRot = Vector3.zero;
                }
            }
            else
            {
                try
                {
                    Vector3 platformRot = body.transform.parent.rotation.eulerAngles;
                    Vector3 deltaPlatformRot = platformRot - platformInitRot;
                    //float yAngle = lastTargetRot.eulerAngles.y + deltaPlatformRot.y;
                    //uprightTargetRot = Quaternion.Euler(new Vector3(0f, yAngle, 0f));
                    uprightTargetRot = Quaternion.Euler(lastTargetRot.eulerAngles + deltaPlatformRot);
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets the look desired direction for the character to look.
        /// </summary>
        private Vector3 GetLookDirection(in Rigidbody body, in Vector3 gravitationalForce)
        {
            Vector3 velocity = body.velocity.sqrMagnitude > 0.2f * 0.2f ?
            Vector3.ProjectOnPlane(body.velocity, -gravitationalForce).normalized : lastLookDirection;

            return LockDirection ? ForwardDirection : velocity;
        }
    }
}