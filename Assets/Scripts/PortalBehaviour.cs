using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public static EventHandler OnPortalTouch;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entro al portal");
        OnPortalTouch?.Invoke(this, EventArgs.Empty);
    }
}
