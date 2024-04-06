using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public void PlaySingleplayerMode() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayMultiplayerMode() {
        //SceneManage r.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }  

    public void QuitGame() {
        Application.Quit();
        Debug.Log("You have exited the game!");
    }
}   
