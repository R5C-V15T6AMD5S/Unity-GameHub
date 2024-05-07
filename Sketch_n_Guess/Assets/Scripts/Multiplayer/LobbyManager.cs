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

    public List<PlayerItems> playerItemsList = new List<PlayerItems>();
    public PlayerItems playerItemsPrefab;
    public Transform playerItemsParent;

    public GameObject playButton;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    private void Start() {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate() {
        if(roomIF.text.Length >= 1) {
            //PhotonNetwork.CreateRoom(roomIF.text);
            PhotonNetwork.CreateRoom(roomIF.text, new RoomOptions(){ MaxPlayers = 4});
        }
    }

    public override void OnJoinedRoom() {
        lobbyPannel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        if(Time.time >= nextUpdateTime) {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
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

    public void JoinRoomName(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        roomPanel.SetActive(false);
        lobbyPannel.SetActive(true);
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList() {
        foreach(PlayerItems item in playerItemsList) {
            Destroy(item.gameObject);
        } playerItemsList.Clear();

        if(PhotonNetwork.CurrentRoom == null) { return; }

        // Players didnt work, dunno why -- That why I had to use Photon.Realtime.Players
        foreach(KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players) {
            PlayerItems newPlayerItem = Instantiate(playerItemsPrefab, playerItemsParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
        UpdatePlayerList();
    }

    private void Update() {
        if(PhotonNetwork.IsMasterClient) {
            playButton.SetActive(true);
        } else {
            playButton.SetActive(false);
        }
    }

    public void OnClickPlayButton() {
        PhotonNetwork.LoadLevel("Multiplayer Gameplay");
    }
}
