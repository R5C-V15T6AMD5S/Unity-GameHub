using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Public username that will be used
    public InputField usernameInput;
    public Text buttonText;

    public void OnClickConnect() {
        if(usernameInput.text.Length >= 1) {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true; // Used for sending players from the same room to Gameplay
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // When connected, transfer the juser to MP Lobby scene
    public override void OnConnectedToMaster() {
        SceneManager.LoadScene("Multiplayer Lobby");
    }
}
