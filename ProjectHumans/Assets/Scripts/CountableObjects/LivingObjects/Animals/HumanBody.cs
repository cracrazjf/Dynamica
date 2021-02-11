using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBody : Body {

    public Human thisHuman;
    public float abdomenLenth;

    public string objectTypeInLH = "None";
    public string objectTypeInRH = "None";

    public HumanBody(Human human) : base(human) {
        
        this.thisHuman = human;

        if (this.thisHuman.phenotype.traitDict["sex"] == 0)
        {   
            humanPrefab = Resources.Load("Prefabs/HumanMalePrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, thisHuman.startPosition, thisHuman.startRotation) as GameObject;
            this.thisHuman.gameObject.name = this.thisHuman.GetName();

             this.thisHuman.gameObject.SetActive(true);
        } 
        else {
            humanPrefab = Resources.Load("Prefabs/HumanFemalePrefab",typeof(GameObject)) as GameObject;
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

        skeletonList = new List<GameObject>();
        foreach(GameObject skeleton in GameObject.FindGameObjectsWithTag("Skeleton"))
        {
            skeletonList.Add(skeleton);
            skeletonIndexDict.Add(skeleton.name, numSkeletons);
            numSkeletons++;
        }



    }

    public override void UpdateBodyStates() 
    {
        bodyStateArray = new bool[numBodyStates];
        if(isStanding())
        {
            bodyStateArray[bodyStateIndexDict["standing"]] = true;
        }
        if(isSitting())
        {
            bodyStateArray[bodyStateIndexDict["sitting"]] = true;
        }
        if (isLaying())
        {
            bodyStateArray[bodyStateIndexDict["laying"]] = true;
        }
    }
    
    bool isSitting()
    {
        GameObject abdomen = skeletonList[skeletonIndexDict["abdomen"]];
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(this.thisHuman.gameObject.transform.position, -this.thisHuman.gameObject.transform.up, abdomenLenth + 0.45f);
    }

    bool isLaying()
    {
        GameObject abdomen = skeletonList[skeletonIndexDict["abdomen"]];
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(this.thisHuman.gameObject.transform.position, -Vector3.up, bodyExtent + 0.2f);
    }
    bool isStanding()
    {
        GameObject leftFoot = skeletonList[skeletonIndexDict["leftfoot"]];
        GameObject rightFoot = skeletonList[skeletonIndexDict["rightfoot"]];
        if(Physics.Raycast(leftFoot.transform.position, -Vector3.up, 0.2f)
            && Physics.Raycast(rightFoot.transform.position, -Vector3.up, 0.2f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}