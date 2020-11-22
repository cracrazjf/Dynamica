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
        if(this.thisHuman.actionState != "none") {
            bodyStateArray[bodyStateIndexDict[this.thisHuman.actionState]] = 1.0f;
        }
        

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
        if (this.thisHuman.visualInputCamera.gameObject.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Space)){ // replace this with the visual refresh rate at some point
                Texture2D visualInputTexture = new Texture2D(visualResolution, visualResolution, TextureFormat.RGB24, false);

                this.thisHuman.visualInputCamera.Render();
                RenderTexture.active = this.thisHuman.visualInputCamera.targetTexture;
                visualInputTexture.ReadPixels(new Rect(0, 0, visualResolution, visualResolution), 0, 0);

                //visualInputTexture = ResizeTexture(visualInputTexture, (float)nervousSystemRes / (float)resWidth);

                Color[] visualInputArray = visualInputTexture.GetPixels();
                
                Debug.Log(visualInputArray.Length);
                // 1024

                int resolutionSquared = visualResolution*visualResolution;
                for (int i=0; i<resolutionSquared; i++){
                    visualInput[0,i] = visualInputArray[i].r;
                    visualInput[1,i] = visualInputArray[i].g;
                    visualInput[2,i] = visualInputArray[i].b;
                }

                saveVisualImage(visualInputTexture);
            
            }
            else{
                // return the previous visual state
            }
        }
        else{
            Debug.Log("Camera is off");
        }
        

        return visualInput;
    
    }

    void saveVisualImage(Texture2D visualInputTexture){
        byte[] bytes = visualInputTexture.EncodeToPNG();
        string fileName = visualInputName();
        System.IO.File.WriteAllBytes(fileName, bytes);
    }

    string visualInputName() {
        return string.Format("{0}/VisualInputs/visualInput_{1}x{2}_{3}.png",
        Application.dataPath,
        visualResolution,
        visualResolution,
        System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    Texture2D ResizeTexture(Texture2D pSource, float pScale){
        //*** Variables
        int i;
    
        //*** Get All the source pixels
        Color[] aSourceColor = pSource.GetPixels(0);
        Vector2 vSourceSize = new Vector2(pSource.width, pSource.height);
    
        //*** Calculate New Size
        float xWidth = Mathf.RoundToInt((float)pSource.width * pScale);                     
        float xHeight = Mathf.RoundToInt((float)pSource.height * pScale);
    
        //*** Make New
        Texture2D oNewTex = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGB24, false);
    
        //*** Make destination array
        int xLength = (int)xWidth * (int)xHeight;
        Color[] aColor = new Color[xLength];
    
        Vector2 vPixelSize = new Vector2(vSourceSize.x / xWidth, vSourceSize.y / xHeight);
    
        //*** Loop through destination pixels and process
        Vector2 vCenter = new Vector2();
        for(i=0; i<xLength; i++){
    
            //*** Figure out x&y
            float xX = (float)i % xWidth;
            float xY = Mathf.Floor((float)i / xWidth);
    
            //*** Calculate Center
            vCenter.x = (xX / xWidth) * vSourceSize.x;
            vCenter.y = (xY / xHeight) * vSourceSize.y;


            int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
            int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
            int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
            int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);
 
            //*** Loop and accumulate
            Vector4 oColorTotal = new Vector4();
            Color oColorTemp = new Color();
            float xGridCount = 0;
            for(int iy = xYFrom; iy < xYTo; iy++){
                for(int ix = xXFrom; ix < xXTo; ix++){
 
                    //*** Get Color
                    oColorTemp += aSourceColor[(int)(((float)iy * vSourceSize.x) + ix)];
 
                    //*** Sum
                    xGridCount++;
                }
            }
 
            //*** Average Color
            aColor[i] = oColorTemp / (float)xGridCount;
        }
        oNewTex.SetPixels(aColor);
        oNewTex.Apply();
        Debug.Log("visualInput taken");
        return oNewTex;
        

    }
}
