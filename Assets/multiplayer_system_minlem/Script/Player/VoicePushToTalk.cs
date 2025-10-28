using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;

public class VoicePushToTalk : MonoBehaviour
{
    public Recorder recorder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = Input.GetKey(KeyCode.V);
        }
    }
}
