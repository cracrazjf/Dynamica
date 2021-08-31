using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public abstract class MotorSystem 
{
    protected Animal thisAnimal;
    protected AnimalBody thisBody;
    public List<Action> actionList;
    protected Vector<float> paramCopy;

    protected Vector<float> states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    public Dictionary<string, float> stateDict;
    protected int numArgs;
    
    public Vector<float> GetStates() { return states; }
    public float GetState(string place) { return stateDict[place]; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, float> GetStateDict() { return stateDict; }

    public bool isCrouching;
    public bool setAxis;
    public bool reached;
    public Transform rightHand;
    public Transform leftHand;

    public MotorSystem(Animal passed) {
        thisAnimal = passed;
        this.thisBody = thisAnimal.GetBody();

        stateLabelList = new List<string> {
            "crouch",      // 0, negative is down 
            "sit",         // 1, negative is down
            "lay",         // 2 -1 or 1 (or 0 if not switched)
            "stand",       // 3 -1 or 1 (or 0 if not switched)
            "rotate",      // 4, now a proportion
            "take steps",  // 5, now a proportion
            "consume",     // 6, set to consumable if ongoing
            "sleep",       // 7, awake/maintain/fall asleep
            "rest",        // 8 -1 or 1 (or 0 if not switched)
            "look vertically",   // 9 -1 or 1 (or 0 if not switched)
            "look horizontally", // 10 -1 or 1 (or 0 if not switched)
            "hand action", // 11, release/maintain/grab
            "active right",// 12  -1 if left, or 1 if right
            "active left", // 13  -1 if left, or 1 if right
            "RP x",        // 14  -1 to 1, proportion of max range from start pos
            "RP y",        // 15
            "RP z",        // 16
            
        };
        this.InitStates(stateLabelList);
        this.InitActionDict();
        this.numArgs = 4;
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

        //stateDict["take steps"] = 1.0f;
        //actionList[5](); //take steps

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

    public abstract void Crouch();
    public abstract void Sit();
    public abstract void Lay();
    public abstract void Stand();
    public abstract void Rotate();
    public abstract void TakeSteps();
    public abstract void Consume();
    public abstract void Sleep();
    public abstract void Rest();
    public abstract void Look();
    public abstract void UseHand();
}