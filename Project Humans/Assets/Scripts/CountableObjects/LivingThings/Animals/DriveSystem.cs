using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSystem
{
    //private Phenotype phenotype;

    /// <value>Holds a reference to the Human the system is associated with</value>
    // private Human human;

    /// <value>Holds a reference to the Animal the system is associated with</value>
    private Animal animal;

    /// <value>List of states</value>
    public List<string> stateLabelList = new List<string> { "hunger", "thirst", "sleepiness", "fatigue", "health"};

    /// <value>Num of states</value>
    public int numStates ;  
    public int STATES;

    public Dictionary<string, int> stateIndexDict = new Dictionary<string, int>();
    /// <value>List of values for each state, set to baseline in Start</value>
    public List<float> stateValueList = new List<float>();

    /// <value>List of values for each state, automatically set to True for all</value>
    public List<float> stateDisplayList = new List<float>();

    // Jon -- do we need this?
    public float[] state_thresholds = { 0.90f, 0.90f, 0.001f, 0.99f, 0.90f };

    /// <value>List of values to resolve each state at corresponding index</value>
    public List<Action> state_action = new List<Action>();

    /// <summary>
    /// Drive constructor
    /// </summary>
    public DriveSystem(Animal thisAnimal) {
        this.animal = thisAnimal;

        //set baseline state values
        
        for (int i = 0; i < numStates; i++)
        {
            // make the state dict
            //stateValueList[i] = new List<float>{ 0.001f, 0.001f, 0.001f, -0.001f, 0.001f };
            stateIndexDict.Add(stateLabelList[i], i);
            //set baselines to 0
            stateValueList.Add(0);

            // default each state display to true
            stateDisplayList.Add(1);
        }
    }

    private void Start(){
        numStates = stateLabelList.Count; 
    }

    public void UpdateDrives()
    {
        float traitValue = 0;

        for (int i = 0; i < STATES; i++) {

            /* change_label = stateLabelList[i] + "_change";
            trait_index = human.phenotype.traitIndexDict[change_label];
            stateValueList[i] += trait_value * Time.deltaTime;
            if(stateValueList[i] > 1)
            {stateValueList[i] = 1;} */

            //traitValue =  human.phenotype.traitValueList[i];
            if (stateValueList[i] < 1.0)
            {
                stateValueList[i] += traitValue * Time.deltaTime;
            }
        }
    }
}