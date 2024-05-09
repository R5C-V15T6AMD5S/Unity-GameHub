using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessages : MonoBehaviour
{
    [SerializeField] public Text messageText;

    public void SetText(string msgTxt) {
        messageText.text = msgTxt;
    }
}
