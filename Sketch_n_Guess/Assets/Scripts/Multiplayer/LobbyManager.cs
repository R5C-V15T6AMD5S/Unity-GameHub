using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomIF;
    public GameObject lobbyPannel;
    public GameObject roomPanel;
    public Text roomName;

    public RoomItems roomItemsPrefab;
    List<RoomItems> roomItemsList = new List<RoomItems>();
    public Transform contentObject;

    private void Start() {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate() {
        if(roomIF.text.Length >= 1) {
            PhotonNetwork.CreateRoom(roomIF.text, new RoomOptions(){ MaxPlayers = 3});
        }
    }

    public override void OnJoinedRoom() {
        lobbyPannel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        UpdateRoomList(roomList);
    }

    void UpdateRoomList(List<RoomInfo> list) {
        foreach(RoomItems item in roomItemsList) {
            Destroy(item.gameObject);
        } roomItemsList.Clear();

        foreach(RoomInfo room in list) {
            RoomItems newRoom = Instantiate(roomItemsPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }
}
