using System;

using UnityEngine;

public class ObstacleActivationTrigger : MonoBehaviour
{
    [Tooltip("Tag of object triggering obstacle")]
    [SerializeField] private string ActivatorObjectTag;

    public event Action OnTriggerCollisionDetected;

    private bool active = true;

    public bool Active 
    {
        set 
        {
            active = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == ActivatorObjectTag && active) 
        {
            OnTriggerCollisionDetected?.Invoke();
        }
    }
}