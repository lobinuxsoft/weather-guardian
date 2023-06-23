using CryingOnion.GizmosRT.Runtime;
using System;
using UnityEngine;
using WeatherGuardian.Behaviours;
using WeatherGuardian.Behaviours.Configs;

namespace WeatherGuardian.Utils
{
    public class CharacterDebugGizmos : MonoBehaviour
    {
        private readonly Guid id1 = Guid.NewGuid();
        private readonly Guid id2 = Guid.NewGuid();

        [SerializeField] private HeightSpringConfig heightSpringConfig;
        [SerializeField] private MoveConfig moveConfig;
        [SerializeField] private Gradient primaryGradient;
        [SerializeField] private Gradient moveGradient;
        [SerializeField] private Gradient raycastGradient;

        private Animator animator;
        private HeightSpringBehaviour hsb;

        private CapsuleCollider capsuleCollider;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            hsb = animator.GetBehaviour<HeightSpringBehaviour>();

            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void LateUpdate()
        {
            if (GizmosRT.Enabled)
            {
                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
                Vector3 velocityDir = Vector3.ProjectOnPlane(hsb.Body.velocity, Vector3.up);

                Vector3 pos = transform.position + transform.up * capsuleCollider.center.y;

                GizmosRT.Arrow(id1, pos, forward, 1, Vector3.ClampMagnitude(forward, 2).magnitude, primaryGradient);

                Matrix4x4 capsuleMatrix = Matrix4x4.TRS(pos, transform.rotation, transform.localScale);
                GizmosRT.Capsule(id1, capsuleMatrix, primaryGradient.Evaluate(0), capsuleCollider.height * 0.5f, capsuleCollider.radius);

                float lerp = Mathf.Clamp01(velocityDir.magnitude / moveConfig.MaxSpeed);

                GizmosRT.Arrow(id2, pos, velocityDir.normalized, 1, lerp * 2, moveGradient);

                GizmosRT.Circle(
                    id2,
                    pos,
                    Quaternion.LookRotation(forward),
                    moveGradient.Evaluate(lerp),
                    lerp * 4,
                    Vector3.SignedAngle(forward, velocityDir, Vector3.up)
                    );

                GizmosRT.Sphere(
                    hsb.GroundedInfo.rayHit.point + hsb.GroundedInfo.rayHit.normal * heightSpringConfig.RayCastRadius * 0.5f,
                    heightSpringConfig.RayCastRadius,
                    raycastGradient.Evaluate(hsb.GroundedInfo.rayHit.distance / heightSpringConfig.RayCastLength)
                    );
            }
        }
    }
}