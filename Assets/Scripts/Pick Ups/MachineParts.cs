using UnityEngine;

using FMODUnity;

namespace WeatherGuardian.PickUps 
{
    public class MachineParts : MonoBehaviour, ICollectable
    {
        [SerializeField] private EventReference pickUpSFX;

        [SerializeField] private EventReference MachinePartSFX;

        [SerializeField] ParticleSystem particleSystem;

        [SerializeField] [Range(0.01f, 1.0f)] private float vanishSpeed;

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

            if (!MachinePartSFX.IsNull)
                RuntimeManager.PlayOneShot(MachinePartSFX);

            Destroy(gameObject, vanishSpeed);      
        }        
    }
}