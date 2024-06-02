using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            Debug.Log("PLAY!");
            SceneManager.LoadScene("Game");
        }
    
        public void GoToSettings()
        {
            Debug.Log("SETTINGS!");
            SceneManager.LoadScene("Settings");
        }

        public void QuitGame()
        {
            Debug.Log("QUIT!");
            Application.Quit();
        }
    }
}
