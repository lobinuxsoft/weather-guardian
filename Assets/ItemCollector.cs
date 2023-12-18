using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    int MachineParts = 0;
    [SerializeField] private int totalMachineParts;
    [SerializeField] TMP_Text machineAmmountText;
    [SerializeField] TMP_Text machineAmmountOnMachineText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MachinePart"))
        {
            Destroy(other.gameObject);
            MachineParts++;
            machineAmmountText.text = MachineParts.ToString();
            machineAmmountOnMachineText.text = machineAmmountText.text;
        }
    }

    public bool HasAllMachineParts() 
    { 
        return MachineParts >= totalMachineParts;
    }
}
