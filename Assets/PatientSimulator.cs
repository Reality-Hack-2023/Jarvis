using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class PatientSimulator : MonoBehaviour
{

    public float hr, hrv;
    public float stress_level;
    public bool updated_data;
    public bool enable_sim;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello!");
        StartCoroutine(YieldData());
    }

    IEnumerator YieldData()
    {

        var hrv_ranges = new List<float> {/* 0 */ 10.0f, 15.0f,    /* 1 */ 30.0f, 40.0f,  /* 2 */ 60.0f, 80.0f,  /* 3 */ 100.0f, 150.0f};
        var hr_ranges =  new List<float>  {/* 0 */ 120.0f, 130.0f, /* 1 */ 90.0f, 110.0f, /* 2 */ 85.0f, 110.0f, /* 3 */ 60.0f, 85.0f};

        for (;;) {
            hr = Random.Range(hr_ranges[(int)stress_level*2], hr_ranges[(int)stress_level*2+1]);
            hrv = Random.Range(hrv_ranges[(int)stress_level*2], hrv_ranges[(int)stress_level*2+1]);

            if (enable_sim) updated_data = true;

            yield return new WaitForSeconds(5.0f);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}