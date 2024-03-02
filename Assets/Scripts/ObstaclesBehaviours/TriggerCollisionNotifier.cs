using System;

using UnityEngine;

public enum ObjectInitialStatus { STARTS_ENABLE, STARTS_DISABLE };

[RequireComponent(typeof(Collider))]
public class TriggerCollisionNotifier : MonoBehaviour
{
    [Tooltip("Tag of object that can trigger obstacle")]
    [SerializeField] private string[] activatorObjectTag;

    [Tooltip("Starting active condition of the object")]
    [SerializeField] private ObjectInitialStatus triggerInitialStatus;

    Collider myCollider;

    public event Action OnTriggerCollisionDetected;

    private bool active;

    public bool Active 
    {
        set 
        {
            active = value;

            myCollider.enabled = value;
        }
        get 
        {
            return active;
        }
    }

    private void Awake()
    {
        myCollider = GetComponent<Collider>();

        myCollider.isTrigger = true;

        switch (triggerInitialStatus) 
        {
            case ObjectInitialStatus.STARTS_ENABLE:

                Active = true;

                break;
            case ObjectInitialStatus.STARTS_DISABLE:

                Active = false;

                break;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < activatorObjectTag.Length; i++) 
        {
            if (other.transform.tag == activatorObjectTag[i]) 
            {
                OnTriggerCollisionDetected?.Invoke();
            }
        }
    }
}