using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControls : MonoBehaviour
{
    // Enables the button for the next level
    public Button[] enableButtonLVL;
    public LevelButtonManager buttonManager;

    public void LevelCompleted(int buttonIndex) {
        Button[] buttonsToEnable = new Button[] { enableButtonLVL[buttonIndex] };
        buttonManager.EnableNextLevelBT(buttonsToEnable);
    }
}
