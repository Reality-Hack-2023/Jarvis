using System.Collections;
using System.Collections.Generic;
using System.Random;

using UnityEngine;
using UnityEngine.Events;

public class PatientSimulator : MonoBehaviour
{
    HRTextUpdater hrTextUpdater;

    public UnityEvent<float> hr;
    public UnityEvent<float> hrv;
    public UnityEvent<float> stress_level;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(YieldData());
    }

    IEnumerator YieldData()
    {
        Random rng = new Random();

        // randomize? recv events from buttons to change fake stress level?
        var stress_level = 2;

        var hrv_ranges = new List<double> {/* 0 */ 10, 15,    /* 1 */ 30, 40,  /* 2 */ 60, 80,  /* 3 */ 100, 150};
        var hr_ranges =  new List<double>  {/* 0 */ 120, 130, /* 1 */ 90, 110, /* 2 */ 85, 110, /* 3 */ 60, 85};

        for (;;) {
            double hr = rng.Next(hr_ranges[stress_level*2], hr_ranges[stress_level*2+1]));
            double hrv = rng.Next(hrv_ranges[stress_level*2], hrv_ranges[stress_level*2+1]));
            double stress_level = stress_level;

            hrTextUpdater.UpdateTemplate(hr, hrv, stress_level);

            yield return new WaitForSeconds(5.f);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}