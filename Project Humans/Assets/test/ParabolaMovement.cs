using UnityEngine;
using System;
public class ParabolaMovement : MonoBehaviour {
    Vector3 startpos;
    void Start() {
        startpos = startpos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.3f);
    }
    void Update()
    {
        transform.localPosition = Vector3.Lerp(startpos, new Vector3(startpos.x, startpos.y, startpos.z + 0.6f), Mathf.PingPong(Time.time, 1));
        
    }
    
}