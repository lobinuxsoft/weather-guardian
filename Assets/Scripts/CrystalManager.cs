using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherGuardian.Gameplay;

public class CrystalManager : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> crystalMeshRenderersList;
    [SerializeField] private Material noLightCrystalMaterial;

    private void Start()
    {
       WeatherMachine.OnMachineOff += WeatherMachine_OnMachineOff;

    }

    private void WeatherMachine_OnMachineOff(object sender, System.EventArgs e)
    {
        foreach (MeshRenderer material in crystalMeshRenderersList)
        {
            material.material = noLightCrystalMaterial;
        }
    }


}
