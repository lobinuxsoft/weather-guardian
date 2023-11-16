using UnityEngine;
using WeatherGuardian.Obstacles;

namespace WeatherGuardian.Utils
{   
    public class PressSoundController : MonoBehaviour
    {
        [SerializeField] private HorizontalObstacle horizontalObstacle;

        [SerializeField] private FMODUnity.EventReference stoneHitSfx;

        [SerializeField] private FMODUnity.EventReference nimbusHitSfx;

        [SerializeField] private FMODUnity.EventReference stoneLoopSfx;

        [SerializeField] private string nimbusTag;

        [SerializeField] private string stoneTag;

        private void Start()
        {
            if (horizontalObstacle != null)
                horizontalObstacle.OnStartMoving += LoopSfxBehaviour;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == nimbusTag) 
            {
                if (!nimbusHitSfx.IsNull)
                    FMODUnity.RuntimeManager.PlayOneShot(nimbusHitSfx);
            }

            if (other.transform.tag == stoneTag)
            {
                if (!stoneHitSfx.IsNull)
                    FMODUnity.RuntimeManager.PlayOneShotAttached(stoneHitSfx, gameObject);
            }
        }

        private void OnDestroy()
        {
            if (horizontalObstacle != null)
                horizontalObstacle.OnStartMoving -= LoopSfxBehaviour;
        }

        private void LoopSfxBehaviour() 
        {
            if (!stoneLoopSfx.IsNull)
                FMODUnity.RuntimeManager.PlayOneShotAttached(stoneLoopSfx, gameObject);
        } 
    }
}