using System.IO;
using UnityEngine;
using WeatherGuardian.Utils;

public class SpikesBehaviours : MonoBehaviour
{
    [SerializeField] private KillerCollider[] spikesColliders;

    [SerializeField] private FMODUnity.EventReference showSpikesSfx;    

    [SerializeField] private FMODUnity.EventReference hideSpikesSfx;

    [SerializeField] private FMODUnity.EventReference deathSpikesSfx;

    [SerializeField][Range(0.01f, 10.0f)] private  float inRevealedPositionDuration = 1.0f;

    [SerializeField] private Vector3 revealedPosition;
    [SerializeField] private Vector3 hidePosition;

    private Timer timer;

    private void Awake()
    {
        timer = new Timer(inRevealedPositionDuration, false);

        timer.OnTimerEnds += HideSpikes;

        for (short i = 0; i < spikesColliders.Length; i++) 
        {
            spikesColliders[i].OnEnter += TriggerDeathSfx;
        }
    }

    private void Start()
    {
        transform.localPosition = hidePosition;
    }

    private void OnDestroy()
    {
        timer.OnTimerEnds -= HideSpikes;

        for (short i = 0; i < spikesColliders.Length; i++)
        {
            spikesColliders[i].OnEnter -= TriggerDeathSfx;
        }
    }

    private void Update()
    {
        timer.UpdateTimerWithReset();
    }

    public void ShowSpikes()
    {
        transform.localPosition = revealedPosition;

        timer.IsRunning = true;

        if (!showSpikesSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(showSpikesSfx, gameObject);
    }

    public void HideSpikes()
    {
        transform.localPosition = hidePosition;

        timer.IsRunning = false;

        if (!hideSpikesSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(hideSpikesSfx, gameObject);
    }
    
    private void TriggerDeathSfx() 
    {
        if (!hideSpikesSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(deathSpikesSfx);
    }
}