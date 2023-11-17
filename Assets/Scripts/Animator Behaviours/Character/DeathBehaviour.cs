using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class DeathBehaviour : StateMachineBehaviour
    {
        private readonly int deathHash = Animator.StringToHash("DEATH");

        [Header("Death:")]
        [SerializeField] private DeathConfig deathConfig;

        private HeightSpringBehaviour hsb;
        private CapsuleCollider capsuleCollider;

        private Vector3 defaultCapsuleCenter = Vector3.zero;
        private float defaultCapsuleHeigth = 0;

        public void Death(Animator animator)
        {
            if(!hsb)
                hsb = animator.GetBehaviour<HeightSpringBehaviour>();

            if (!capsuleCollider)
            {
                capsuleCollider = animator.GetComponent<CapsuleCollider>();
                defaultCapsuleCenter = capsuleCollider.center;
                defaultCapsuleHeigth = capsuleCollider.height;
            }

            capsuleCollider.center = deathConfig.CapsuleCenter;
            capsuleCollider.height = deathConfig.CapsuleHeight;

            hsb.Body.constraints = RigidbodyConstraints.FreezeRotation;
            //hsb.Body.velocity = Vector3.zero;
            animator.SetBool(deathHash, true);
        }

        public void Revive(Animator animator)
        {
            hsb.Body.constraints = RigidbodyConstraints.None;
            animator.SetBool(deathHash, false);
            capsuleCollider.center = defaultCapsuleCenter;
            capsuleCollider.height = defaultCapsuleHeigth;
        }
    }
}