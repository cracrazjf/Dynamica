using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MotorSystem 
{
    protected Animal thisAnimal;
    protected AnimalBody thisBody;
    protected List<Action> actionList;

    protected bool[] states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, bool> stateDict;
    
    public bool[] GetStates() { return states; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, bool> GetStateDict() { return stateDict; }
    
    protected float[] args;
    protected List<string> argsLabelList;
    protected Dictionary<string, int> argsIndexDict;
    protected Dictionary<string, float> argsDict;

    public float[] GetArgs() { return args; }
    public List<string> GetArgLabels() { return argsLabelList; }
    public Dictionary<string, int> GetArgIndices() { return argsIndexDict; }
    public Dictionary<string, float> GetArgDict() { return argsDict; }


    public MotorSystem(Animal passed) {
        thisAnimal = passed;
        this.thisBody = thisAnimal.GetBody();

        stateLabelList = new List<string> {
            "sitting down",// 0
            "sitting up",  // 1
            "laying down", // 2
            "standing up", // 3
            "rotating",    // 4
            "taking steps",// 5
            "picking up",  // 6
            "setting down",// 7 
            "consuming",   // 8
            "waking up",   // 9
            "sleeping",    // 10
            "resting",     // 11
            "looking"      // 12
        };
        this.InitStates(stateLabelList);

        argsLabelList = new List<string> {
            "step proportion",                          
            "rotation proportion",               
            "held position",
            "target x",
            "target y",
            "target z"
        };

        this.InitActionArguments(argsLabelList);
        this.InitActionDict();
    }

    public void SetState(string label, bool val) {
        stateDict[label] = val;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = val;
    }

    public void SetArgs(string label, float val) {
        argsDict[label] = val;
        int currentIndex = argsIndexDict[label];
        args[currentIndex] = val;
    }

    public void TakeAction(int[,] things) {

        SetArgs("step proportion", 1);
        SetArgs("rotation proportion", 1);

        for(int i = 0; i < states.Length; i++) {
            // switched from i == 1... my bad
            if (things[0 , i] == 1) {
                Debug.Log("Doing action at " + i);
                actionList[i].DynamicInvoke();
            } 
        }
    }

    void InitStates(List<string> passedList) {
        states = new bool[passedList.Count];
        stateLabelList = passedList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, bool>();

        if (passedList != null){
            for (int i = 0; i < passedList.Count; i++) {
                states[i] = false;
                stateIndexDict[passedList[i]] = i;
                stateDict[passedList[i]] = false;
            }
        } else { Debug.Log("No actions passed to this animal"); }
    }

    void InitActionArguments(List<string> passedArgsLabels) {
        args = new float[passedArgsLabels.Count];
        argsLabelList = passedArgsLabels;
        argsIndexDict = new Dictionary<string, int>();
        argsDict = new Dictionary<string, float>();

        if (passedArgsLabels != null){
            for (int i = 0; i < passedArgsLabels.Count; i++) {
                args[i] = 0.0f;
                argsIndexDict[passedArgsLabels[i]] = i;
                argsDict[passedArgsLabels[i]] = 0.0f;
            }
        } else { Debug.Log("No args defined for this animal"); }
    }
    void InitActionDict() {
        actionList = new List<Action>();

        actionList.Add(SitDown);
        actionList.Add(SitUp);
        actionList.Add(LayDown);
        actionList.Add(StandUp);
        actionList.Add(Rotate);
        actionList.Add(TakeSteps);
        actionList.Add(PickUp);
        actionList.Add(SetDown);
        actionList.Add(Consume);
        actionList.Add(WakeUp);
        actionList.Add(Sleep);
        actionList.Add(Rest);
        actionList.Add(LookAt);
    }

    public abstract void SitDown();
    public abstract void SitUp();
    public abstract void LayDown();
    public abstract void StandUp();
    public abstract void Rotate();
    public abstract void TakeSteps();
    public abstract void PickUp();
    public abstract void SetDown();
    public abstract void Consume ();
    public abstract void WakeUp();
    public abstract void Sleep();
    public abstract void Rest();
    public abstract void LookAt();
}