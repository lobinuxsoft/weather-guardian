using UnityEngine;
using WeatherGuardian.Utils;

public class SpikesManager : MonoBehaviour
{
    private TriggerCollisionNotifier activationTrigger;

    private SpikesBehaviours spikesBehaviours;

    [SerializeField] [Range(0.1f, 10.0f)] private float timeBeforeActivatingTrap = 1f;

    private Timer timer;

    private void Awake()
    {
        activationTrigger = gameObject.GetComponentInChildren<TriggerCollisionNotifier>(true);

        spikesBehaviours = gameObject.GetComponentInChildren<SpikesBehaviours>(true);

        if (activationTrigger != null) 
        {
            activationTrigger.OnTriggerCollisionDetected += StartTimer;
        }        
        
        timer = new Timer(timeBeforeActivatingTrap, false);

        timer.OnTimerEnds += spikesBehaviours.ShowSpikes;

        timer.OnTimerEnds += StopTimer;
    }

    private void OnDestroy()
    {
        if (activationTrigger != null)
        {
            activationTrigger.OnTriggerCollisionDetected -= StartTimer;
        }

        timer.OnTimerEnds -= spikesBehaviours.ShowSpikes;

        timer.OnTimerEnds -= StopTimer;
    }

    private void Update()
    {
        timer.UpdateTimerWithReset();
    }

    private void StartTimer() 
    {
        timer.IsRunning = true;
    }

    private void StopTimer() 
    {
        timer.IsRunning = false;
    }
}