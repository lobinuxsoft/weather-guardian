using System;
using UnityEngine;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Behaviours
{
    public class DeathBehaviour : StateMachineBehaviour
    {
        private readonly int jumpHash = Animator.StringToHash("JUMP");
        private readonly int umbrellaHash = Animator.StringToHash("UMBRELLA");
        private readonly int stompHash = Animator.StringToHash("STOMP");
        private readonly int dashHash = Animator.StringToHash("DASH");
        private readonly int interactHash = Animator.StringToHash("INTERACT");
        private readonly int deathHash = Animator.StringToHash("DEATH");

        [Header("Death:")]
        [SerializeField] private DeathConfig deathConfig;

        private HeightSpringBehaviour hsb;
        private CapsuleCollider capsuleCollider;

        
        private Vector3 defaultCapsuleCenter = Vector3.zero;
        private float defaultCapsuleHeigth = 0;

        public bool IsDeath { get; private set; } = false;

        public static event Action onDeath;

        public void Death(Animator animator)
        {
            IsDeath = true;

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

            hsb.Body.linearDamping = 0.0f;
            hsb.Body.constraints = RigidbodyConstraints.FreezeRotation;

            animator.SetBool(jumpHash, false);
            animator.SetBool(umbrellaHash, false);
            animator.SetBool(stompHash, false);
            animator.SetBool(dashHash, false);
            animator.SetBool(interactHash, false);
            animator.SetBool(deathHash, IsDeath);

            onDeath?.Invoke();
        }

        public void Revive(Animator animator)
        {
            IsDeath = false;
            hsb.Body.linearDamping = 0.0f;
            hsb.Body.constraints = RigidbodyConstraints.None;
            animator.SetBool(deathHash, IsDeath);
            animator.SetBool(jumpHash, false);
            animator.SetBool(umbrellaHash, false);
            animator.SetBool(stompHash, false);
            animator.SetBool(dashHash, false);
            animator.SetBool(interactHash, false);
            capsuleCollider.center = defaultCapsuleCenter;
            capsuleCollider.height = defaultCapsuleHeigth;
        }
    }
}