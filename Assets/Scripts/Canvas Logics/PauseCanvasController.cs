using UnityEngine;
using UnityEngine.InputSystem;

namespace WeatherGuardian.CanvasManager
{
    [RequireComponent(typeof(GameplayCanvasesController))]
    public class PauseCanvasController : CanvasVisibilityController
    {
        private GameplayCanvasesController gameplayCanvasesController;

        private void Awake()
        {
            gameplayCanvasesController = GetComponent<GameplayCanvasesController>();
        }

        public void PauseUnpauseGame()
        {
            if (myCanvasObject.IsVisible)
            {
                HideCanvas();
            }
            else
            {
                if (!gameplayCanvasesController.SettingsCanvasController.myCanvasObject.IsVisible)
                {
                    ShowCanvas();
                }
            }

            gameplayCanvasesController.SettingsCanvasController.HideCanvas();

            ResumeStopTime();
        }

        public void PauseUnpauseGame(InputAction.CallbackContext callbackContext) 
        {
            if (callbackContext.performed) 
            {
                if (myCanvasObject.IsVisible) 
                {
                    HideCanvas();
                }
                else 
                {
                    if (!gameplayCanvasesController.SettingsCanvasController.myCanvasObject.IsVisible) 
                    {
                        ShowCanvas();
                    }
                }

                gameplayCanvasesController.SettingsCanvasController.HideCanvas();

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