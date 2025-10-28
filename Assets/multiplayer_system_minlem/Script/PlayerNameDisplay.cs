using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameDisplay : MonoBehaviourPun
{
    public TextMeshPro nameText;
    void Start()
    {
        if (photonView.IsMine)
        {
            // Biar ga dobel nama di atas kepala kita sendiri (optional)
            nameText.text = PhotonNetwork.NickName;
        }
        else
        {
            nameText.text = photonView.Owner.NickName;
        }
    }
}
