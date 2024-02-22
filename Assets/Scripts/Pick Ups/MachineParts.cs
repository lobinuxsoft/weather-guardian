using UnityEngine;

namespace WeatherGuardian.PickUps 
{
    public class MachineParts : MonoBehaviour, ICollectable
    {
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

            Destroy(gameObject, 0.5f);       
        }        
    }
}