using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class SimpleMotorSystem : MotorSystem {
    protected Animal thisAnimal;
    protected AnimalBody thisBody;
    protected List<Action> actionList;
    protected Vector<float> paramCopy;

    protected Vector<float> states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, float> stateDict;
    protected int numArgs;
    
    public Vector<float> GetStates() { return states; }
    public float GetState(string place) { return stateDict[place]; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, float> GetStateDict() { return stateDict; }


    public SimpleMotorSystem(Animal passed) : base(passed) {}

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
        
        stateDict["right"] = 0.3f;
        stateDict["RP x"] = 1;
        stateDict["RP z"] = 1;
        actionList[10](); //usehand

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
        actionList.Add(Consume);
        actionList.Add(Sleep);
        actionList.Add(Rest);
        actionList.Add(Look);
        actionList.Add(UseHand);
    }

    public override void Crouch() {
        Debug.Log("Called here.");
    }
    public override void Sit() {
        Debug.Log("Called here.");
    }
    public override void Lay() {
        Debug.Log("Called here.");
    }
    public override void Stand() {
        Debug.Log("Called here.");
    }
    public override void Rotate() {
        Debug.Log("Called here.");
    }
    public override void TakeSteps() {
        Debug.Log("Called here.");
    }
    public override void Consume() {
        Debug.Log("Called here.");
    }
    public override void Sleep() {
        Debug.Log("Called here.");
    }
    public override void Rest() {
        Debug.Log("Called here.");
    }
    public override void Look() {
        Debug.Log("Called here.");
    }
    public override void UseHand() {
        Debug.Log("Called here.");
    }
}