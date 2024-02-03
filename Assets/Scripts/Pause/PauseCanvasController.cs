using UnityEngine;
using UnityEngine.InputSystem;

namespace WeatherGuardian.Pause
{    
    public class PauseCanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas pauseCanvas;        

        void Awake()
        {   
            pauseCanvas.gameObject.SetActive(false);
        }

        public void SwitchCanvasVisibility()
        {
            pauseCanvas.gameObject.SetActive(!pauseCanvas.gameObject.activeSelf);

            ResumeStopTime();            
        }

        public void SwitchCanvasVisibility(InputAction.CallbackContext callbackContext) 
        {
            if (callbackContext.performed) 
            {
                pauseCanvas.gameObject.SetActive(!pauseCanvas.gameObject.activeSelf);

                ResumeStopTime();
            }
        }

        private void ResumeStopTime() 
        {
            if (Time.timeScale <= 0.0f) 
            {
                Time.timeScale = 1.0f;
            }
            else 
            {
                Time.timeScale = 0.0f;
            }
        }
    }
}