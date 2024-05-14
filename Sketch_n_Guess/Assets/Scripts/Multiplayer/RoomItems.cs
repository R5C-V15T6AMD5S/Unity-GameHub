using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItems : MonoBehaviour
{
    public Text roomName;
    LobbyManager lobbyManager;

    private void Start() {
        // Finds GameObject with that name
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    // Sets the label text to roomName
    public void SetRoomName(string _roomName) {
        roomName.text = _roomName;
    }

    // When clicked, joins the room
    public void OnClickItem() {
        lobbyManager.JoinRoomName(roomName.text);
    }
}
