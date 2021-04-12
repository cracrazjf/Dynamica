using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class DriveSystem
{
    /// <value>Holds a reference to the Animal the system is associated with</value>
    protected Animal thisAnimal;
    protected Dictionary<string, float> thisTraitDict;

    protected Vector<float> states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, float> stateDict;
    
    public Vector<float> GetStates() { return states; }
    public float GetState(string place) { return stateDict[place]; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, float> GetStateDict() { return stateDict; }


    /// <summary>
    /// Drive constructor
    /// </summary>
    public DriveSystem(Animal animal) {
        this.thisAnimal = animal;

        this.stateLabelList = new List<string> {
            // Originally driveStates, will be read-in eventually
            "hunger", 
            "thirst", 
            "sleepiness",
            "fatigue",
            "health",
        };
        this.InitStates(this.stateLabelList);
        //SetState("health", 1.0f);
    }

   void InitStates(List<string> passedList) {
        thisTraitDict = thisAnimal.GetPhenotype().GetTraitDict();

        states = Vector<float>.Build.Dense(passedList.Count);
        stateLabelList = passedList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, float>();

        if (passedList != null){
            for (int i = 0; i < passedList.Count; i++) {
                // Debug.Log(stateLabelList[i]);
                states[i] = thisTraitDict[(stateLabelList[i] + "_value")];
                stateIndexDict[passedList[i]] = i;
                stateDict[passedList[i]] = states[i];
            }
        } else { Debug.Log("No actions passed to this animal"); }
    }

    // DISCUSS
    public void SetState(string label, float val) {
        stateDict[label] = val;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = val;
    }

    public void UpdateDrives() { 
        foreach (string label in stateLabelList) {
            string changeLabel = label + "_change";
            float changeValue = thisTraitDict[changeLabel];

            float toUpdate = GetStateDict()[label] + changeValue;
            SetState(label, toUpdate);
            // Ensure all drive states are in bounds
            if (this.stateDict[label] < -1f) { this.SetState(label, -1f); }
            else if (this.stateDict[label] > 1f) { this.SetState(label, 1.0f); }
        }

        // Hunger and thirst updates
        float currentHealth = stateDict["health"];
        if (stateDict["hunger"] >= 1.0 ) {
            float toUpdate = currentHealth - thisTraitDict["starvation_damage"];
            this.SetState("health", toUpdate);
        }
        if (stateDict["thirst"] >= 1.0) {
            float toUpdate = currentHealth - thisTraitDict["dehydration_damage"];
            this.SetState("health", toUpdate);
        }    
    } 
}