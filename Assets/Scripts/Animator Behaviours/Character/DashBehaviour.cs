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
        float dragDefault = 0;
        float dragModifier = 0;
        Vector3 forward = Vector3.zero;

        float lerpDuration = 0.0f;
        float lastTimeUse = 0.0f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if (stateInfo.tagHash != enterTagHash) return;
            if (stateInfo.tagHash != loopTagHash) return;

            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            dragDefault = heightSpringBehaviour.Body.drag;
            dragModifier = dashConfig.Distance * dashConfig.Duration;

            forward = Vector3.ProjectOnPlane(animator.transform.forward, Vector3.up).normalized;
            heightSpringBehaviour.Body.drag = 0;
            heightSpringBehaviour.Body.constraints = RigidbodyConstraints.FreezeRotation;

            lerpDuration = dashConfig.Duration;

            Vector3 force = forward * heightSpringBehaviour.Body.mass * dashConfig.Distance / dashConfig.Duration;
            heightSpringBehaviour.Body.AddForce(force, ForceMode.Impulse);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.tagHash != loopTagHash) return;

            heightSpringBehaviour.Body.useGravity = false;
            heightSpringBehaviour.ShouldMaintainHeight = false;

            // Aca se modifica el uso del paraguas...
            animator.SetBool(
                groundedHash,
                dashConfig.UseUmbrella ? heightSpringBehaviour.GroundedInfo.grounded : true
                );

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

            animator.SetBool(umbrellaHash, !animator.GetBool(groundedHash));

            lerpDuration = 0;
            heightSpringBehaviour.Body.constraints = RigidbodyConstraints.None;
            heightSpringBehaviour.Body.useGravity = true;
            heightSpringBehaviour.ShouldMaintainHeight = true;

            heightSpringBehaviour.Body.velocity -= heightSpringBehaviour.Body.velocity;

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