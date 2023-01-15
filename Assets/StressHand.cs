using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressHand : MonoBehaviour
{
    public GameObject ChangeTarget;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = ChangeTarget.GetComponent<Renderer>();
    }

    public void ChangeObjectColor()
    {
        rend.material.SetColor("_Color", Color.blue);
    }
}