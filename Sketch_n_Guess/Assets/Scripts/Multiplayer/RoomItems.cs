using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItems : MonoBehaviour
{
    public Text roomName;
    
    public void SetRoomName(string _roomName) {
        roomName.text = _roomName;
    }
}
