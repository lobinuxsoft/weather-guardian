using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherGuardian.CameraControls;

[RequireComponent(typeof(Collider))]
public class DollyCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera dollyCamera;
       
    private Collider myCollider;

    private void Awake()
    {    
        myCollider = GetComponent<Collider>();

        myCollider.isTrigger = true;

        dollyCamera.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player") 
        {   
            dollyCamera.gameObject.SetActive(true);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player") 
        {    
            dollyCamera.gameObject.SetActive(false);
        }
    }
}
