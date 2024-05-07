using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItems : MonoBehaviourPunCallbacks
{
    public Text playerName;

    public void SetPlayerInfo(Photon.Realtime.Player _player) {
        playerName.text = _player.NickName;
    }
}
