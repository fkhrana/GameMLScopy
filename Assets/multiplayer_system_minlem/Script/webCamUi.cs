using UnityEngine;
using UnityEngine.UI;

public class webCamUi : MonoBehaviour
{
    public RawImage rawImageDisplay;

    [Header("Buttons")]
    public Button camToggleButton;
    public Button micToggleButton;

    [Header("Visual UI Status")]
    public GameObject camOnIconUI;
    public GameObject camOffIconUI;
    public GameObject micOnIconUI;
    public GameObject micOffIconUI;

    private WebCamTexture webcamTexture;
    private bool isCamOn = false;
    private bool isMicOn = false;

    void Start()
    {
        webcamTexture = new WebCamTexture();
        rawImageDisplay.texture = webcamTexture;
        rawImageDisplay.material.mainTexture = webcamTexture;

        isCamOn = false;
        isMicOn = false;

        camToggleButton.onClick.AddListener(ToggleCam);
        micToggleButton.onClick.AddListener(ToggleMic);

        UpdateUI();
    }

    void ToggleCam()
    {
        isCamOn = !isCamOn;

        if (isCamOn)
            webcamTexture.Play();
        else
            webcamTexture.Stop();

        UpdateUI();
    }

    void ToggleMic()
    {
        isMicOn = !isMicOn;
        UpdateUI();
    }

    void UpdateUI()
    {
        camOnIconUI.SetActive(isCamOn);
        camOffIconUI.SetActive(!isCamOn);

        micOnIconUI.SetActive(isMicOn);
        micOffIconUI.SetActive(!isMicOn);
    }
}
