using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body {

    public Animal thisAnimal;

    public GameObject humanPrefab;
    public Rigidbody rigidbody;

    public string labelLH;
    public string labelRH;

    protected int numSkeletons;
    protected Dictionary<string, GameObject> skeletonDict;

    protected int numBodyStates;
    protected Dictionary<string, bool> bodyStateDict;

    public Body(Animal animal) {
        this.thisAnimal = animal;
    }

    public void InitBodyStates(List<string> passedBodyStateLabelList){
        bodyStateDict = new Dictionary<string, bool>();
        if (passedBodyStateLabelList != null){
            for (int i = 0; i < passedBodyStateLabelList.Count; i++) {
                bodyStateDict[passedBodyStateLabelList[i]] = false;
            }
        }
        else{
            Debug.Log("No body states defined for this animal");
        }
    }

    public virtual List<string> GetBodyStateLabelList() {
        return null;
    }

    public virtual void UpdateBodyStates() {
        Debug.Log("No update body states defined for this animal");
    }

    public virtual void UpdateSkeletonStates()
    {
        Debug.Log("No update skeleton states defined for this animal");
    }

    // getters for body state data structures
    public int GetNumBodyStates() {
        return numBodyStates;
    }

    public Dictionary<string, bool> GetBodyStateDict() {
        return bodyStateDict;
    }

    public int GetNumSkeleton() {
        return skeletonDict.Count;
    }

    public Dictionary<string, GameObject> GetSkeletonDict() {
        return skeletonDict;
    }

    public void SetBodyState(string label, bool passed) {
        bodyStateDict[label] = passed;
    }
}
