using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    public InputField inputMessage;
    public GameObject UserMessages;
    public GameObject Content;

    private void Start() {
        // Add event listener for the end of edit (when user finishes editing)
        inputMessage.onEndEdit.AddListener(delegate { SendMessageOnEnter(); });
    }
    
    public void SendMessageOnEnter() {
        // Check if the input field is not empty and if the user pressed the "Return" key
        if(!string.IsNullOrEmpty(inputMessage.text) && Input.GetKeyDown(KeyCode.Return)) {
            SendMessage();
        }
    }
    
    public void SendMessage() {
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, (PhotonNetwork.NickName + ": " + inputMessage.text)); 

        inputMessage.text = "";
    }

    [PunRPC]
    public void GetMessage(string ReceiveMessage) {
        GameObject M = Instantiate(UserMessages, Vector3.zero, Quaternion.identity, Content.transform);
        M.GetComponent<UserMessages>().myMessage.text = ReceiveMessage;
    }
}
