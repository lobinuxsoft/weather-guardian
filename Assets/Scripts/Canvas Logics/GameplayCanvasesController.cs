using UnityEngine;

namespace WeatherGuardian.CanvasManager
{
    [RequireComponent(typeof(PauseMenuNavigationSelector))]
    public class GameplayCanvasesController : MonoBehaviour
    {
        [SerializeField] CanvasVisibilityController pauseCanvasController;
        
        [SerializeField] CanvasVisibilityController settingsCanvasController;

        [SerializeField] Canvas endGameCanvasController;



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
            PortalBehaviour.OnPortalTouch += PortalBehaviour_OnPortalTouch;

            if (pauseCanvasController == null) 
            {
                Debug.Log("No pause canvas controller was selected!");
            }


            if (settingsCanvasController == null) 
            {
                Debug.Log("No settings canvas controller was selected!");
            }
        }

        public void PortalBehaviour_OnPortalTouch(object sender, System.EventArgs e)
        {
            endGameCanvasController.gameObject.SetActive(true);
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

        private void OnDestroy()
        {
            PortalBehaviour.OnPortalTouch -= PortalBehaviour_OnPortalTouch;

        }
    }
}