using UnityEngine;

using FMODUnity;

namespace WeatherGuardian.PickUps 
{
    public class MachineParts : MonoBehaviour, ICollectable
    {
        [SerializeField] private EventReference pickUpSFX;

        [SerializeField] ParticleSystem particleSystem;

        private void Start()
        {
            if (particleSystem != null) 
            {
                particleSystem.Stop();
            }
        }

        void ICollectable.PickedUp()
        {
            particleSystem.Play();

            if (!pickUpSFX.IsNull)
                RuntimeManager.PlayOneShot(pickUpSFX);

            Destroy(gameObject, 0.5f);       
        }        
    }
}