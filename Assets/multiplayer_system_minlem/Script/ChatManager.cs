using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun
{
    [Header("UI References")]
    public GameObject chatPanel;
    public TMP_InputField inputField;
    public Transform content;
    public GameObject messagePrefab;

    private bool chatVisible = true;

    void Start()
    {
        if (inputField != null)
            inputField.ActivateInputField();
    }

    void Update()
    {
        // Toggle panel chat dengan tombol C
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            chatVisible = !chatVisible;
            chatPanel.SetActive(chatVisible);

            // Kalau panel baru dinyalakan, langsung aktifkan input
            if (chatVisible && inputField != null)
                inputField.ActivateInputField();
        }

        // Kirim chat dengan Enter
        if (chatVisible && Input.GetKeyDown(KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(inputField.text))
            {
                SendChat(inputField.text);
                inputField.text = "";
                inputField.ActivateInputField();
            }
        }
    }

    public void SendChat(string message)
    {
        if (photonView != null)
        {
            photonView.RPC("ReceiveChat", RpcTarget.All, PhotonNetwork.NickName, message);
        }
    }

    [PunRPC]
    public void ReceiveChat(string sender, string message)
    {
        GameObject msgObj = Instantiate(messagePrefab, content);
        TMP_Text msgText = msgObj.GetComponent<TMP_Text>();
        if (msgText != null)
        {
            msgText.text = $"<b>{sender}:</b> {message}";
        }

        // === Bubble Chat ===
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.Owner.NickName == sender)
            {
                player.GetComponent<PlayerBubbleChat>()?.ShowBubble(message);
            }
        }
    }

}
