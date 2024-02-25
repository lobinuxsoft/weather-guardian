using UnityEngine;

namespace WeatherGuardian 
{
    public class SceneManager : MonoBehaviour
    {
        private const string gameplaySceneName = "Desert Flooded Level";

        private const string settingsSceneName = "Settings Scene";

        private const string creditsSceneName = "Credits Scene";
        
        private const string mainMenuSceneName = "Main Menu Scene";

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