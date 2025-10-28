using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class WebcamManager : MonoBehaviourPun
{
    public RawImage webcamDisplay;
    private WebCamTexture webcamTexture;

    void Start()
    {
        if (photonView.IsMine) // Hanya kamera kita sendiri
        {
            webcamTexture = new WebCamTexture();
            webcamDisplay.texture = webcamTexture;
            webcamTexture.Play();
        }
    }
}