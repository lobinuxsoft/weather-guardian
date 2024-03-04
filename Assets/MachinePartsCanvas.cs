using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WeatherGuardian.PickUps;
using static WeatherGuardian.PickUps.ItemCollector;

public class MachinePartsCanvas : MonoBehaviour
{
    [SerializeField] ItemCollector itemCollector;
    [SerializeField] TMP_Text totalAmmount;
    // Start is called before the first frame update
    void Start()
    {
        itemCollector.OnItemCollected += ItemCollector_OnItemCollected;
    }

    private void ItemCollector_OnItemCollected(object sender, OnItemCollectedEventArgs e)
    {
        totalAmmount.text = e.amountPartsCollected.ToString();
    }

}
