using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public GameObject mainUI;
    public TMP_InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomNameText;
    public GameObject uiProfileEdit;
    public GameObject panelEditorChara;
    public Animator animator;

    [Header("Tab Character Editor")]
    public GameObject basePanel;
    public GameObject accessoriesPanel;
    public GameObject clothingPanel;

    public GameObject btnBase;
    public GameObject btnAccessories;
    public GameObject btnClothing;
    public GameObject imageBase;
    public GameObject imageAccessories;
    public GameObject imageClothing;

    [Header("Room List")]
    public Transform roomListContainer;         // Parent untuk daftar room
    public GameObject roomListItemPrefab;       // Prefab item daftar room

    [Header("Komponen")]
    public TMP_InputField nicknameInput;

    private Dictionary<string, RoomInfo> roomListEntries = new Dictionary<string, RoomInfo>();
    private bool isUIShowing = false;
    private bool isAnimating = false;


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        lobbyPanel.SetActive(false);
        uiProfileEdit.SetActive(false);
        panelEditorChara.SetActive(false);
        accessoriesPanel.SetActive(false);
        clothingPanel.SetActive(false);
        imageBase.SetActive(true);
        imageAccessories.SetActive(false);
        imageClothing.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    // --- ROOM LIST UPDATE ---
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate dipanggil. Jumlah room: " + roomList.Count);

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                roomListEntries.Remove(room.Name);
            }
            else
            {
                roomListEntries[room.Name] = room;
            }
        }
        UpdateRoomListUI();
    }

    void UpdateRoomListUI()
    {
        Debug.Log("UpdateRoomListUI dipanggil. Jumlah room untuk UI: " + roomListEntries.Count);

        foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in roomListEntries.Values)
        {
            GameObject newItem = Instantiate(roomListItemPrefab, roomListContainer);

            newItem.GetComponentInChildren<TMP_Text>().text = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";

            Button joinButton = newItem.GetComponentInChildren<Button>();

            joinButton.onClick.AddListener(() => {
                PhotonNetwork.JoinRoom(room.Name);
            });
        }
    }

    public void onBackClick()
    {
        lobbyPanel.SetActive(false);
        mainUI.SetActive(true);
    }
    public void onClickCreateSpace()
    {
        mainUI.SetActive(false);
        lobbyPanel.SetActive(true);
    }
    public void onClickCloseButtonEditChara()
    {
        panelEditorChara.SetActive(false);
    }

    public void onClickProfileUI()
    {
        if (isAnimating) return;

        if (!isUIShowing)
        {
            uiProfileEdit.SetActive(true);
            animator.SetBool("uiOpen", true);
            animator.SetBool("uiClose", false);
            isUIShowing = true;
        }
        else
        {
            StartCoroutine(CloseUIWithDelay());
        }
    }

    private IEnumerator CloseUIWithDelay()
    {
        isAnimating = true;

        animator.SetBool("uiClose", true);
        animator.SetBool("uiOpen", false);

        yield return new WaitForSeconds(1.8f);

        uiProfileEdit.SetActive(false);
        isUIShowing = false;
        isAnimating = false;

        animator.SetBool("uiClose", false);
    }
    public void ToggleCharacterEditor()
    {
        panelEditorChara.SetActive(!panelEditorChara.activeSelf);
    }

    public void CreateRoom()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Belum siap buat room: masih connecting...");
            return;
        }

        if (!string.IsNullOrEmpty(roomInputField.text))
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10;
            PhotonNetwork.CreateRoom(roomInputField.text, roomOptions);
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        if (!string.IsNullOrEmpty(roomInputField.text))
        {
            PhotonNetwork.JoinRoom(roomInputField.text);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        PhotonNetwork.LoadLevel("RoomScene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    #region ClothCustomize
    public void OnClickBase()
    {
        basePanel.SetActive(true);
        accessoriesPanel.SetActive(false);
        clothingPanel.SetActive(false);

        imageBase.SetActive(true);
        imageAccessories.SetActive(false);
        imageClothing.SetActive(false);
    }

    public void OnClickAccessories()
    {
        basePanel.SetActive(false);
        accessoriesPanel.SetActive(true);
        clothingPanel.SetActive(false);

        imageBase.SetActive(false);
        imageAccessories.SetActive(true);
        imageClothing.SetActive(false);
    }

    public void OnClickClothing()
    {
        basePanel.SetActive(false);
        accessoriesPanel.SetActive(false);
        clothingPanel.SetActive(true);

        imageBase.SetActive(false);
        imageAccessories.SetActive(false);
        imageClothing.SetActive(true);
    }
    #endregion
}
