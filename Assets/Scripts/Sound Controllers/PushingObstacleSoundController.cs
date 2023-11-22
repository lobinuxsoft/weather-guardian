using UnityEngine;
using WeatherGuardian.Utils;

public class PushingObstacleSoundController : MonoBehaviour
{
    [SerializeField] private GameObject soundTriggerer;

    [SerializeField] private SplineFollowPath splineFollowPath;

    [SerializeField] private FMODUnity.EventReference moveSfx;

    [SerializeField] private FMODUnity.EventReference hitAirSfx;

    private ColliderDetector colliderDetector;

    private Collider triggerSoundCollider;

    private void Awake()
    {
        colliderDetector = soundTriggerer.GetComponent<ColliderDetector>();

        triggerSoundCollider = soundTriggerer.GetComponent<Collider>();

        if (triggerSoundCollider != null) 
        {
            triggerSoundCollider.enabled = false;
            triggerSoundCollider.isTrigger = true;
        }
    }

    void Start()
    {
        colliderDetector.onEnter += TriggerStoneLoopSound;

        colliderDetector.onExit += TriggerStoneAirStrikeSound;

        if (splineFollowPath.TimerToStartMoving != null) 
        {
            splineFollowPath.TimerToStartMoving.OnTimerEnds += EnableSoundTrigger;
        }
        else 
        {
            EnableSoundTrigger();
        }
    }

    private void OnDestroy()
    {
        colliderDetector.onEnter -= TriggerStoneLoopSound;

        colliderDetector.onExit -= TriggerStoneAirStrikeSound;

        if (splineFollowPath.TimerToStartMoving != null)
            splineFollowPath.TimerToStartMoving.OnTimerEnds -= EnableSoundTrigger;
    }

    private void TriggerStoneLoopSound(GameObject gameObject)
    {
        if (!moveSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(moveSfx, this.gameObject);
    }

    private void TriggerStoneAirStrikeSound(GameObject gameObject)
    {
        if (!hitAirSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(hitAirSfx, this.gameObject);
    }

    private void EnableSoundTrigger() 
    {
        triggerSoundCollider.enabled = true;
    }
}