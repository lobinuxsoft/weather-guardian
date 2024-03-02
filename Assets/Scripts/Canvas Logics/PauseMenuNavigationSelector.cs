using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WeatherGuardian.CanvasManager 
{
    public class PauseMenuNavigationSelector : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;

        [SerializeField] private GameObject configMenuFirstButton;

        public void NavigateBetweenPauseButtons()
        {
            //pauseMenuFirstButton.Select();
            //eventSystem.SetSelectedGameObject(pauseMenuFirstButton);
        }

        public void NavigateBetweenSettingsButtons()
        {
            eventSystem.SetSelectedGameObject(configMenuFirstButton);
        }
    }
}