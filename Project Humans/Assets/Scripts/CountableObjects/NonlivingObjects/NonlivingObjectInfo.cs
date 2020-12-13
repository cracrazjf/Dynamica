using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;


public class NonlivingObjectInfo {
    // this is the initial number that will be spawned
    public string name;
    public int startingN;
    public int currentN;

    public int numConstants;
    public List<string> constantLabelList;
    public Dictionary<string, int> constantIndexDict;
    public Dictionary<string, float> constantDict;

    public NonlivingObjectInfo(string passedName, int passedStartingN) {
        name = passedName;
        startingN = passedStartingN;
        currentN = 0;

        numConstants = 0;
        constantLabelList = new List<string>();
        constantIndexDict = new Dictionary<string, int>();
        constantDict = new Dictionary<string, float>();

        ImportObjectInfo();
    }

    public void ImportObjectInfo(){

        string line;
        System.IO.StreamReader file;
        
        string filename = @"Assets/Scripts/config/"+ name.ToLower() + ".config";
        file = new System.IO.StreamReader(filename);  
        
        while((line = file.ReadLine()) != null)  
        {  
            string[] lineInfo = line.Split(new[] { "=" }, StringSplitOptions.None);
            string[] leftArray = lineInfo[0].Split(new[] { "." }, StringSplitOptions.None);
            string[] rightArray = lineInfo[1].Split(new[] { "," }, StringSplitOptions.None);

            if (leftArray[0] == "constant"){
                constantLabelList.Add(leftArray[1]);
                constantIndexDict.Add(leftArray[1], numConstants);
                constantDict.Add(leftArray[1], float.Parse(rightArray[0]));
                numConstants++;
            }
        }  
        
        file.Close();
    }
}