using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SinlePlayer() {
       // Scenemanager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit() {
        Debug.Log("Quit!\n");
        Application.Quit();
    }
}
