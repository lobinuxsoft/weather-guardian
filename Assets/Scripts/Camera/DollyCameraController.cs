using Cinemachine;
using UnityEngine;
using WeatherGuardian.Behaviours;

[RequireComponent(typeof(Collider))]
public class DollyCameraController : MonoBehaviour
{
    const string playerTag = "Player";

    [SerializeField] private CinemachineVirtualCamera dollyCamera;

    private Collider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();

        myCollider.isTrigger = true;

        DisableCamera();
    }

    private void OnEnable() => DeathBehaviour.onDeath += DisableCamera;

    private void OnDisable() => DeathBehaviour.onDeath -= DisableCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == playerTag)
        {
            dollyCamera.Follow = other.transform;
            dollyCamera.LookAt = other.transform;
            EnableCamera();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == playerTag)
            DisableCamera();
    }

    private void EnableCamera() => dollyCamera.gameObject.SetActive(true);

    private void DisableCamera() => dollyCamera.gameObject.SetActive(false);
}