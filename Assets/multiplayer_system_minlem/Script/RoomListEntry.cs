using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListEntry : MonoBehaviour
{
    public TMP_Text roomNameText;
    public TMP_Text playerCountText;
    public Button joinButton;

    private string roomName;

    public void Initialize(RoomInfo info)
    {
        roomName = info.Name;
        roomNameText.text = info.Name;
        playerCountText.text = $"{info.PlayerCount} / {info.MaxPlayers}";

        joinButton.onClick.AddListener(() => {
            PhotonNetwork.JoinRoom(roomName);
        });
    }
}
