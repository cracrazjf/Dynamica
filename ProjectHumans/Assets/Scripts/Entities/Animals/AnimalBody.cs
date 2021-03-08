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

    protected List<GameObject> holders;
    protected List<GameObject> holdings;
    public List<GameObject> GetHoldings() { return holdings; }
    public GameObject GetHolder(int i) { return holders[i]; }

    public AnimalBody(Animal animal, Vector3 position) : base((Entity) animal, position) {
        stateLabelList = new List<string> {
            "standing", 
            "sitting", 
            "laying",
            "alive"
        };
        InitStates(stateLabelList);
        InitBodyDicts();
        InitHolders();
    }

    public virtual void InitHolders() {
        holdings = new List<GameObject>();
        holders = new List<GameObject>();
    }

    public void InitBodyDicts() {
        limbDict = new Dictionary <string, GameObject>();
        skeletonDict = new Dictionary <string, GameObject>();
        jointDict = new Dictionary<string, ConfigurableJoint>();

        foreach (Transform child in globalPos) {
            if(child.name == "Body") {
                globalPos = child;
            }
        }

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

    public Vector3 GetHolderCoords(float passedIndex) {
        int index = (int) passedIndex;
        if (holders.Count > index) { return holders[(int) index].transform.position; }

        Debug.Log("Not a valid held item position");
        return new Vector3(0, 0, 0);
    }

    public void AddHoldings(GameObject toAdd, int heldIndex) { 
        holdings[heldIndex] = toAdd;
    }

    public void ToggleKinematic(string name) {
        if (name != null) {
            if (skeletonDict.ContainsKey(name)) {
                GameObject currentPart = skeletonDict[name];
                currentPart.GetComponent<Rigidbody>().isKinematic = !(currentPart.GetComponent<Rigidbody>().isKinematic);
            }
        }
    }

    public void EatObject(int heldIndex) {
        GameObject toEat = holdings[heldIndex];
        World.RemoveEntity(toEat.name);
        //eating stuff
        holdings.RemoveAt(heldIndex);
    }

    public void RemoveObject(int heldIndex) {
        World.DestroyComponent(holdings[heldIndex].GetComponent<FixedJoint>());
        holdings.RemoveAt(heldIndex);
    }
}
