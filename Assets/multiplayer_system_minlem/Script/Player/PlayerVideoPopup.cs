using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using TMPro;

public class PlayerVideoPopup : MonoBehaviourPun
{
    public GameObject popupUI;  // Canvas yang muncul di atas player
    public TMP_Text nameText;
    //public TMP_Text statusText;

    public float detectionRadius = 3f; // jarak deteksi
    private Transform localPlayer;

    void Start()
    {
        popupUI.SetActive(false); // awalnya popup mati
        //nameText.text = photonView.Owner.NickName;
        //statusText.text = "Idle"; // bisa diubah ke status lain (ex: mic on/off)

        if (photonView.IsMine)
        {
            localPlayer = this.transform;
        }
    }

    void Update()
    {
        if (photonView.IsMine) return; // Jangan cek ke diri sendiri

        if (localPlayer == null)
            localPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = Vector3.Distance(transform.position, localPlayer.position);
        if (distance <= detectionRadius)
        {
            popupUI.SetActive(true);
        }
        else
        {
            popupUI.SetActive(false);
        }
    }
}

