using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body {

    public Animal thisAnimal;

    public GameObject humanPrefab;
    public Rigidbody rigidbody;

    protected int numSkeletons;
    protected bool[] skeletonStateArray;
    protected List<GameObject> skeletonList;
    protected Dictionary<string, int> skeletonIndexDict;

    protected int numBodyStates;
    protected bool[] bodyStateArray;
    protected List<string> bodyStateLabelList;
    protected Dictionary<string, int> bodyStateIndexDict;

    public Body(Animal animal) {
        this.thisAnimal = animal;
    }

    public void InitBodyStates(List<string> passedBodyStateLabelList){
        if (passedBodyStateLabelList != null){
            bodyStateIndexDict = new Dictionary <string, int>();

            int i;
            for (i=0; i<passedBodyStateLabelList.Count; i++){
                bodyStateIndexDict.Add(passedBodyStateLabelList[i], i);
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

    public virtual void UpdateSkeletonStates()
    {
        Debug.Log("No update skeleton states defined for this animal");
    }
    public void SetBodyState(int index, bool value) {
        bodyStateArray[index] = value;
    }

    // getters for body state data structures
    public int GetNumBodyStates(){
        return numBodyStates;
    }

    public List<string> GetBodyStateLabelList(){
        return bodyStateLabelList;
    }

    public bool[] GetBodyStateArray(){
        return bodyStateArray;
    }

    public Dictionary<string, int> GetBodyStateIndexDict(){
        return bodyStateIndexDict;
    }

    public string GetBodyStateLabel(int index){
        return bodyStateLabelList[index];
    }

    public bool GetBodyState(int index){
        return bodyStateArray[index];
    }

    public int GetBodyStateIndex(string label){
        return bodyStateIndexDict[label];
    }

    public int GetNumSkeleton()
    {
        return numSkeletons;
    }

    public List<GameObject> GetSkeletonList()
    {
        return skeletonList;
    }

    public bool[] GetSkeletonStateArray()
    {
        return skeletonStateArray;
    }

    public Dictionary<string, int> GetSkeletonIndexDict()
    {
        return skeletonIndexDict;
    }

    public GameObject GetSkeletonGameobject(int index)
    {
        return skeletonList[index];
    }

    public bool GetSkeletonState(int index)
    {
        return skeletonStateArray[index];
    }

    public int GetSkeletonIndex(string label)
    {
        return skeletonIndexDict[label];
    }
}