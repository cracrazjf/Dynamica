using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSystem
{
    /// <value>Holds a reference to the Animal the system is associated with</value>
    public Animal thisAnimal;

    /// <value>List of states</value>
    public List<string> stateLabelList = new List<string> { "hunger", "thirst", "wakefulness", "energy", "health"};

    /// <value>Num of states</value>
    public int numStates; 

    public Dictionary<string, int> stateIndexDict = new Dictionary<string, int>();
    /// <value>List of values for each state, set to baseline in Start</value>
    public Dictionary<string, float> stateDict = new Dictionary<string, float>();

    /// <value>List of values for each state, automatically set to True for all</value>
    public Dictionary<string, bool> stateDisplayDict = new Dictionary<string, bool>();

    /// <summary>
    /// Drive constructor
    /// </summary>
    public DriveSystem(Animal animal) {
        this.thisAnimal = animal;
        //set baseline state values
        
        numStates = stateLabelList.Count;

        for (int i = 0; i < numStates; i++)
        {
            // make the state dict
            //stateValueList[i] = new List<float>{ 0.001f, 0.001f, 0.001f, -0.001f, 0.001f };
            stateIndexDict.Add(stateLabelList[i], i);
            //set baselines to 0
            stateDict.Add(stateLabelList[i], 1.0f);

            // default each state display to true
            stateDisplayDict.Add(stateLabelList[i], true);
        }
    }

    public void UpdateDrives()
    {
        for (int i = 0; i < numStates; i++) {
            string stateLabel = stateLabelList[i];
            string changeLabel = stateLabel + "_change";
            
            // add some error checking to make sure the labels in config and stateLabelList match
            float changeValue = float.Parse(thisAnimal.phenotype.traitDict[changeLabel]);
            stateDict[stateLabel] += changeValue;

            if (stateDict["hunger"] <= 0.0){
                // add check to make sure starvation_damage is a key in the dict
                stateDict["health"] -= float.Parse(thisAnimal.phenotype.traitDict["starvation_damage"]);
            }
            if (stateDict["thirst"] <= 0.0){
                // add check to make sure dehydration_damage is a key in the dict
                stateDict["health"] -= float.Parse(thisAnimal.phenotype.traitDict["dehydration_damage"]);
            }

            if (stateDict[stateLabel] > 1.0)
            {
                stateDict[stateLabel] = 1.0f;
            }
            else if (stateDict[stateLabel] < 0.0)
            {
                stateDict[stateLabel] = 0.0f;
            }
        }
    }
}