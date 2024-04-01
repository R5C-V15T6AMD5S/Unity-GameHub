using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    public Button[] lvlButtons;

    public void EnableNextLevelBT(string lvlBTname, Color color) {
        foreach(Button button in lvlButtons) {
            if(button.name == lvlBTname) {
                button.interactable = true;
                button.GetComponent<Image>().color = color;
                break;
            }
        }
    }
}
