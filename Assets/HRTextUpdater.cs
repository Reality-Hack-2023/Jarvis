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
    public PatientSimulator patientSimulator;

    // Start is called before the first frame update
    void Start()
    {
        textTemplate = labelElement.text;
    }

    private void Update()
    {
        float hr = -1.0f,  hrv = -1.0f,  stress_level = -1.0f;
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
                UpdateTemplate(hr, hrv, stress_level);
            }
        }
        else if (patientSimulator.updated_data) {
            UpdateTemplate(patientSimulator.hr, patientSimulator.hrv, patientSimulator.stress_level);    
        }

    }

    public void UpdateTemplate(float hr, float hrv, float stress_level)
    {
        int hr_int = (int)hr;
        int stress_level_int = (int)stress_level;
        labelElement.text = textTemplate.Replace("{hr}", hr_int.ToString()).Replace("{hrv}", hrv.ToString()).Replace("{sl}", stress_level_int.ToString());
    }
}
