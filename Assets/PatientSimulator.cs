using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class PatientSimulator : MonoBehaviour
{

    public float hr, hrv;
    public float stress_level;
    public int math_stress_level;
    public bool updated_data;
    public bool enable_sim;

    private float _mean;
    private float _stdDev;
    private int _n;

    public int AnalyzeHRV(float hrv)
    {
        _n++;
        float delta = hrv - _mean;
        _mean += delta / _n;
        _stdDev += delta * (hrv - _mean);

        if (_n < 2)
        {
            return 2;
        }
        _stdDev = Mathf.Sqrt(_stdDev / (_n - 1));

        if (hrv < _mean - 3 * _stdDev)
        {
            return 0;
        }
        else if (hrv < _mean - 2 * _stdDev)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello!");
        StartCoroutine(YieldData());
    }

    IEnumerator YieldData()
    {

        var hrv_ranges = new List<float> {/* 0 */ 10.0f, 15.0f,    /* 1 */ 30.0f, 40.0f,  /* 2 */ 60.0f, 80.0f,  /* 3 */ 100.0f, 150.0f, /* 4 */ 20.0f, 400.0f};
        var hr_ranges =  new List<float>  {/* 0 */ 120.0f, 130.0f, /* 1 */ 90.0f, 110.0f, /* 2 */ 85.0f, 110.0f, /* 3 */ 60.0f, 85.0f, /* 4 */ 20.0f, 100.0f};

        for (;;) {
            hr = Random.Range(hr_ranges[(int)stress_level*2], hr_ranges[(int)stress_level*2+1]);
            hrv = Random.Range(hrv_ranges[(int)stress_level*2], hrv_ranges[(int)stress_level*2+1]);
            math_stress_level = AnalyzeHRV(hrv);
            
            if (enable_sim) updated_data = true;

            yield return new WaitForSeconds(5.0f);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}