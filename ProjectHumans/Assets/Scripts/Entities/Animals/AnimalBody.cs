using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBody : Body {

    protected Animal thisAnimal;
    public GameObject abdomen;
    public GameObject head;
    public GameObject mouth;

    public float xMax, yMax, zMax;

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
        PlaceBody(position);
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
        //mouth = skeletonDict["Mouth"];
    }

    public override void InitGameObject(Vector3 pos) {
        thisAnimal = (Animal) thisEntity;

        string bodyName = thisAnimal.GetSpecies() + thisAnimal.GetSex();
        string filePath = "Prefabs/" + bodyName + "Prefab";
        GameObject loadedPrefab = Resources.Load(filePath, typeof(GameObject)) as GameObject;
        
        this.gameObject = (GameObject.Instantiate(loadedPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject);
        this.gameObject.name = thisEntity.GetName();

        rigidbody = GetGameObject().GetComponent<Rigidbody>();
        globalPos = this.gameObject.transform;
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


    public void PlaceBody(Vector3 position) {
        this.globalPos.position = position;
        this.gameObject.SetActive(true);
    }

    public void SetState(string label, bool passed) {
        stateDict[label] = passed;
        int currentIndex = stateIndexDict[label];
        states[currentIndex] = passed;
    }

    public virtual void UpdateBodyStates() { Debug.Log("No update body states defined for this animal"); }

    public virtual void UpdateSkeletonStates() { Debug.Log("No update skeleton states defined for this animal"); }


    // do not use
    public void RotateJointTo(string joint, Quaternion target) {
        if (this.jointDict.ContainsKey(joint)) {
            Quaternion toMultiply = skeletonDict[joint].transform.localRotation;
            toMultiply = toMultiply * target;
            this.jointDict[joint].targetRotation = this.jointDict[joint].targetRotation * toMultiply;
        }
    }

    public void RotateJointBy(string joint, Quaternion target) {
        if (this.jointDict.ContainsKey(joint)) {
            this.jointDict[joint].targetRotation = Quaternion.identity * (this.jointDict[joint].targetRotation * Quaternion.Inverse(target));
        }
    }

    public void RotateSkeletonTo(string name, Quaternion target) {
        if (this.skeletonDict.ContainsKey(name)) {
            this.skeletonDict[name].transform.localRotation = target;
        }
    }

    public void RotateLimbBy(string name, Quaternion target) {
        Debug.Log("Tried limb rotation");
        if (this.limbDict.ContainsKey(name)) {
            this.limbDict[name].transform.localRotation = Quaternion.identity * Quaternion.Inverse(target);
        }
    }

    public void TranslateSkeletonTo(string name, Vector3 goalPos) {
        //Debug.Log("Tried to translate " + name + " to " + goalPos);
        if (skeletonDict.ContainsKey(name)) {
            GameObject currentPart = skeletonDict[name];
            Vector3 currentPos = currentPart.transform.position;
            bool kinematicTemp = currentPart.GetComponent<Rigidbody>().isKinematic;

            if (Math.Pow(currentPos.y - goalPos.y, 2) < 0.005 ) { // something funky\
                Debug.Log("Reached goal position");
                currentPart.GetComponent<Rigidbody>().isKinematic = kinematicTemp;
            } else {
                currentPart.GetComponent<Rigidbody>().useGravity = false;
                currentPart.GetComponent<Rigidbody>().isKinematic = true;
                currentPos = Vector3.MoveTowards(currentPos, goalPos, 10f * Time.deltaTime);
                //Debug.Log("Tried to translate " + name + " to " + goalPos);
                currentPart.GetComponent<Rigidbody>().isKinematic = kinematicTemp;
                currentPart.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    public void PrintSkelPos(string name) {
        if (skeletonDict.ContainsKey(name)) {
            GameObject currentPart = skeletonDict[name];
            Vector3 currentPos = currentPart.transform.position;

            Debug.Log("Current position of " + name + " is " + currentPos);
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

    public void DisableKinematic(string name) {
        if (name != null) {
            if (skeletonDict.ContainsKey(name)) {
                GameObject currentPart = skeletonDict[name];
                currentPart.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    public void DisableGravity(string name) {
        if (name != null) {
            if (skeletonDict.ContainsKey(name)) {
                GameObject currentPart = skeletonDict[name];
                currentPart.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }

    public void EnsureKinematic(string name) {
        if (name != null) {
            if (skeletonDict.ContainsKey(name)) {
                GameObject currentPart = skeletonDict[name];
                currentPart.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public void EnsureGravity(string name) {
        if (name != null) {
            if (skeletonDict.ContainsKey(name)) {
                GameObject currentPart = skeletonDict[name];
                currentPart.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    public virtual void SleepAdjust() {
        float val = thisAnimal.GetDriveSystem().GetState("sleepiness");
        val += (thisAnimal.GetPhenotype().GetTrait("sleepiness_change") * 20);
        thisAnimal.GetDriveSystem().SetState("sleepiness", val);

        Debug.Log("Snoozed a bit!");
    }

    public virtual void RestAdjust() {
        float val = thisAnimal.GetDriveSystem().GetState("fatigue");
        val += (thisAnimal.GetPhenotype().GetTrait("fatigue_change") * 20);
        thisAnimal.GetDriveSystem().SetState("fatigue", val);

        Debug.Log("Rested a bit!");
    }

    public virtual void EatObject(int heldIndex) {
        GameObject toEat = holdings[heldIndex];
        World.RemoveEntity(toEat.name);
        //eating stuff
        holdings.RemoveAt(heldIndex);
    }

    public virtual void RemoveObject(int heldIndex) {
        World.DestroyComponent(holdings[heldIndex].GetComponent<FixedJoint>());
        holdings.RemoveAt(heldIndex);
    }

    public bool CheckBend(string joint, float toCheck) {
        if (this.jointDict.ContainsKey(joint)) {
            Quaternion toMultiply = skeletonDict[joint].transform.localRotation;
            Quaternion toSee = this.jointDict[joint].targetRotation * toMultiply;
            Debug.Log("Current bend is " + toSee);
            if (toCheck > toSee.x) {
                return false;
            }
            return true;
        }
        return false;
    }
}
