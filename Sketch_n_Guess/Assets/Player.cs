using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //public Button[] levelButtons;
    public int numberOfCompletedLevels = 1;

    // This array keeps track of the completion status of each level
    //private bool[] levelCompletionStatus;

    public void SavePlayer() {
        SaveLoadFunctions.SavePlayerStats(this);
    }

    public void  LoadPlayer() {
        PlayerData data = SaveLoadFunctions.LoadPlayerStats();

        numberOfCompletedLevels = data.levelCompletionNumber;
    }
}
