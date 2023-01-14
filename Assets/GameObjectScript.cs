using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sngty;

public class GameObjectScript : MonoBehaviour
{
    public SingularityManager mySingularityManager;

    public void onConnected() {
        Debug.Log("Connected to device!");
    }

    public void onMessageRecieved(string message) {
        Debug.Log("Message recieved from device: " + message);
    }

    public void onError(string errorMessage) {
        Debug.LogError("Error with Singularity: " + errorMessage);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("JARVIS");

        List<DeviceSignature> pairedDevices = mySingularityManager.GetPairedDevices();
        DeviceSignature myDevice = default(DeviceSignature);

        for (int i = 0; i < pairedDevices.Count; i++) {
            if (pairedDevices[i].name == "jARvis") {
                myDevice = pairedDevices[i];
                break;
            }
        }

        if (!myDevice.Equals(default(DeviceSignature))) {
            mySingularityManager.ConnectToDevice(myDevice);
        } else {
            Debug.Log("CANNOT FIND DEVICE !!! :(");
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
