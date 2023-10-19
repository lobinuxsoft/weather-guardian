using CryingOnion.GizmosRT.Runtime;
using System;
using UnityEngine;
using UnityEngine.Playables;
using WeatherGuardian.Behaviours;
using WeatherGuardian.FMOD.Configs;

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

    [Header("FMOD Event Config")]
    [SerializeField] private SFXEventsConfigs sfxEvents;

    private Animator animator;
    private HeightSpringBehaviour heightSpringBehaviour;

    private Vector3 center;

    private Guid normalId = Guid.NewGuid();
    private Vector3 normal;

    private Guid forwardId = Guid.NewGuid();
    private Vector3 forward;

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

        if (!sfxEvents.footstepSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(sfxEvents.footstepSfx, gameObject);
    }

    public void JumpEffect()
    {
        jumpParticles.transform.position = center;
        jumpParticles.transform.up = normal;
        jumpParticles.transform.forward = forward;
        jumpParticles.Play();

        if (!sfxEvents.jumpSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(sfxEvents.jumpSfx, gameObject);
    }

    public void LandEffect()
    {
        landParticles.transform.position = center;
        landParticles.transform.up = normal;
        landParticles.transform.forward = forward;
        landParticles.Play();

        if (!sfxEvents.landingSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(sfxEvents.landingSfx, gameObject);
    }

    public void OpenUmbrellaEffect()
    {
        if (!sfxEvents.openUmbrellaSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(sfxEvents.openUmbrellaSfx, gameObject);

        if (!sfxEvents.glideSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(sfxEvents.glideSfx, gameObject);
    }

    public void CloseUmbrellaEffect()
    {
        if (!sfxEvents.closeUmbrellaSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(sfxEvents.closeUmbrellaSfx, gameObject);
    }

    public void DashEffect()
    {
        if (!sfxEvents.dashSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(sfxEvents.dashSfx, gameObject);
    }
}