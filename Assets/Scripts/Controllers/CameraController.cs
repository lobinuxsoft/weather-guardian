using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WeatherGuardian.CameraControls
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] InputActionReference switchAction;

        [SerializeField] CinemachineVirtualCamera isometricCamera;
        [SerializeField] CinemachineFreeLook freeCamera;

        [SerializeField] Transform cameraTarget;

        private void OnEnable()
        {
            switchAction.action.started += SwitchCamera;

            switchAction.action.Enable();

            isometricCamera.Follow = cameraTarget;
            isometricCamera.LookAt = cameraTarget;

            freeCamera.Follow = cameraTarget;
            freeCamera.LookAt = cameraTarget;
        }

        public void OnDisable()
        {
            switchAction.action.started -= SwitchCamera;

            switchAction.action.Disable();
        }

        private void SwitchCamera(InputAction.CallbackContext context)
        {
            if(isometricCamera.gameObject.activeSelf)
            {
                freeCamera.gameObject.SetActive(true);
                isometricCamera.gameObject.SetActive(false);
            }
            else
            {
                isometricCamera.gameObject.SetActive(true);
                freeCamera.gameObject.SetActive(false);
            }
        }
    }
}