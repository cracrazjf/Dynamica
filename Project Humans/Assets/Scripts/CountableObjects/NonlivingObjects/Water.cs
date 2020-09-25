using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Water : MonoBehaviour
{
    System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer> ().material.color = Color.cyan;
        gameObject.transform.localScale = new Vector3(.2f, .2f, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
