using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class CameraHandler : MonoBehaviour
{
    public GameObject cameraPopupUI;
    public RawImage rawImage;

    private WebCamTexture webCam;

    private void Start()
    {
        webCam = new WebCamTexture();
        rawImage.texture = webCam;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") /*&& !other.GetComponent<PhotonView>().IsMine*/)
        {
            Debug.Log("Masuk area player lain: " + other.name);
            cameraPopupUI.SetActive(true);
            webCam.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") /*&& !other.GetComponent<PhotonView>().IsMine*/)
        {
            Debug.Log("Keluar area player lain: " + other.name);
            cameraPopupUI.SetActive(false);
            webCam.Stop();
        }
    }
}
