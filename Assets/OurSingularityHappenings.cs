using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sngty;

public class OurSingularityHappenings : MonoBehaviour
{
    public SingularityManager mySingularityManager;
    public string CurrentMsg;
    public bool MsgReceived = false;
    public void OnConnected() {
        Debug.Log("Connected to device!");
    }

    public void OnMessageRecieved(string message) {
        CurrentMsg = message;
        MsgReceived = true;
    }

    public void OnError(string errorMessage) {
        Debug.LogErrorFormat("Error with Singularity: {0}", errorMessage);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("JARVIS");

        List<DeviceSignature> pairedDevices = mySingularityManager.GetPairedDevices();
        DeviceSignature myDevice = default;

        foreach (DeviceSignature ds in pairedDevices) {
            if (ds.name == "jARvis") {
                Debug.LogFormat("found jarvis! it's {0}", ds.ToString());
                myDevice = ds;
                break;
            } else
            {
                Debug.LogWarningFormat("ignoring device {0}", ds.ToString());
            }
        }

        if (!myDevice.Equals(default(DeviceSignature))) {
            Debug.Log("Connected!");
            mySingularityManager.ConnectToDevice(myDevice);
        } else {
            throw new System.Exception("CANNOT FIND DEVICE !!! :(");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() {
        mySingularityManager.DisconnectAll();
    }
}