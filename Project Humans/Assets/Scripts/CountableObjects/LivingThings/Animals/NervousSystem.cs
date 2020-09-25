using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NervousSystem : MonoBehaviour {
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    public void GetInput ()
    {
        // we need to get the human's current camera image as a 3-d bitmap matrix of rgb-values
        // for example, if the camera resolution is 256x256, then the matrix would be 256x256x3, where those 
        // three are r,g,b
        // then we look in our preferences for what the nervous system resolution is, and we downsample
        // 32x32x3, 256/x = 32, x = 8, basically an algorithm that 


    }
}