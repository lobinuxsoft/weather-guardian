using CryingOnion.GizmosRT.Runtime;
using System;
using UnityEngine;
using UnityEngine.Playables;
using WeatherGuardian.Behaviours;

public class CharacterEffectController : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem stepsParticles;
    [SerializeField] private PlayableDirector jumpParticles;
    [SerializeField] private PlayableDirector landParticles;

    [Space(5)]
    [Header("Only for debug mode.")]
    [SerializeField] private Gradient normalGradient;
    [SerializeField] private Gradient forwardGradient;

    [Header("FMOD Events")]
    [SerializeField] private FMODUnity.EventReference footstepSfx;
    [SerializeField] private FMODUnity.EventReference landingSfx;
    [SerializeField] private FMODUnity.EventReference jumpSfx;
    [SerializeField] private FMODUnity.EventReference openUmbrellaSfx;
    [SerializeField] private FMODUnity.EventReference closeUmbrellaSfx;
    [SerializeField] private FMODUnity.EventReference dashSfx;

    Animator animator;
    HeightSpringBehaviour heightSpringBehaviour;

    Vector3 center;

    Guid normalId = Guid.NewGuid();
    Vector3 normal;

    Guid forwardId = Guid.NewGuid();
    Vector3 forward;

    private void Start()
    {
        animator = GetComponent<Animator>();
        heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();
    }

    private void LateUpdate()
    {
        center = heightSpringBehaviour.GroundedInfo.rayHit.point;
        normal = heightSpringBehaviour.GroundedInfo.rayHit.normal;
        forward = Vector3.ProjectOnPlane(transform.forward, normal != Vector3.zero ? normal : animator.transform.up).normalized;
        GizmosRT.Arrow(normalId, center, normal != Vector3.zero ? normal : animator.transform.up, 1, 1, normalGradient);
        GizmosRT.Arrow(forwardId, center, forward, 1, 1, forwardGradient);
    }

    public void FootStepEffect()
    {
        stepsParticles.transform.position = center;
        stepsParticles.transform.up = normal;
        stepsParticles.transform.forward = forward;
        stepsParticles.Play();

        if (!footstepSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(footstepSfx, gameObject);
    }

    public void JumpEffect()
    {
        jumpParticles.transform.position = center;
        jumpParticles.transform.up = normal;
        jumpParticles.transform.forward = forward;
        jumpParticles.Play();

        if (!jumpSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(jumpSfx, gameObject);
    }

    public void LandEffect()
    {
        landParticles.transform.position = center;
        landParticles.transform.up = normal;
        landParticles.transform.forward = forward;
        landParticles.Play();

        if (!landingSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(landingSfx, gameObject);
    }

    public void OpenUmbrellaEffect()
    {
        if (!openUmbrellaSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(openUmbrellaSfx, gameObject);
    }

    public void CloseUmbrellaEffect()
    {
        if (!closeUmbrellaSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(closeUmbrellaSfx, gameObject);
    }

    public void DashEffect()
    {
        if (!dashSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(dashSfx, gameObject);
    }
}