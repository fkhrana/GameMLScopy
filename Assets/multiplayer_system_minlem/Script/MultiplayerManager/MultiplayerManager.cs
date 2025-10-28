using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    void Start()
    {    
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon!");
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.Instantiate("PlayerPrefab", new Vector3(Random.Range(-2, 2), 0, 0), Quaternion.identity);
    }
}
