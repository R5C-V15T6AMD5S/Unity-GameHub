using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.45f;

    public void LoadNextLevel(string loadScene) {
        //StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        StartCoroutine(LoadLevel(loadScene));
    }

    IEnumerator LoadLevel(string sceneName) {
        // Play animation
        transition.SetTrigger("StartTrans");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        if(sceneName == "Quit") {
            Application.Quit();
            Debug.Log("You have exited the game!");
        } else { 
            SceneManager.LoadScene(sceneName);
        } 
    }
}
