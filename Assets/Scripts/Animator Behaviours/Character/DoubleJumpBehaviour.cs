using WeatherGuardian.Behaviours.Configs;
using UnityEngine;

namespace WeatherGuardian.Behaviours
{
    public class DoubleJumpBehaviour : StateMachineBehaviour
    {
        int jumpHash = Animator.StringToHash("JUMP");

        [Header("Jump:")]
        [SerializeField] JumpConfig jumpConfig;

        private HeightSpringBehaviour heightSpringBehaviour;
        float verticalVelocity = 0.0f;

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            heightSpringBehaviour.ShouldMaintainHeight = false;
            heightSpringBehaviour.Body.AddForce(Vector3.up * jumpConfig.JumpForceFactor, ForceMode.Impulse);

            animator.SetBool(jumpHash, false);

            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Jump");
        }

        public void Jump(Animator animator)
        {
            if (heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            if (heightSpringBehaviour.GroundedInfo.grounded) return;

            verticalVelocity = Vector3.Dot(heightSpringBehaviour.Body.velocity, Vector3.up);

            if (verticalVelocity < 0.0f) return;

            animator.SetBool(jumpHash, true);
        }
    }
}