using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControls : MonoBehaviour {
    public Player playerData;
    public Button[] enableButtonLVL;
    public LevelButtonManager buttonManager;

    // Saves how many levels have been completed
    private bool[] levelCompleted = new bool[4];

    public void LevelCompleted(int buttonIndex) {

        SceneManager.LoadScene("Singleplayer");

        if (!levelCompleted[buttonIndex]) {
            playerData.numberOfCompletedLevels++;
            levelCompleted[buttonIndex] = true;
            // Debug.Log("Levels Completed: " + playerData.numberOfCompletedLevels);

            Button[] buttonsToEnable = new Button[] { enableButtonLVL[buttonIndex] };
            buttonManager.EnableNextLevelBT(buttonsToEnable);
        }
    }
}
