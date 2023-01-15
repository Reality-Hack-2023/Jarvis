using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    IEnumerator YieldData()
    {
        for (;;) {
            for (int i = 1; i < 16; i+=2) {
                AudioSource source1 = GameObject.Find(i.ToString()).GetComponent<AudioSource>();
                source1.Play();
                AudioSource source2 = GameObject.Find((i+1).ToString()).GetComponent<AudioSource>();
                source2.Play();
                Debug.Log("Playing " + i.ToString());
                Debug.Log("Playing " + (i + 1).ToString());
                yield return new WaitForSeconds(3.0f);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(YieldData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
