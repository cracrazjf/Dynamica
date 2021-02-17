using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBody : Body {

    public Human thisHuman;
    public float abdomenLength;

    public HumanBody(Human human) : base(human) {
        
        this.thisHuman = human;
        Vector3 fixDrop = new Vector3(0, 2f, 0);

        if (this.thisHuman.phenotype.GetTraitDict()["sex"] == 0) {   
            humanPrefab = Resources.Load("Prefabs/HumanPrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, thisHuman.startPosition + fixDrop, thisHuman.startRotation) as GameObject;
            this.thisHuman.gameObject.name = this.thisHuman.GetName();

             this.thisHuman.gameObject.SetActive(true);
        } else {
            humanPrefab = Resources.Load("Prefabs/HumanPrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, thisHuman.startPosition + fixDrop, thisHuman.startRotation) as GameObject;
            this.thisHuman.gameObject.name = this.thisHuman.GetName();

            this.thisHuman.gameObject.SetActive(true);
        }

        rigidbody = this.thisHuman.gameObject.GetComponent<Rigidbody>();


        stateLabelList = new List<string> {
            "standing", 
            "sitting", 
            "laying",
            "holding with left hand",
            "holding with right hand",
            "sleeping"
        };
        this.InitStates(stateLabelList);

        skeletonDict = new Dictionary <string, GameObject>();
        jointDict = new Dictionary<string, ConfigurableJoint>();
        List<Transform> humanParts = new List<Transform>();
        foreach (Transform child in this.thisHuman.gameObject.transform)
        {
            foreach(Transform grandChild in child)
            {
                skeletonDict.Add(grandChild.name, grandChild.gameObject);
                if (grandChild.TryGetComponent(out ConfigurableJoint configurable))
                {
                    jointDict.Add(grandChild.name, configurable);
                }
            }
            
        }
        foreach (KeyValuePair<string, ConfigurableJoint> x in jointDict)
        {
            Debug.Log(x);
        }
    }

    public override void UpdateBodyStates() {
        CheckSitting();
        CheckLaying();
    }
    
    void CheckSitting() {
        GameObject Body = skeletonDict["Body"];
        float bodyExtent = Body.GetComponent<Collider>().bounds.extents.y;
        bool toUpdate =  Physics.Raycast(Body.transform.position, -Body.transform.up, bodyExtent + 0.2f);

        this.SetState("sitting", toUpdate);
    }

    void CheckLaying() {
        GameObject Body = skeletonDict["Body"];
        float bodyExtent = Body.GetComponent<Collider>().bounds.extents.y;
        bool toUpdate = Physics.Raycast(Body.transform.position, -Vector3.up, bodyExtent + 0.2f);

        this.SetState("laying", toUpdate);
    }
}