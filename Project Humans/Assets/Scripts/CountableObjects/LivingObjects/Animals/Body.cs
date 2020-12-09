using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body {

    public Animal thisAnimal;

    public GameObject humanPrefab;
    public Rigidbody rigidbody;

    private int numBodyStates;
    private bool[] bodyStateArray;
    private List<string> bodyStateLabelList;
    private Dictionary<string, int> bodyStateIndexDict;

    public Body(Animal animal) {
        this.thisAnimal = animal;
    }

    public void InitBodyStates(){

        if (bodyStateLabelList.Count() > 0){
            this.bodyStateLabelList = passedBodyStateLabelList;
    
            bodyStateIndexDict = new Dictionary <string, int>();

            int i;
            for (i=0; i<bodyStateLabelList.Count; i++){
                bodyStateIndexDict.Add(bodyStateLabelList[i], i);
            }
            numBodyStates = i;
            bodyStateArray = new bool[numBodyStates];
        }
        else{
            Debug.Log("No body states defined for this animal");
        }
    }

    public virtual void UpdateBodyStates() {
        Debug.Log("No update body states defined for this animal");
    }

    // getters for body state data structures
    public int GetNumBodyStates(){
        return numBodyStates;
    }

    public float[] GetBodyStateLabelList(){
        return bodyStateLabelList;
    }

    public bool[] GetBodyStateArray(){
        return bodyStateArray;
    }

    public Dictionary<string, int> GetBodyStateIndexDict(){
        return bodyStateIndexDict;
    }

    public string getBodyStateLabel(int index){
        return bodyStateLabelList[index];
    }

    public bool getBodyState(int index){
        return bodyStateArray[index];
    }

    public int getBodyStateIndex(string label){
        return bodyStateIndexDict[label];
    }
}