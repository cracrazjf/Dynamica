using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSystem
{
    /// <value>Holds a reference to the Animal the system is associated with</value>
    public Animal thisAnimal;

    private int numDriveStates;
    private float[] driveStateArray;
    private List<string> driveStateLabelList;
    private Dictionary<string, int> driveStateIndexDict;

    /// <summary>
    /// Drive constructor
    /// </summary>
    public DriveSystem(Animal animal) {
        this.thisAnimal = animal;
    }

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
    public int GetNumDriveStates(){
        return numDriveStates;
    }

    public float[] GetDriveStateLabelList(){
        return driveStateLabelList;
    }

    public bool[] GetDriveStateArray(){
        return driveStateArray;
    }

    public Dictionary<string, int> GetDriveStateIndexDict(){
        return driveStateIndexDict;
    }

    public string getDriveStateLabel(int index){
        return driveStateLabelList[index];
    }

    public float getDriveState(int index){
        return driveStateArray[index];
    }

    public int getDriveStateIndex(string label){
        return driveStateIndexDict[label];
    }
}