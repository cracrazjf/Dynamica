using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBody : Body {

    public Human thisHuman;
    public float abdomenLength;

    List<string> bodyStateLabelList;
    public string objectTypeInLH = "None";
    public string objectTypeInRH = "None";


    public HumanBody(Human human) : base(human) {
        
        this.thisHuman = human;

        if (this.thisHuman.phenotype.traitDict["sex"] == 0)
        {   
            humanPrefab = Resources.Load("Prefabs/HumanPrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, thisHuman.startPosition, thisHuman.startRotation) as GameObject;
            this.thisHuman.gameObject.name = this.thisHuman.GetName();

             this.thisHuman.gameObject.SetActive(true);
        } 
        else {
            humanPrefab = Resources.Load("Prefabs/HumanPrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, thisHuman.startPosition, thisHuman.startRotation) as GameObject;
            this.thisHuman.gameObject.name = this.thisHuman.GetName();

            this.thisHuman.gameObject.SetActive(true);
        }

        rigidbody = this.thisHuman.gameObject.GetComponent<Rigidbody>();


        bodyStateLabelList = new List<string>
        {
            "standing", 
            "sitting", 
            "laying",
            "holding with left hand",
            "holding with right hand",
            "sleeping"
        };

        foreach(GameObject skeleton in GameObject.FindGameObjectsWithTag("Skeleton")) {
            skeletonDict[skeleton.name] = skeleton;
        }
        abdomenLength = skeletonDict["Abdomen"].transform.localScale.y;
    }

    public override List<string> GetBodyStateLabelList() {
        return bodyStateLabelList;
    }

    public override void UpdateBodyStates() 
    {
        if(isStanding()) {
            bodyStateDict["standing"] = true;
        }
        if(isSitting()) {
            bodyStateDict["sitting"] = true;
        }
        if (isLaying()) {
            bodyStateDict["laying"] = true;
        }
    }
    
    bool isSitting() {
        GameObject abdomen = skeletonDict["Abdomen"];
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(this.thisHuman.gameObject.transform.position, -this.thisHuman.gameObject.transform.up, abdomenLength + 0.45f);
    }

    bool isLaying() {
        GameObject abdomen = skeletonDict["Abdomen"];
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(this.thisHuman.gameObject.transform.position, -Vector3.up, bodyExtent + 0.2f);
    }

    bool isStanding() {
        GameObject leftFoot = skeletonDict["Foot_L"];
        GameObject rightFoot = skeletonDict["Foot_R"];
        return (Physics.Raycast(leftFoot.transform.position, -Vector3.up, 0.2f) && Physics.Raycast(rightFoot.transform.position, -Vector3.up, 0.2f));
    }
}