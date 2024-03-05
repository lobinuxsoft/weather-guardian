using UnityEngine;
using TMPro;
using System;

namespace WeatherGuardian.PickUps 
{
    public class ItemCollector : MonoBehaviour
    {
        int MachineParts = 0;
        [SerializeField] private int totalMachineParts;
        [SerializeField] TMP_Text machineAmmountText;
        [SerializeField] TMP_Text machineAmmountOnMachineText;

        public event EventHandler<OnItemCollectedEventArgs> OnItemCollected;

        public class OnItemCollectedEventArgs : EventArgs
        {
            public int amountPartsCollected;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MachinePart"))
            {
                other.gameObject.GetComponent<ICollectable>().PickedUp();
            
                MachineParts++;
                
                machineAmmountText.text = MachineParts.ToString();
                
                machineAmmountOnMachineText.text = machineAmmountOnMachineText.text;

                OnItemCollected?.Invoke(this, new OnItemCollectedEventArgs
                {
                    amountPartsCollected = MachineParts
                });
            }
        }

        public bool HasAllMachineParts() 
        { 
            return MachineParts >= totalMachineParts;
        }
    }
}
