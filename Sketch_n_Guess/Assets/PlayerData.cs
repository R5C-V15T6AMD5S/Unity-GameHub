using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public int levelCompletionNumber = 0;
    public bool[] levelStatus = new bool[4];

    public PlayerData (Player player) {
        levelCompletionNumber = player.numberOfCompletedLevels;

        for (int i = 0; i < player.levelCompletionStatus.Length; i++) {
            levelStatus[i] = player.levelCompletionStatus[i];
        }
    }
}
