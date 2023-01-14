using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sngty;

public class OurSingularityHappenings : MonoBehaviour
{
    public SingularityManager mySingularityManager;
    public UnityEvent<float> hr;
    public UnityEvent<float> hrv;
    public UnityEvent<float> stress_level;

    public void OnConnected() {
        Debug.Log("Connected to device!");
    }

    public void OnMessageRecieved(string message) {
        Debug.Log("Message recieved from device: " + message);
        if (message.StartsWith("DATA:"))
        {
            var tabloc = message.IndexOf('\t');
            var payload = message[tabloc..];
            var tag = message[0..tabloc];
            UnityEvent<float> evtgt = null;
            switch (tag)
            {
                case "DATA:HR:":
                    evtgt = hr;
                    break;
                case "DATA:HRV:":
                    evtgt = hrv;
                    break;
                case "DATA:SL:":
                    evtgt = stress_level;
                    break;
                default:
                    Debug.LogErrorFormat("Argh! %s is absolutely not one of our payload types!", tag);
                    return;
            }
            float payload_value = float.Parse(payload);
            evtgt.Invoke(payload_value);
        }
    }

    public void OnError(string errorMessage) {
        Debug.LogErrorFormat("Error with Singularity: %s", errorMessage);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("JARVIS");

        List<DeviceSignature> pairedDevices = mySingularityManager.GetPairedDevices();
        DeviceSignature myDevice = default;

        foreach (DeviceSignature ds in pairedDevices) {
            if (ds.name == "jARvis") {
                Debug.LogFormat("found jarvis! it's %s", ds.ToString());
                myDevice = ds;
                break;
            } else
            {
                Debug.LogWarningFormat("ignoring device %s", ds.ToString());
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