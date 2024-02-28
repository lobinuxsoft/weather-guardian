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
            eventSystem.SetSelectedGameObject(pauseMenuFirstButton);
        }

        public void NavigateBetweenPauseButtons()
        {
            eventSystem.SetSelectedGameObject(pauseMenuFirstButton);
        }

        public void NavigateBetweenSettingsButtons()
        {
            eventSystem.SetSelectedGameObject(configMenuFirstButton);
        }
    }
}