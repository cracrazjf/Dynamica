using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanNervousSystem : NervousSystem {
    bool[] touchInput = new bool[5]; // standing, sitting, laying, holding_LH, holding_RH

    // todo add all the states and transitions to 
    public List<string> bodyStateLabelList = new List<string>{
                                                              "standing", 
                                                              "sitting", 
                                                              "laying",
                                                              "sleeping", 
                                                              "awake", 
                                                              "asleep",
                                                              "holding with left hand",
                                                              "holding with right hand", 

                                                              "sitting down", 
                                                              "sitting up", 
                                                              "laying down",
                                                              "standing up", 
                                                              "rotating", 
                                                              "taking steps",
                                                              "picking up with left hand", 
                                                              "picking up with right hand", 
                                                              "setting down with left hand",
                                                              "setting down with right hand",
                                                              "eating with left hand", 
                                                              "eating with right hand",
                                                              "drinking"};
    
    public Dictionary<string, int> bodyStateIndexDict = new Dictionary<string, int>(); // we don't need that 
    private float[] bodyStateArray = new float[21];

    public Human thisHuman;
    int visualResolution;
    float visualRefreshRate;

    float[ , ] visualInput;

    public HumanNervousSystem(Human human) : base(human) {
        this.thisHuman = human;
        visualResolution = int.Parse(thisHuman.phenotype.traitDict["visual_resolution"]);
        visualRefreshRate = float.Parse(thisHuman.phenotype.traitDict["visual_refresh_rate"]);
        visualInput = new float[3,visualResolution*visualResolution];

        for (int i=0; i<bodyStateLabelList.Count; i++){
            bodyStateArray[i] = 0.0f;
            bodyStateIndexDict.Add(bodyStateLabelList[i], i);
        }

        if (this.thisHuman.visualInputCamera != null) {
            if (this.thisHuman.visualInputCamera.targetTexture == null) {
                this.thisHuman.visualInputCamera.targetTexture = new RenderTexture(visualResolution, visualResolution, 24);
                /* 24 is the depth buffer, or depth texture, is actually just a render texture that contains values of how far objects in the scene are from the camera.*/
            }
            else {
                visualResolution = this.thisHuman.visualInputCamera.targetTexture.width;
                visualResolution = this.thisHuman.visualInputCamera.targetTexture.height;
            }
        }
    }

    public void updateBodyState(){
        bodyStateArray[bodyStateIndexDict[this.thisHuman.bodyState]] = 1.0f;
        bodyStateArray[bodyStateIndexDict[this.thisHuman.actionState]] = 1.0f;

        if (this.thisHuman.sleepingState == true){
            bodyStateArray[bodyStateIndexDict["sleeping"]] = 1.0f;
        }
        else{
            bodyStateArray[bodyStateIndexDict["sleeping"]] = 0.0f;
        }

        if (this.thisHuman.LHState == true){
            bodyStateArray[bodyStateIndexDict["holding with left hand"]] = 1.0f;
        }
        else{
            bodyStateArray[bodyStateIndexDict["holding with left hand"]] = 0.0f;
        }

        if (this.thisHuman.RHState == true){
            bodyStateArray[bodyStateIndexDict["holding with right hand"]] = 1.0f;
        }
        else{
            bodyStateArray[bodyStateIndexDict["holding with right hand"]] = 0.0f;
        }
    }

    public override float[] GetBodyState(){
        updateBodyState();
        return bodyStateArray;
    }

    public override float [ , ] GetVisualInput()
    {
        // we need to get the human's current camera image as a 3-d bitmap matrix of rgb-values
        // for example, if the camera resolution is 256x256, then the matrix would be 256x256x3, where those 
        // three are r,g,b
        // then we look in our preferences for what the nervous system resolution is, and we downsample
        // 32x32x3, 256/x = 32, x = 8, basically an algorithm that 
    
        return visualInput;
    }
}
