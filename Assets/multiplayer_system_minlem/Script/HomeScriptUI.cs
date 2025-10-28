using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Random = UnityEngine.Random;

public class HomeScriptUI : MonoBehaviourPunCallbacks
{
    [Header("Home UI")]
    public GameObject homeUi;
    public GameObject createSpaceUI;
    public Button createSpaceButton;

    [Header("Create Space UI")]
    public InputField spaceNameInput;
    public Button backToHomeUi;
    public Button createRoomUi;
    public Button joinRoomUi;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        createSpaceUI.SetActive(false);

        // Generate random nickname jika belum ada
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
        {
            string randomName = $"Player{Random.Range(1000, 9999)}";
            PlayerPrefs.SetString("PlayerName", randomName);
        }

        // Set Photon nickname
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");

        // Connect ke Photon server jika belum terhubung
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    public void OnCreateSpaceClick()
    {
        homeUi.SetActive(false);
        createSpaceUI.SetActive(true);
    }

    public void OnBackClick()
    {
        createSpaceUI.SetActive(false);
        homeUi.SetActive(true);
    }

    public void OnCreateRoomClick()
    {
        if (string.IsNullOrWhiteSpace(spaceNameInput.text)) return;

        RoomOptions options = new RoomOptions { MaxPlayers = 10 };
        PhotonNetwork.CreateRoom(spaceNameInput.text.Trim(), options);
    }

    public void OnJoinRoomClick()
    {
        if (string.IsNullOrWhiteSpace(spaceNameInput.text)) return;

        PhotonNetwork.JoinRoom(spaceNameInput.text.Trim());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Berhasil masuk room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("RoomScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Gagal buat room: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Gagal join room: " + message);
    }
}
