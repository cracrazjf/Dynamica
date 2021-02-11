using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSystem
{
    /// <value>Holds a reference to the Animal the system is associated with</value>
    public Animal thisAnimal;

    protected int numDriveStates;
    protected List<string> driveStateLabelList;
    protected Dictionary<string, float> driveStateDict;

    /// <summary>
    /// Drive constructor
    /// </summary>
    public DriveSystem(Animal animal) {
        this.thisAnimal = animal;
    }

    public void InitDriveStates(List<string> driveStateLabelList) {
        driveStateDict = new Dictionary <string, float>();
        foreach (string drive in driveStateLabelList) {
            driveStateDict[drive] = 0;
        }
        numDriveStates = driveStateDict.Count;
    }

    public virtual void UpdateDrives() {
        Debug.Log("No drive states defined for this animal");
    }

    public int GetNumDriveStates() {
        return numDriveStates;
    }

    public List<string> GetDriveStateLabelList() {
        return driveStateLabelList;
    }

    public Dictionary<string, float> GetDriveStateDict() {
        return driveStateDict;
    }

    public void SetDriveState(string label, float val) {
        driveStateDict[label] = val;
    }
}