using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level.UI
{
    public class PauseMenu : MonoBehaviour
    {
        private static bool _gameIsPaused;

        public GameObject pauseMenuUI;
    
        public GameObject lvlUpMenuUI;
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (_gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    
        private void Resume()
        {
            _gameIsPaused = false;
            pauseMenuUI.SetActive(false);
            if (lvlUpMenuUI.activeSelf)
            {
                return;
            }
            Time.timeScale = 1f;
        
        }
    
        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _gameIsPaused = true;
        }
    
        public void GoToMainMenu()
        {
            Resume();
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            Debug.Log("QUIT!");
            Application.Quit();
        }
    }
}
