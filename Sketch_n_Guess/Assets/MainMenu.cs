using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SinglePlayer() {
       // Scenemanager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MultiPlayer() {
       // Scenemanager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitApp() {
        Debug.Log("Quit!\n");
        Application.Quit();
    }
}
