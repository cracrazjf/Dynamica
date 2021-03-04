using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBody : Body {

    protected Animal thisAnimal;
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

    protected List<Vector3> holderCoords;
    protected List<GameObject> holdings;
    public List<GameObject> GetHoldings() { return holdings; }

    public AnimalBody(Animal animal, Transform position) : base((Entity) animal, position) {
        stateLabelList = new List<string> {
            "standing", 
            "sitting", 
            "laying",
        };
        InitStates(stateLabelList);
    }

    public void InitHolders() {
        holdings = new List<GameObject>();
        holderCoords = new List<Vector3>();
    }

    public void InitBodyDicts() {
        limbDict = new Dictionary <string, GameObject>();
        skeletonDict = new Dictionary <string, GameObject>();
        jointDict = new Dictionary<string, ConfigurableJoint>();

        foreach (Transform child in globalPos) {
            limbDict.Add(child.name, child.gameObject);
            foreach(Transform grandChild in child) {
                skeletonDict.Add(grandChild.name, grandChild.gameObject);
                if (grandChild.TryGetComponent(out ConfigurableJoint configurable)) {
                    jointDict.Add(grandChild.name, configurable);
                }
            }  
        }
        abdomen = skeletonDict["Abdomen"];
        head = skeletonDict["Head"];
        eyeLevel = head.transform.position.y;
    }

    // Initializes state information but also calls standard height and holder info
    public void InitStates(List<string> passedList) {
        states = new bool[passedList.Count];
        stateLabelList = passedList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, bool>();

        if (passedList != null){
            for (int i = 0; i < passedList.Count; i++) {
                states[i] = false;
                stateIndexDict[passedList[i]] = i;
                stateDict[passedList[i]] = false;
            }
        } else { Debug.Log("No body states passed to this animal"); }

        InitHolders();
    }

    public void SetState(string label, bool passed) {
        stateDict[label] = passed;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = passed;
    }

    public virtual void UpdateBodyStates() { Debug.Log("No update body states defined for this animal"); }

    public virtual void UpdateSkeletonStates() { Debug.Log("No update skeleton states defined for this animal"); }

    public void RotateJoint(string joint, Quaternion target) {
        if (this.jointDict.ContainsKey(joint)) {
            this.jointDict[joint].targetRotation = target;
        }
    }

    // Unused at the moment
    public void TranslateSkeletonBy(string name, Vector3 vector) {
        if (skeletonDict.ContainsKey(name)) {
            GameObject currentPart = skeletonDict[name];
            Vector3 currentPos = currentPart.transform.position;
            currentPart.GetComponent<Rigidbody>().isKinematic = true;
            Vector3 translatedPos = currentPos + vector;

            currentPos = Vector3.MoveTowards(currentPos, translatedPos, 1.0f * Time.deltaTime);
        }
    }

    public void TranslateSkeletonTo(string name, Vector3 goalPos) {
        Debug.Log("Tried to move");
        if (skeletonDict.ContainsKey(name)) {
            GameObject currentPart = skeletonDict[name];
            Vector3 currentPos = currentPart.transform.position;
            currentPart.GetComponent<Rigidbody>().isKinematic = true;

            currentPos = Vector3.MoveTowards(currentPos, goalPos, 1.0f * Time.deltaTime);
        }
    }

    public Vector3 GetHolderCoords(float index) {
        if (holderCoords.Count > index) { return holderCoords[(int) index]; }

        Debug.Log("Not a valid held item position");
        return new Vector3(0, 0, 0);
    }
    
}
