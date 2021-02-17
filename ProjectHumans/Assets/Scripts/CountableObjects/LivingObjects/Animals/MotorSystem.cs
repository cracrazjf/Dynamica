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
    
    protected float[] actionArgs;
    protected List<string> argsLabelList;
    protected Dictionary<string, int> argsIndexDict;
    protected Dictionary<string, float> actionArgsDict;

    public float[] GetArgs() { return actionArgs; }
    public List<string> GetArgLabels() { return argsLabelList; }
    public Dictionary<string, int> GetArgIndices() { return argsIndexDict; }
    public Dictionary<string, float> GetArgDict() { return actionArgsDict; }

    // DISCUSS
    public MotorSystem(Animal passed) {
        this.thisAnimal = passed;
    }

    // DISCUSS
    public void InitStates(List<string> passedStateLabelList){
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

    // DISCUSS
    public void SetState(string label, bool val) {
        stateDict[label] = val;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = val;
    }

    public void InitActionArguments(List<string> passedArgsLabels){
        actionArgs = new float[passedArgsLabels.Count];
        argsLabelList = passedArgsLabels;
        argsIndexDict = new Dictionary<string, int>();
        actionArgsDict = new Dictionary<string, float>();

        if (passedArgsLabels != null){
            for (int i = 0; i < passedArgsLabels.Count; i++) {
                actionArgs[i] = 0.0f;
                argsIndexDict[passedArgsLabels[i]] = i;
                actionArgsDict[passedArgsLabels[i]] = 0.0f;
            }
        }
        else { Debug.Log("No args defined for this animal"); }
    }

    public virtual void TakeAction(AI.ActionChoiceStruct actionChoiceStruct) { Debug.Log("No actions defined for this animal"); }

    public virtual void EndAction(string actionLabel) { Debug.Log("No actions to end for this animal"); }

    public virtual void UpdateActionStates() { Debug.Log("No action updates defined for this animal"); }
}