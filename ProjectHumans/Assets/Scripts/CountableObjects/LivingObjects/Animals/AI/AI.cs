using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI 
{
    protected int numBodyStates;
    protected Dictionary<string, int> bodyStateIndexDict;
    protected List<string> bodyStateLabelList;
    protected bool[] bodyStateArray;
    
    protected int numDriveStates;
    protected Dictionary<string, int> driveStateIndexDict;
    protected List<string> driveStateLabelList;
    protected float[] driveStateArray;

    protected int numActionStates;
    protected List<string> actionStateLabelList;
    protected Dictionary<string, int> actionStateIndexDict;
    protected bool[] actionStateArray;
    
    protected int numActionArguments;
    protected Dictionary<string, int> actionArgumentIndexDict;
    protected List<string> actionArgumentLabelList;
    protected float[] actionArgumentArray;

    protected int numTraits;
    protected Dictionary<string, int> traitIndexDict;
    protected List<string> traitLabelList;
    protected Dictionary<string, float> traitDict;

    protected Animal.ActionChoiceStruct actionChoiceStruct;

    bool outputDefinitionError = false;

    public AI(Dictionary<string, int> bodyStateIndexDict,
              Dictionary<string, int> driveStateIndexDict,
              Dictionary<string, int> actionStateIndexDict, 
              Dictionary<string, int> actionArgumentIndexDict,
              Dictionary<string, float> traitDict) 
    {

        this.bodyStateIndexDict = bodyStateIndexDict;
        this.driveStateIndexDict = driveStateIndexDict;
        this.actionStateIndexDict = actionStateIndexDict;
        this.actionArgumentIndexDict = actionArgumentIndexDict;
        this.traitDict = traitDict;

        InitBaseAIDataStructures();
        InitActionChoiceStruct();
    }

    protected void InitBaseAIDataStructures(){
        bodyStateLabelList = new List<string>();
        driveStateLabelList = new List<string>();
        actionStateLabelList = new List<string>();
        actionArgumentLabelList = new List<string>();
        traitLabelList = new List<string>();
        traitIndexDict = new Dictionary<string, int>();


        numBodyStates = bodyStateIndexDict.Count;
        foreach(KeyValuePair<string, int> entry in bodyStateIndexDict){
            bodyStateLabelList.Add(entry.Key);
            bodyStateArray = new bool[numBodyStates];
        }

        numDriveStates = driveStateIndexDict.Count;
        foreach(KeyValuePair<string, int> entry in driveStateIndexDict){
            driveStateLabelList.Add(entry.Key);
            driveStateArray = new float[numDriveStates];
        }

        numActionStates = actionStateIndexDict.Count;
        foreach(KeyValuePair<string, int> entry in actionStateIndexDict){
            actionStateLabelList.Add(entry.Key);
            actionStateArray = new bool[numActionStates];
        }

        numActionArguments = actionArgumentIndexDict.Count;
        foreach(KeyValuePair<string, int> entry in actionArgumentIndexDict){
            actionArgumentLabelList.Add(entry.Key);
            actionArgumentArray = new float[numActionArguments];
        }

        numTraits = traitDict.Count;
        int i = 0;
        foreach(KeyValuePair<string, float> entry in traitDict){
            traitLabelList.Add(entry.Key);
            traitIndexDict.Add(entry.Key, i++);
        }

    }

    public void InitActionChoiceStruct(){
        this.actionChoiceStruct = new Animal.ActionChoiceStruct();
        this.actionChoiceStruct.actionChoiceArray = new bool[numActionStates];
        this.actionChoiceStruct.actionArgumentArray = new float[numActionArguments];
    }

    public virtual Animal.ActionChoiceStruct ChooseAction (float[ , ] visualInput, 
                                                           bool[] bodyStateArray, 
                                                           bool[] actionStateArray, 
                                                           float[] driveStateArray, 
                                                           Dictionary<string, float> traitDict){
        
        if (outputDefinitionError == false){
            Debug.Log("No ChooseAction function defined for this AI");
            outputDefinitionError = true;
        }
        
        
        return actionChoiceStruct;

    }

    


}