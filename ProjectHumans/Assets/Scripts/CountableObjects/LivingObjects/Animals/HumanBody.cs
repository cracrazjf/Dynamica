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
        skeletonDict = new Dictionary <string, GameObject>();
        JointDict = new Dictionary<string, ConfigurableJoint>();
        List<Transform> humanParts = new List<Transform>();
        foreach (Transform child in this.thisHuman.gameObject.transform)
        {
            foreach(Transform grandChild in child)
            {
                skeletonDict.Add(grandChild.name, grandChild.gameObject);
                if (grandChild.TryGetComponent(out ConfigurableJoint configurable))
                {
                    JointDict.Add(grandChild.name, configurable);
                }
            }
            
        }
        foreach (KeyValuePair<string, ConfigurableJoint> x in JointDict)
        {
            Debug.Log(x);
        }
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
            Debug.Log(isSitting());
        }
        if (isLaying()) {
            bodyStateDict["laying"] = true;
        }
    }
    
    bool isSitting() {
        GameObject Body = skeletonDict["Body"];
        float bodyExtent = Body.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(Body.transform.position, -Body.transform.up, bodyExtent + 0.2f);
    }

    bool isLaying() {
        GameObject Body = skeletonDict["Body"];
        float bodyExtent = Body.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(Body.transform.position, -Vector3.up, bodyExtent + 0.2f);
    }

    bool isStanding() {
        GameObject leftFoot = skeletonDict["Foot_L"];
        GameObject rightFoot = skeletonDict["Foot_R"];
        return (Physics.Raycast(leftFoot.transform.position, -Vector3.up, 0.2f) && Physics.Raycast(rightFoot.transform.position, -Vector3.up, 0.2f));
    }
}