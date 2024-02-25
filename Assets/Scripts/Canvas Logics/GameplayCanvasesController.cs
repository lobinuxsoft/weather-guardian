using UnityEngine;

namespace WeatherGuardian.CanvasManager
{
    [RequireComponent(typeof(PauseMenuNavigationSelector))]
    public class GameplayCanvasesController : MonoBehaviour
    {
        [SerializeField] CanvasVisibilityController pauseCanvasController;
        
        [SerializeField] CanvasVisibilityController settingsCanvasController;

        PauseMenuNavigationSelector navigationSelector;

        public CanvasVisibilityController SettingsCanvasController 
        {
            get 
            {
                return settingsCanvasController;
            }
        }

        private void Awake()
        {
            navigationSelector = GetComponent<PauseMenuNavigationSelector>();
        }

        private void Start()
        {
            if (pauseCanvasController == null) 
            {
                Debug.Log("No pause canvas controller was selected!");
            }


            if (settingsCanvasController == null) 
            {
                Debug.Log("No settings canvas controller was selected!");
            }
        }

        public void ShowPauseCanvas() 
        {
            if (pauseCanvasController != null && settingsCanvasController != null) 
            {
                pauseCanvasController.ShowCanvas();

                settingsCanvasController.HideCanvas();

                navigationSelector.NavigateBetweenPauseButtons();
            }
        }

        public void ShowSettingsCanvas() 
        {
            if (pauseCanvasController != null && settingsCanvasController != null)
            {
                pauseCanvasController.HideCanvas();

                settingsCanvasController.ShowCanvas();

                navigationSelector.NavigateBetweenSettingsButtons();
            }
        }

        public void ResumeTime()
        {
            Time.timeScale = 1.0f;
        }
    }
}