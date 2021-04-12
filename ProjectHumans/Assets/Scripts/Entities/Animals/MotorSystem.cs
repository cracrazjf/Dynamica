using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public abstract class MotorSystem 
{
    protected Animal thisAnimal;
    protected AnimalBody thisBody;
    protected List<Action> actionList;
    protected Vector<float> paramCopy;

    protected Vector<float> states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, float> stateDict;
    
    public Vector<float> GetStates() { return states; }
    public float GetState(string place) { return stateDict[place]; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, float> GetStateDict() { return stateDict; }


    public MotorSystem(Animal passed) {
        thisAnimal = passed;
        this.thisBody = thisAnimal.GetBody();

        stateLabelList = new List<string> {
            "crouching",   // 0, negative is down 
            "sitting",     // 1, negative is down
            "laying down", // 2 -1 or 1 (or 0 if not switched)
            "standing up", // 3 -1 or 1 (or 0 if not switched)
            "rotating",    // 4, now a proportion
            "taking steps",// 5, now a proportion
            "hand action", // 6, release/maintain/grab
            "active right",// 7  -1 or 1 (1 if right)
            "consuming",   // 8, set to consumable if ongoing
            "sleeping",    // 9, awake/maintain/fall asleep
            "resting",     // 11 -1 or 1 (or 0 if not switched)
            "looking",     // 12 -1 or 1 (or 0 if not switched)
            "RP x",        // 13  -1 to 1, proportion of max range from start pos
            "RP y",        // 14
            "RP z"         // 15
        };
        this.InitStates(stateLabelList);
        this.InitActionDict();
    }

    public void SetState(string label, float val) {
        stateDict[label] = val;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = val;
    }

    public void SetState(int index, float val) {
        string label  = stateLabelList[index];
        stateDict[label] = val;
        states[index] = val;
    }

    public void TakeAction(Vector<float> actions) {
        Debug.Log(actions);
        if (actions[11] != 1) {
            for(int i = 0; i < states.Count; i++) {
                if (actions[i] != 0) {
                    SetState(i, actions[i]);
                    Debug.Log("Doing action at " + i);
                    actionList[i].DynamicInvoke();
                }
            } 
        } else {
            Rest();
        }
    }

    void InitStates(List<string> passedList) {
        states = Vector<float>.Build.Dense(passedList.Count);
        stateLabelList = passedList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, float>();

        if (passedList != null){
            for (int i = 0; i < passedList.Count; i++) {
                states[i] = 0f;
                stateIndexDict[passedList[i]] = i;
                stateDict[passedList[i]] = 0f;
            }
        } else { Debug.Log("No actions passed to this animal"); }
    }

    void InitActionDict() {
        actionList = new List<Action>();

        actionList.Add(Crouch);
        actionList.Add(Sit);
        actionList.Add(Lay);
        actionList.Add(Stand);
        actionList.Add(Rotate);
        actionList.Add(TakeSteps);
        actionList.Add(UseHand);
        actionList.Add(Consume);
        actionList.Add(Sleep);
        actionList.Add(Rest);
        actionList.Add(Look);
    }

    public abstract void Crouch();
    public abstract void Sit();
    public abstract void Lay();
    public abstract void Stand();
    public abstract void Rotate();
    public abstract void TakeSteps();
    public abstract void UseHand();
    public abstract void Consume();
    public abstract void Sleep();
    public abstract void Rest();
    public abstract void Look();
}