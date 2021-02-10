using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSystem
{
    /// <value>Holds a reference to the Animal the system is associated with</value>
    public Animal thisAnimal;

    protected int numDriveStates;
    protected float[] driveStateArray = null;
    protected List<string> driveStateLabelList;
    protected Dictionary<string, int> driveStateIndexDict;

    /// <summary>
    /// Drive constructor
    /// </summary>
    public DriveSystem(Animal animal) {
        this.thisAnimal = animal;
    }

    //why is this not automatically called in the constructor? where is this information declared if not here? -jc
    public void InitDriveStates(List<string> driveStateLabelList){
        driveStateIndexDict = new Dictionary <string, int>();

        int i;
        for (i=0; i<driveStateLabelList.Count; i++){
            driveStateIndexDict.Add(driveStateLabelList[i], i);
        }
        numDriveStates = i;
        driveStateArray = new float[numDriveStates];
    }

    public virtual void UpdateDrives(){
        Debug.Log("No drive states defined for this animal");
    }


    // getters for drive state data structures
    public void SetDriveState(int index, float value){
        driveStateArray[index] = value;
    }
    public int GetNumDriveStates(){
        return numDriveStates;
    }

    public List<string> GetDriveStateLabelList(){
        return driveStateLabelList;
    }

    public float[] GetDriveStateArray(){
        return driveStateArray;
    }

    public Dictionary<string, int> GetDriveStateIndexDict(){
        return driveStateIndexDict;
    }

    public string GetDriveStateLabel(int index){
        return driveStateLabelList[index];
    }

    public float GetDriveState(int index){
        return driveStateArray[index];
    }

    public int GetDriveStateIndex(string label){
        return driveStateIndexDict[label];
    }
}