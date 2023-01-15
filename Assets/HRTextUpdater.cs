using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HRTextUpdater : MonoBehaviour
{

    public TextMeshProUGUI labelElement;
    string textTemplate;
    public OurSingularityHappenings sensor_data;

    // Start is called before the first frame update
    void Start()
    {
        textTemplate = labelElement.text;
        sensor_data.hr.AddListener(hr =>
        {
            UpdateTemplate("{hr}", hr);
        });
        sensor_data.hrv.AddListener(hrv =>
        {
            UpdateTemplate("{hrv}", hrv);
        });
        sensor_data.stress_level.AddListener(sl =>
        {
            UpdateTemplate("{sl}", sl);
        });
    }

    private void UpdateTemplate(string metavariable, float value)
    {
        labelElement.text = textTemplate.Replace(metavariable, value.ToString());
    }
}
