using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSystem 
{
    public Animal thisAnimal;
    protected Transform transform;

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

    public List<Action> actionList;

    public MotorSystem(Animal passed) {
        this.thisAnimal = passed;
    }

    public void InitStates(List<string> passedStateLabelList) {
        states = new bool[passedStateLabelList.Count];
        stateLabelList = passedStateLabelList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, bool>();

        if (passedStateLabelList != null){
            for (int i = 0; i < passedStateLabelList.Count; i++) {
                states[i] = false;
                stateIndexDict[passedStateLabelList[i]] = i;
                stateDict[passedStateLabelList[i]] = false;
            }
        } else { Debug.Log("No actions passed to this animal"); }
    }

    public virtual void InitActionDict(){}

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

    public void InitActionArguments(List<string> passedArgsLabels) {
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

    public virtual void TakeAction(int[] toDo) { }

    public virtual void EndAction(string actionLabel) { Debug.Log("No actions to end for this animal"); }

    public virtual void UpdateActionStates() { Debug.Log("No action updates defined for this animal"); }

    public virtual void UpdateSkeletonStates() { Debug.Log("No actions defined for this animal"); }

    public virtual bool CheckActionLegality(string action) {
        Debug.Log("No actions defined for this animal");
        return false;
    }
}