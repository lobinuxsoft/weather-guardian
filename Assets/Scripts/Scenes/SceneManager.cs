using UnityEngine;

namespace WeatherGuardian 
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private string gameplaySceneName;

        [SerializeField] private string settingsSceneName;

        [SerializeField] private string creditsSceneName;

        [SerializeField] private string mainMenuSceneName;

        public void ChangeSceneToGameplay() 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameplaySceneName);
        }

        public void ChangeSceneToSettings()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(settingsSceneName);
        }

        public void ChangeSceneToCredits()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(creditsSceneName);
        }

        public void ChangeSceneToMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
        }

        public void ExitGame() 
        {
            Application.Quit();
        }
    }
}