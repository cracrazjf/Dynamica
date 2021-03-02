using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSystem
{
    /// <value>Holds a reference to the Animal the system is associated with</value>
    public Animal thisAnimal;

    protected float[] states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, float> stateDict;
    
    public float[] GetStates() { return states; }
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
        SetState("health", 1.0f);
    }

    // DISCUSS
    public void InitStates(List<string> passedStateLabelList){
        states = new float[passedStateLabelList.Count];
        stateLabelList = passedStateLabelList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, float>();

        if (passedStateLabelList != null){
            for (int i = 0; i < passedStateLabelList.Count; i++) {
                states[i] = 0.0f;
                stateIndexDict[passedStateLabelList[i]] = i;
                stateDict[passedStateLabelList[i]] = 0.0f;
            }
        } else { Debug.Log("No drives passed to this animal"); }
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
            float changeValue = thisAnimal.phenotype.GetTraitDict()[changeLabel];

            float toUpdate = GetStateDict()[label] + changeValue;
            SetState(label, toUpdate);
            // Ensure all drive states are in bounds
            if (this.stateDict[label] < 0) { this.SetState(label, 0.0f); }
            else if (this.stateDict[label] > 1) { this.SetState(label, 1.0f); }
        }

        // Hunger and thirst updates
        float currentHealth = stateDict["health"];
        if (stateDict["hunger"] >= 1.0 ) {
            float toUpdate = currentHealth - thisAnimal.phenotype.GetTraitDict()["starvation_damage"];
            this.SetState("health", toUpdate);
        }
        if (stateDict["thirst"] >= 1.0) {
            float toUpdate = currentHealth - thisAnimal.phenotype.GetTraitDict()["dehydration_damage"];
            this.SetState("health", toUpdate);
        }    
    } 
}