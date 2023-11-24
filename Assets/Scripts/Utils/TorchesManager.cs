using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class TorchesManager : MonoBehaviour
    {
        [SerializeField] TorchControl[] torches;

        public void TorchesOn()
        {
            for (int i = 0; i < torches.Length; i++)
                torches[i].TorchOn();
        }

        public void TorchesOff()
        {
            for (int i = 0; i < torches.Length; i++)
                torches[i].TorchOff();
        }
    }
}