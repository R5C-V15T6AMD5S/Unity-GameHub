using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItems : MonoBehaviour
{
    public Text roomName;
    LobbyManager lobbyManager;

    private void Start() {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName) {
        roomName.text = _roomName;
    }

    public void OnClickItem() {
        lobbyManager.JoinRoomName(roomName.text);
    }
}
