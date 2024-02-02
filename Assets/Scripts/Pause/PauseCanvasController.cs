using UnityEngine;

namespace WeatherGuardian.Pause
{
    [RequireComponent(typeof(PauseInputDetection), typeof(Canvas))]
    public class PauseCanvasController : MonoBehaviour
    {
        private Canvas pauseCanvas;

        private PauseInputDetection pauseInputDetection;

        void Awake()
        {
            pauseCanvas = GetComponent<Canvas>();

            pauseInputDetection = GetComponent<PauseInputDetection>();
            
            pauseCanvas.gameObject.SetActive(false);

            pauseInputDetection.onPauseTriggered += SwitchCanvasVisibility;
        }

        private void OnDestroy()
        {
            pauseInputDetection.onPauseTriggered -= SwitchCanvasVisibility;
        }

        public void SwitchCanvasVisibility() 
        {
            pauseCanvas.gameObject.SetActive(pauseCanvas.gameObject.activeSelf);
        }
    }
}