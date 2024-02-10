using UnityEngine;
using UnityEngine.EventSystems;

namespace WeatherGuardian.CanvasManager 
{
    public class PauseMenuNavigationSelector : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;

        [SerializeField] private GameObject pauseMenuFirstButton;

        [SerializeField] private GameObject configMenuFirstButton;

        private void Start()
        {
            eventSystem.firstSelectedGameObject = pauseMenuFirstButton;
        }

        public void NavigateBetweenPauseButtons()
        {
            eventSystem.firstSelectedGameObject = pauseMenuFirstButton;
        }

        public void NavigateBetweenSettingsButtons()
        {
            eventSystem.firstSelectedGameObject = configMenuFirstButton;
        }
    }
}