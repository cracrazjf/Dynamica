using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBody : Body {

    public Human thisHuman;
    public float abdomenLength;

    public string objectTypeInLH = "None";
    public string objectTypeInRH = "None";
    
    Dictionary<string, GameObject> skeletonDict;

    protected bool[] states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, bool> stateDict;

    public HumanBody(Human human) : base(human) {
        
        this.thisHuman = human;

        // Currently humans have no difference in model, but here we discriminate loaded model based on sex
        if (this.thisHuman.phenotype.GetTraitDict()["sex"] == 0) {   
            humanPrefab = Resources.Load("Prefabs/HumanPrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, thisHuman.startPosition, thisHuman.startRotation) as GameObject;
            this.thisHuman.gameObject.name = this.thisHuman.GetName();

             this.thisHuman.gameObject.SetActive(true);
        } else {
            humanPrefab = Resources.Load("Prefabs/HumanPrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, thisHuman.startPosition, thisHuman.startRotation) as GameObject;
            this.thisHuman.gameObject.name = this.thisHuman.GetName();

            this.thisHuman.gameObject.SetActive(true);
        }

        rigidbody = this.thisHuman.gameObject.GetComponent<Rigidbody>();

        this.stateLabelList = new List<string> {
            // Originally bodyStates, will be read-in eventually

            "standing", 
            "sitting", 
            "laying",
            "holding with left hand",
            "holding with right hand",
            "sleeping"
        };

        Transform tagFinder = this.thisHuman.gameObject.GetComponent<Transform>();
        skeletonDict = new Dictionary <string, GameObject>();
        foreach(Transform unit in tagFinder) {
            foreach(Transform skeleton in unit) {
                if (skeleton.tag == "Skeleton") {
                    skeletonDict[skeleton.name] = skeleton.gameObject;
                }  
            }
        }
        abdomenLength = skeletonDict["Abdomen"].transform.localScale.y;
    }


    public override void UpdateBodyStates() {
        CheckSitting();
        CheckLaying();
        CheckStanding();
    }
    
    void CheckSitting() {
        GameObject abdomen = skeletonDict["Abdomen"];
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        bool toUpdate = Physics.Raycast(this.thisHuman.gameObject.transform.position, -this.thisHuman.gameObject.transform.up, abdomenLength + 0.45f);

        this.SetState("sitting", toUpdate);
    }

    void CheckLaying() {
        GameObject abdomen = skeletonDict["Abdomen"];
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        bool toUpdate = Physics.Raycast(this.thisHuman.gameObject.transform.position, -Vector3.up, bodyExtent + 0.2f);

        this.SetState("laying", toUpdate);
    }

    void CheckStanding() {
        GameObject leftFoot = skeletonDict["Foot_L"];
        GameObject rightFoot = skeletonDict["Foot_R"];
        bool toUpdate =  (Physics.Raycast(leftFoot.transform.position, -Vector3.up, 0.2f) && Physics.Raycast(rightFoot.transform.position, -Vector3.up, 0.2f));

        this.SetState("standing", toUpdate);
    }
}