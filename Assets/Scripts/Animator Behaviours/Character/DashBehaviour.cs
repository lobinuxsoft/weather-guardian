using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class DashBehaviour : StateMachineBehaviour
    {
        // Dont enter from this states
        private readonly int stompHash = Animator.StringToHash("STOMP");

        // Parameter for internal control
        private readonly int dashHash = Animator.StringToHash("DASH");
        private readonly int umbrellaHash = Animator.StringToHash("UMBRELLA");
        private readonly int groundedHash = Animator.StringToHash("GROUNDED");

        // Tags to evaluate states
        private readonly int enterTagHash = Animator.StringToHash("ENTER");
        private readonly int loopTagHash = Animator.StringToHash("LOOP");
        private readonly int exitTagHash = Animator.StringToHash("EXIT");

        [Header("Dash:")]
        [SerializeField] private DashConfig dashConfig;

        HeightSpringBehaviour heightSpringBehaviour = null;
        UprightSpringBehaviour uprightSpringBehaviour = null;
        CapsuleCollider capsuleCollider = null;

        public Vector3 defaultCapsuleCenter = Vector3.zero;
        public float defaultCapsuleHeigth = 0;

        float dragDefault = 0;
        float dragModifier = 0;
        Vector3 forward = Vector3.zero;

        float lerpDuration = 0.0f;
        float lastTimeUse = 0.0f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.tagHash == enterTagHash)
            {
                if (heightSpringBehaviour == null)
                    heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

                if (uprightSpringBehaviour == null)
                    uprightSpringBehaviour = animator.GetBehaviour<UprightSpringBehaviour>();

                if (capsuleCollider == null)
                    capsuleCollider = animator.GetComponent<CapsuleCollider>();

                defaultCapsuleCenter = capsuleCollider.center;
                defaultCapsuleHeigth = capsuleCollider.height;

                lastTimeUse = Time.time;

                dragDefault = heightSpringBehaviour.Body.drag;
                dragModifier = dashConfig.Distance * dashConfig.Duration;

                forward = Vector3.ProjectOnPlane(animator.transform.forward, Vector3.up).normalized;
                heightSpringBehaviour.Body.drag = 0;
                heightSpringBehaviour.Body.constraints = RigidbodyConstraints.FreezeRotation;

                lerpDuration = dashConfig.Duration;
            }

            if (stateInfo.tagHash == loopTagHash)
            {
                capsuleCollider.center = dashConfig.CapsuleCenter;
                capsuleCollider.height = dashConfig.CapsuleHeight;
                Vector3 force = forward * heightSpringBehaviour.Body.mass * dashConfig.Distance / dashConfig.Duration;
                heightSpringBehaviour.Body.AddForce(force, ForceMode.Impulse);
                heightSpringBehaviour.Body.angularVelocity -= heightSpringBehaviour.Body.angularVelocity;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.tagHash != loopTagHash) return;

            heightSpringBehaviour.Body.angularVelocity -= heightSpringBehaviour.Body.angularVelocity;
            heightSpringBehaviour.Body.useGravity = false;
            heightSpringBehaviour.ShouldMaintainHeight = false;

            lastTimeUse = Time.time;

            // Aca se modifica el uso del paraguas...
            animator.SetBool(groundedHash, dashConfig.UseUmbrella ? heightSpringBehaviour.GroundedInfo.grounded : true);

            if (lerpDuration > 0)
            {
                heightSpringBehaviour.Body.drag = Mathf.Lerp(0, dragModifier, dashConfig.VelocityBehaviourCurve.Evaluate((dashConfig.Duration - lerpDuration) / dashConfig.Duration));
                lerpDuration -= Time.deltaTime;
            }
            else
            {
                heightSpringBehaviour.Body.drag = dragDefault;
                animator.SetBool(dashHash, false);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.tagHash != exitTagHash) return;

            heightSpringBehaviour.Body.angularVelocity -= heightSpringBehaviour.Body.angularVelocity;
            capsuleCollider.center = defaultCapsuleCenter;
            capsuleCollider.height = defaultCapsuleHeigth;

            animator.SetBool(umbrellaHash, !animator.GetBool(groundedHash));

            lerpDuration = 0;
            heightSpringBehaviour.Body.constraints = RigidbodyConstraints.None;
            heightSpringBehaviour.Body.drag = 0;
            heightSpringBehaviour.Body.useGravity = true;
            heightSpringBehaviour.ShouldMaintainHeight = true;

            heightSpringBehaviour.Body.velocity -= heightSpringBehaviour.Body.velocity;

            uprightSpringBehaviour.LockDirection = animator.GetBool(umbrellaHash);

            lastTimeUse = Time.time;
        }

        public void Dash(Animator animator)
        {
            if (animator.GetBool(stompHash)) return;

            if ((Time.time - lastTimeUse) < dashConfig.Cooldown) return;

            animator.SetBool(dashHash, true);
        }
    }
}