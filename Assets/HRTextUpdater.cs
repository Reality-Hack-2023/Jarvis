using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class HRTextUpdater : MonoBehaviour
{

    public TMP_Text labelElement;
    string textTemplate;
    public OurSingularityHappenings sensor_data;
    float hr, hrv, stress_level;

    // Start is called before the first frame update
    void Start()
    {
        textTemplate = labelElement.text;
    }

    private void Update()
    {
        string message = sensor_data.CurrentMsg;
        if (sensor_data.MsgReceived)
        {
            sensor_data.MsgReceived = false;
            Debug.LogFormat("Message recieved from device: {1} bytes: {0}", message.Length, message);
            if (message.StartsWith("DATA:"))
            {
                var tabloc = message.IndexOf('\t');
                var payload = message[tabloc..];
                var tag = message[0..tabloc];
                float payload_value = float.Parse(payload);

                switch (tag)
                {
                    case "DATA:HR:":
                        hr = payload_value;
                        break;
                    case "DATA:HRV:":
                        hrv = payload_value;
                        break;
                    case "DATA:SL:":
                        stress_level = payload_value;
                        break;
                    default:
                        Debug.LogErrorFormat("Argh! {0} is absolutely not one of our payload types!", tag);
                        return;
                }
                UpdateTemplate();
            }
        }

    }
    private void UpdateTemplate()
    {
        labelElement.text = textTemplate.Replace("{hr}", hr.ToString()).Replace("{hrv}", hrv.ToString()).Replace("{sl}", stress_level.ToString());
    }
}
