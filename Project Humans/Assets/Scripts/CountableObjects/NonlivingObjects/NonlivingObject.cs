using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;


abstract public class NonlivingObject : CountableObject
{
    public int numConstants;
    public List<string> constantLabelList;
    public Dictionary<string, int> constantIndexDict;
    public Dictionary<string, float> constantDict;

    public NonlivingObject(string objectType, int index, Nullable<Vector3> position, NonlivingObjectInfo passedNonlivingObjectInfo) : 
            base (objectType, index, position) 
    {
        // this could possibly be made more efficient with data structure copying methods
        numConstants = 0;
        constantLabelList = new List<string>();
        constantIndexDict = new Dictionary<string, int>();
        constantDict = new Dictionary<string, float>();
        for (int i=0; i<passedNonlivingObjectInfo.numConstants; i++){
            constantLabelList.Add(passedNonlivingObjectInfo.constantLabelList[i]);
            constantIndexDict.Add(passedNonlivingObjectInfo.constantLabelList[i], numConstants);
            constantDict.Add(passedNonlivingObjectInfo.constantLabelList[i], passedNonlivingObjectInfo.constantDict[passedNonlivingObjectInfo.constantLabelList[i]]);
            numConstants++;
        }
    }

    public void AddConstant(string label, float value){

        if (constantDict.ContainsKey(label)) {
            Debug.Log("Key exists in constant dict, changing value");
            constantDict[label] = value;
        }
        else{
            constantLabelList.Add(label);
            constantIndexDict.Add(label, numConstants);
            constantDict.Add(label, value);
            numConstants++;
        }
    }

    public float GetConstant(string label){
        return constantDict[label];
    }
}
