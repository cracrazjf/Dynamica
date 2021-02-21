using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSystem 
{
    public Animal thisAnimal;

    protected int numActionChoice;
    protected Dictionary<string, bool> actionChoiceDict;

    protected int numActionArguments;
    protected Dictionary<string, float> actionArgumentDict;

    protected Dictionary<string, List<string>> actionRequirementDict = new Dictionary<string, List<string>>();
    protected Dictionary<string, List<string>> actionObstructorDict = new Dictionary<string, List<string>>();
    protected Dictionary<string, List<string>> bodyStateRequirementDict = new Dictionary<string, List<string>>();
    protected Dictionary<string, List<string>> bodyStateObstructorDict = new Dictionary<string, List<string>>();


    public MotorSystem(Animal passed) {
        this.thisAnimal = passed;
    }

    public virtual void InitActionRuleDicts() {
        Debug.Log("No action rules for this animal");
    }

    public virtual void InitActionStates() {
        Debug.Log("No action rules for this animal");
    }

    public virtual void InitActionArguments() {
        Debug.Log("No action rules for this animal");
    }


    public virtual void TakeAction(AI.ActionChoiceStruct actionChoiceStruct) {
        Debug.Log("No actions defined for this animal");
    }
    public virtual void EndAction(string actionLabel) {
        Debug.Log("No actions defined for this animal");
    }

    public virtual void UpdateSkeletonStates() {
        Debug.Log("No actions defined for this animal");
    }

    public virtual bool CheckActionLegality(string action) {
        Debug.Log("No actions defined for this animal");
        return false;
    }

    public int GetNumActionStates() {
        return numActionChoice;
    }

    public Dictionary<string, bool> GetActionStateDict() {
        return actionChoiceDict;
    }

    public int GetNumActionArguments() {
        return numActionArguments;
    }
    
    public Dictionary<string, float> GetActionArgumentDict() {
        return actionArgumentDict;
    }
}