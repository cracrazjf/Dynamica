using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanNervousSystem {
    float[ , , ] visualInput = new float[32, 32, 3];
    float[] touchInput = new float[5]; // standing, sitting, laying, holding_LH, holding_RH
    public List<string> touchInputLabelList = new List<string>{"standing up", "sitting down", "laying down", "holding LH", "holding RH"};
    
    public Dictionary<string, int> touchInputLabelIndexDict = new Dictionary<string, int>();

    public Human thisHuman;
    public HumanNervousSystem(Human human) {
        this.thisHuman = human;

        for (int i=0; i<32; i++){
            for (int j=0; j<32; j++){
                for (int k=0; k<3; k++){
                    visualInput[i,j,k] = 0;
                }
            }
        }

        for (int i=0; i<5; i++){
            touchInput[i] = 0;
            touchInputLabelIndexDict.Add(touchInputLabelList[i], i);
        }
        touchInput[0] = 1; // this assumes that the human starts standing
    }

    public void updateTouchInput(int touchIndex, float value){
        touchInput[touchIndex] = value;
    }

    public float[] GetTouchInput(){
        return touchInput;
    }

    public float[ , , ] GetVisualInput ()
    {
        // we need to get the human's current camera image as a 3-d bitmap matrix of rgb-values
        // for example, if the camera resolution is 256x256, then the matrix would be 256x256x3, where those 
        // three are r,g,b
        // then we look in our preferences for what the nervous system resolution is, and we downsample
        // 32x32x3, 256/x = 32, x = 8, basically an algorithm that 
    
        return visualInput;
    }
}