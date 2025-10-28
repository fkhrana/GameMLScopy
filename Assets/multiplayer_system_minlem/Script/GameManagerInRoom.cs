using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManagerInRoom : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject[] playerPrefabs; // Multiple prefabs for random selection

    [Header("Spawn Settings")]
    public Transform spawnPoint;

    void Start()
    {
        Debug.LogWarning("=====> Start() dipanggil di GameManagerInRoom");

        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            if (playerPrefabs == null || playerPrefabs.Length == 0)
            {
                Debug.LogError("❌ playerPrefabs belum di-assign di inspector!");
                return;
            }

            int randomIndex = Random.Range(0, playerPrefabs.Length);
            GameObject selectedPrefab = playerPrefabs[randomIndex];
            Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;

            GameObject go = PhotonNetwork.Instantiate(selectedPrefab.name, pos, Quaternion.identity);
            if (go == null)
            {
                Debug.LogError("❌ PhotonNetwork.Instantiate gagal — prefab mungkin tidak ditemukan!");
                return;
            }

            Debug.Log("<color=green>[Photon]</color> Spawned player: " + go.name);

            var rend = go.GetComponentInChildren<SpriteRenderer>();
            if (rend != null)
                Debug.Log("<color=yellow>✅ SpriteRenderer ditemukan: " + rend.sprite.name + "</color>");
            else
                Debug.LogWarning("⚠️ SpriteRenderer tidak ditemukan di Player prefab");

            Debug.Log("📍 Player position: " + go.transform.position);
            Debug.Log("🎥 Camera position: " + Camera.main.transform.position);
        }
    }
}
