using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSystem 
{
    public Animal thisAnimal;

    protected int numActionStates;
    protected bool[] actionStateArray;
    protected List<string> actionStateLabelList;
    protected Dictionary<string, int> actionStateIndexDict;

    protected int numActionArguments;
    protected float[] actionArgumentArray;
    protected List<string> actionArgumentLabelList;
    protected Dictionary<string, int> actionArgumentIndexDict;

    protected Dictionary<string, List<string>> actionRequirementDict = new Dictionary<string, List<string>>();
    protected Dictionary<string, List<string>> actionObstructorDict = new Dictionary<string, List<string>>();
    protected Dictionary<string, List<string>> bodyStateRequirementDict = new Dictionary<string, List<string>>();
    protected Dictionary<string, List<string>> bodyStateObstructorDict = new Dictionary<string, List<string>>();


    public MotorSystem(Animal passed) {
        this.thisAnimal = passed;
    }

    public void InitActionStates(){
        this.actionStateLabelList = actionStateLabelList;

        actionStateIndexDict = new Dictionary <string, int>();

        int i;
        for (i=0; i<actionStateLabelList.Count; i++){
            actionStateIndexDict.Add(actionStateLabelList[i], i);
            
            actionRequirementDict.Add(actionStateLabelList[i], new List<string>());
            actionObstructorDict.Add(actionStateLabelList[i], new List<string>());
            bodyStateRequirementDict.Add(bodyStateLabelList[i], new List<string>());
            bodyStateObstructorDict.Add(bodyStateLabelList[i], new List<string>());
        }
        numActionStates = i;
        actionStateArray = new bool[numBodyStates];
    }

    public void InitActionArguments(){
        actionArgumentIndexDict = new Dictionary <string, int>();

        int i;
        for (i=0; i<actionArgumentLabelList.Count; i++){
            actionArgumentIndexDict.Add(actionArgumentLabelList[i], i);
        }
        numActionArguments = i;
        actionArgumentArray = new float[numActionArguments];
    }

    public virtual void InitActionRuleDicts(){
        Debug.Log("No Action Rules for this animal");
    }

    public virtual void TakeAction(struct actionChoiceStruct){
        Debug.Log("No Actions Defined for this animal");
    }

    public virtual void UpdateActionStates(){
        Debug.Log("No Actions Defined for this animal");
    }

    public virtual bool CheckActionLegality(){
        Debug.Log("No Actions Defined for this animal");
        return false;
    }

    public int GetNumActionStates(){
        return numActionStates;
    }

}