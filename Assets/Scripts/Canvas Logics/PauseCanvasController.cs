using UnityEngine;
using UnityEngine.InputSystem;

namespace WeatherGuardian.CanvasManager
{
    [RequireComponent(typeof(GameplayCanvasesController))]
    public class PauseCanvasController : CanvasVisibilityController
    {
        [SerializeField] StartGameMissionCanvas startGameMissionCanvas;
        private GameplayCanvasesController gameplayCanvasesController;
        private bool canPause;


        private void Awake()
        {
            gameplayCanvasesController = GetComponent<GameplayCanvasesController>();
        }

        private void Start()
        {
            startGameMissionCanvas.OnStartGame += StartGameMissionCanvas_OnStartGame;
        }

        private void StartGameMissionCanvas_OnStartGame(object sender, System.EventArgs e)
        {
            canPause = true;
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
            if (callbackContext.performed && canPause) 
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