using UnityEngine;

namespace WeatherGuardian.Utils
{   
    public class PressSoundController : MonoBehaviour
    {
        [SerializeField] private FMODUnity.EventReference stoneHitSfx;

        [SerializeField] private FMODUnity.EventReference nimbusHitSfx;

        [SerializeField] private string nimbusTag;

        [SerializeField] private string stoneTag;        

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
    }
}