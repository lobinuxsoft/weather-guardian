using UnityEngine;
using WeatherGuardian.Utils;

public class BellSoundsController : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference bellSfx;

    [SerializeField] [Range(0.01f, 0.2f)] private float minBellTimeSensibility;
    [SerializeField] [Range(0.8f, 0.99f)] private float maxBellTimeSensibility;

    [SerializeField] private Pendullum pendullumBehaviour;

    private bool soundTriggered = false;
    
    // Update is called once per frame
    void Update()
    {
        if (pendullumBehaviour.PendullumTime < minBellTimeSensibility || pendullumBehaviour.PendullumTime > maxBellTimeSensibility) 
        {
            if (!bellSfx.IsNull && !soundTriggered)
                FMODUnity.RuntimeManager.PlayOneShotAttached(bellSfx, gameObject);

            soundTriggered = true;
        }
        else
        {
            soundTriggered = false;
        }
    }
}
