using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public int numberOfCompletedLevels = 0;

    // Keeps track of the completion status of each of the 4 levels
    public bool[] levelCompletionStatus = new bool[4];

    // Button that saves the players data
    public void SavePlayer() {
        SaveLoadFunctions.SavePlayerStats(this);
    }

    // Button that loads the players data
    public void LoadPlayer() {
        PlayerData data = SaveLoadFunctions.LoadPlayerStats();

        // Enables the levels that have been completed
        numberOfCompletedLevels = data.levelCompletionNumber;
        for(int i = 0; i < data.levelStatus.Length; i++) {
            levelCompletionStatus[i] = data.levelStatus[i];
        }
    }
}
