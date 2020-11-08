using UnityEngine;
using System;
public class moving : MonoBehaviour {
    Vector3 startpos;
    void Start() {
        startpos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.3f);
    }
    int counter ;
    void Update()
    {
        transform.localPosition = Vector3.Lerp(new Vector3(startpos.x, startpos.y, startpos.z + 0.6f), startpos,Mathf.PingPong(Time.time, 1)); 
    }
    
}