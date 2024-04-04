using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public int numberOfCompletedLevels = -1;

    // This array keeps track of the completion status of each level
    // There are 4 levels
    public bool[] levelCompletionStatus = new bool[4];

    public void SavePlayer() {
        SaveLoadFunctions.SavePlayerStats(this);
    }

    public void LoadPlayer() {
        PlayerData data = SaveLoadFunctions.LoadPlayerStats();

        numberOfCompletedLevels = data.levelCompletionNumber;
        for(int i = 0; i < data.levelStatus.Length; i++) {
            levelCompletionStatus[i] = data.levelStatus[i];
        }
    }
}
