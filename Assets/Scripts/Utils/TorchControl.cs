using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class TorchControl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Light torchLight;

        private ParticleSystem.EmissionModule emissionModule;

        private void Awake()
        {
            emissionModule = particle.emission;
            TorchOff();
        }

        public void TorchOn()
        {
            if (!gameObject.activeInHierarchy) return;
            emissionModule.enabled = true;
            torchLight.enabled = true;
        }

        public void TorchOff()
        {
            if (!gameObject.activeInHierarchy) return;
            emissionModule.enabled = false;
            torchLight.enabled = false;
        }
    }
}