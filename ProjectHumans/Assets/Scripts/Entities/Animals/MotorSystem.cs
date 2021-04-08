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
            "laying down", // 2
            "standing up", // 3
            "rotating",    // 4, now a proportion
            "taking steps",// 5, now a proportion
            "hand action", // 6, release/maintain/grab
            "active hand", // 7  
            "consuming",   // 8
            "sleeping",    // 9, awake/maintain/fall asleep
            "resting",     // 11
            "looking",     // 12
            "RP x",        // 13
            "RP y",        // 14
            "RP z"         // 15
        };
        this.InitStates(stateLabelList);
    }

    public void SetState(string label, float val) {
        stateDict[label] = val;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = val;
    }

    public void TakeAction(Vector<float> things) {

        for(int i = 0; i < states.Length; i++) {
            // switched from i == 1... my bad
            if (things[i] == 1f) {
                //Debug.Log("Doing action at " + i);
                actionList[i].DynamicInvoke();
            } 
        }
    }

    void InitStates(List<string> passedList) {
        states = new Vector<float>();
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