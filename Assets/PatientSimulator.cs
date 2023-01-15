using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class PatientSimulator : MonoBehaviour
{
    HRTextUpdater hrTextUpdater;

    float hr, hrv, stress_level;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(YieldData());
    }

    IEnumerator YieldData()
    {

        // randomize? recv events from buttons to change fake stress level?
        var stress_level = 2;

        var hrv_ranges = new List<float> {/* 0 */ 10, 15,    /* 1 */ 30, 40,  /* 2 */ 60, 80,  /* 3 */ 100, 150};
        var hr_ranges =  new List<float>  {/* 0 */ 120, 130, /* 1 */ 90, 110, /* 2 */ 85, 110, /* 3 */ 60, 85};

        for (;;) {
            hr = Random.Range(hr_ranges[stress_level*2], hr_ranges[stress_level*2+1]);
            hrv = Random.Range(hrv_ranges[stress_level*2], hrv_ranges[stress_level*2+1]);

            hrTextUpdater.UpdateTemplate(hr, hrv, stress_level);

            yield return new WaitForSeconds(5.0f);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}