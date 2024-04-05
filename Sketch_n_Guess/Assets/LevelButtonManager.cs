using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    public Color orangeColor = new Color(1f, 0.45f, 0.125f); // Color #FF7420

    public void EnableNextLevelBT(Button[] enableButtonLVL) {
        foreach(Button button in enableButtonLVL) {
            button.interactable = true;
            
            // Change the color properties
            ColorBlock colors = button.colors;
            colors.normalColor = orangeColor;
            colors.pressedColor = orangeColor;
            colors.selectedColor = orangeColor;
            colors.highlightedColor = Color.white;
            button.colors = colors;
        }
    }
}
