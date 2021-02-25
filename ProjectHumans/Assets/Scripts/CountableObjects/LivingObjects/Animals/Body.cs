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
    protected GameObject abdomen;
    protected GameObject head;
    protected float eyeLevel;

    protected Dictionary<string, GameObject> limbDict;
    public Dictionary<string, GameObject> GetLimbDict() { return limbDict; }

    protected Dictionary<string, GameObject> skeletonDict;
    public Dictionary<string, GameObject> GetSkeletonDict() { return skeletonDict; }

    protected Dictionary<string, ConfigurableJoint> jointDict;
    public Dictionary<string, ConfigurableJoint> GetJointDict() { return jointDict; }
    
    protected bool[] states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, bool> stateDict;
    
    public bool[] GetStates() { return states; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, bool> GetStateDict() { return stateDict; }


    public Body(Animal animal) {
        this.thisAnimal = animal;
    }

    public void RotateJoint(string joint, Quaternion target) {
        if (this.jointDict.ContainsKey(joint)) {
            this.jointDict[joint].targetRotation = target;
        }
    }

    public void InitStates(List<string> passedStateLabelList) {
        states = new bool[passedStateLabelList.Count]; 
        stateLabelList = passedStateLabelList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, bool>();

        if (passedStateLabelList != null){
            for (int i = 0; i < passedStateLabelList.Count; i++) {
                states[i] = false;
                stateIndexDict[passedStateLabelList[i]] = i;
                stateDict[passedStateLabelList[i]] = false;
            }
        }
        else { Debug.Log("No body states defined for this animal"); }
    }

    public void SetState(string label, bool passed) {
        stateDict[label] = passed;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = passed;
    }

    public virtual void UpdateBodyStates() { Debug.Log("No update body states defined for this animal"); }

    public virtual void UpdateSkeletonStates() { Debug.Log("No update skeleton states defined for this animal"); }

    public void ResolveAltitude() {
        float yTemp = head.transform.position.y;
        // Checks for underground
        if (yTemp < 0) { VerticalBump(yTemp * -1 + eyeLevel); }
    }

    public void VerticalBump(float height) {
        head.transform.position += new Vector3(0, height, 0);
    }
}
