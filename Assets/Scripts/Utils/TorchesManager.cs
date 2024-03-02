using UnityEngine;
using WeatherGuardian.Gameplay;

namespace WeatherGuardian.Utils
{
    public class TorchesManager : MonoBehaviour
    {
        [SerializeField] TorchControl[] torches;

        private void Start()
        {
            WeatherMachine.OnMachineOff += WeatherMachine_OnMachineOff;
        }

        private void WeatherMachine_OnMachineOff(object sender, System.EventArgs e)
        {
            TorchesOff();
        }

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