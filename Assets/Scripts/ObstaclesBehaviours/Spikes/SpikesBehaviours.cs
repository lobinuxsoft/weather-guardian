using UnityEngine;
using WeatherGuardian.Utils;

public class SpikesBehaviours : MonoBehaviour
{
    [SerializeField][Range(0.01f, 10.0f)] private  float inRevealedPositionDuration = 1.0f;

    [SerializeField] private Vector3 revealedPosition;
    [SerializeField] private Vector3 hidePosition;

    private Timer timer;

    private void Awake()
    {
        timer = new Timer(inRevealedPositionDuration, false);

        timer.OnTimerEnds += HideSpikes;
    }

    private void Start()
    {
        transform.localPosition = hidePosition;
    }

    private void OnDestroy()
    {
        timer.OnTimerEnds -= HideSpikes;
    }

    private void Update()
    {
        timer.UpdateTimerWithReset();
    }

    public void ShowSpikes()
    {
        transform.localPosition = revealedPosition;

        timer.IsRunning = true;
    }

    public void HideSpikes()
    {
        transform.localPosition = hidePosition;

        timer.IsRunning = false;
    }
}