using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherGuardian.Gameplay;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light sunLight;
    [SerializeField] private Light newDirectionalLight;


    private void Start()
    {
        WeatherMachine.OnMachineOff += WeatherMachine_OnMachineOff;
    }

    private void WeatherMachine_OnMachineOff(object sender, System.EventArgs e)
    {
        sunLight.gameObject.SetActive(true);
        newDirectionalLight.gameObject.SetActive(false);
    }


}
