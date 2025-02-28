using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class AirJumpBehaviour : StateMachineBehaviour
    {
        private int jumpHash = Animator.StringToHash("JUMP");

        [Header("Air Jump:")]
        [SerializeField] private AirJumpConfig airJumpConfig;

        private HeightSpringBehaviour heightSpringBehaviour;
        private int jumpCount = 0;

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            heightSpringBehaviour.ShouldMaintainHeight = false;
            float verticalDot = Vector3.Dot(heightSpringBehaviour.Body.linearVelocity, Vector3.up);

            heightSpringBehaviour.Body.linearVelocity -= Vector3.up * verticalDot;
            heightSpringBehaviour.Body.AddForce(Vector3.up * airJumpConfig.JumpForceFactor, ForceMode.Impulse);

            animator.SetBool(jumpHash, false);
        }

        public void Jump(Animator animator)
        {
            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            if (heightSpringBehaviour.GroundedInfo.grounded)
            {
                jumpCount = 0;
                return;
            }

            if (jumpCount >= airJumpConfig.JumpAmount) return;

            jumpCount++;
            animator.SetBool(jumpHash, true);
        }
    }
}