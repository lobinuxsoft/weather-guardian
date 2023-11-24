using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class TorchControl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        private ParticleSystem.EmissionModule emissionModule;

        private void Awake()
        {
            emissionModule = particle.emission;
            TorchOff();
        }

        public void TorchOn() => emissionModule.enabled = true;

        public void TorchOff() => emissionModule.enabled = false;
    }
}