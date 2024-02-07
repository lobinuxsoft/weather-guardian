using UnityEngine;

namespace WeatherGuardian.CanvasManager
{
    public class GameplayCanvasesController : MonoBehaviour
    {
        [SerializeField] CanvasVisibilityController pauseCanvasController;
        
        [SerializeField] CanvasVisibilityController settingsCanvasController;

        public CanvasVisibilityController SettingsCanvasController 
        {
            get 
            {
                return settingsCanvasController;
            }
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
            }
        }

        public void ShowSettingsCanvas() 
        {
            if (pauseCanvasController != null && settingsCanvasController != null)
            {
                pauseCanvasController.HideCanvas();

                settingsCanvasController.ShowCanvas();
            }
        }
    }
}