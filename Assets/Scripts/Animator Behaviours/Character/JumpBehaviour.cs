using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class JumpBehaviour : StateMachineBehaviour
    {
        int jumpHash = Animator.StringToHash("JUMP");
        int groundedHash = Animator.StringToHash("GROUNDED");

        [Header("Jump:")]
        [SerializeField] JumpConfig jumpConfig;

        [Header("Animation Exit Time")]
        [SerializeField, Range(0, 1)] float exitTimeNormalized = 0.2f;

        private HeightSpringBehaviour heightSpringBehaviour;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (heightSpringBehaviour.LastGroundedTime > jumpConfig.CoyoteTime) return;

            if (stateInfo.normalizedTime < exitTimeNormalized) return;

            heightSpringBehaviour.ShouldMaintainHeight = false;
            heightSpringBehaviour.Body.AddForce(Vector3.up * jumpConfig.JumpForceFactor, ForceMode.Impulse);

            animator.SetBool(groundedHash, false);
            animator.SetBool(jumpHash, false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(jumpHash, false);
        }

        public void Jump(Animator animator)
        {
            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            if (!heightSpringBehaviour.GroundedInfo.grounded) return;

            animator.SetBool(jumpHash, true);
        }
    }
}