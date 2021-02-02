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

    public void InitActionStates(List<string> passedActionStateLableList){

        actionStateIndexDict = new Dictionary <string, int>();
        int i;
        for (i=0; i<passedActionStateLableList.Count; i++){
            actionStateIndexDict.Add(passedActionStateLableList[i], i);
            
            actionRequirementDict.Add(passedActionStateLableList[i], new List<string>());
            actionObstructorDict.Add(passedActionStateLableList[i], new List<string>());
            bodyStateRequirementDict.Add(passedActionStateLableList[i], new List<string>());
            bodyStateObstructorDict.Add(passedActionStateLableList[i], new List<string>());
        }
        numActionStates = i;
        actionStateArray = new bool[this.GetNumActionStates()];
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

    public virtual void TakeAction(Animal.ActionChoiceStruct actionChoiceStruct){
        Debug.Log("No Actions Defined for this animal");
    }
    public virtual void EndAction(string actionLabel){
        Debug.Log("No Actions Defined for this animal");
    }

    public virtual void UpdateActionStates(){
        Debug.Log("No Actions Defined for this animal");
    }

    public virtual bool CheckActionLegality(string action){
        Debug.Log("No Actions Defined for this animal");
        return false;
    }

    public int GetNumActionStates(){
        return numActionStates;
    }

    public bool[] GetActionStateArray() {
        return actionStateArray;
    }

    public bool GetActionState(int index) {
        return actionStateArray[index];
    }

    public Dictionary<string, int> GetActionStateIndexDict() {
        return actionStateIndexDict;
    }

    public int getActionStateIndex(string label){
        return actionStateIndexDict[label];
    }

    public List<string> GetActionStateLabelList() {
        return actionStateLabelList;
    }

    public string GetActionStateLabel(int index){
        return actionStateLabelList[index];
    }

    public int GetNumActionArguments() {
        return numActionArguments;
    }

    public float[] GetActionArgumentArray() {
        return actionArgumentArray;
    }

    public float GetActionArgument(int index) {
        return actionArgumentArray[index];
    }
    
    public Dictionary<string, int> GetActionArgumentIndexDict() {
        return actionArgumentIndexDict;
    }
    
    public int GetActionArgumentIndex(string label) {
        return actionArgumentIndexDict[label];
    }

    public List<string> GetActionArgumentLabelList() {
        return actionArgumentLabelList;
    }

    public string GetActionArgumentLabel(int index){
        return actionArgumentLabelList[index];
    }
    

}