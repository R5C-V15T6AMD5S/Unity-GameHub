using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public int levelCompletionNumber;
    //public int[] levelStars;

    public PlayerData (Player player) {
        levelCompletionNumber = player.numberOfCompletedLevels;
        //levelStars = player.numberOfStarsPerLevel;
    }
}
