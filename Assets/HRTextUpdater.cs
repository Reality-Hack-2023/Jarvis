using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HRTextUpdater : MonoBehaviour
{

    public TextMeshProUGUI hrtext;
    public int currentHr = 120;
    public string textTemplate;
    // Start is called before the first frame update
    void Start()
    {
        textTemplate = hrtext.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseHR(int number_bpm)
    {
        currentHr += number_bpm;
        hrtext.text = textTemplate.Replace("{hr}", currentHr.ToString());
    }
}
