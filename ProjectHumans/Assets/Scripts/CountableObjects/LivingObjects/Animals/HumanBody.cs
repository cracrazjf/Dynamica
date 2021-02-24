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

        limbDict = new Dictionary <string, GameObject>();
        skeletonDict = new Dictionary <string, GameObject>();
        jointDict = new Dictionary<string, ConfigurableJoint>();
        List<Transform> humanParts = new List<Transform>();

        foreach (Transform child in this.thisHuman.gameObject.transform) {
            limbDict.Add(child.name, child.gameObject);
            foreach(Transform grandChild in child) {
                skeletonDict.Add(grandChild.name, grandChild.gameObject);
                if (grandChild.TryGetComponent(out ConfigurableJoint configurable)) {
                    jointDict.Add(grandChild.name, configurable);
                }
            }  
        }
        this.abdomen = skeletonDict["Abdomen"];
    }

    public override void UpdateBodyStates() {
        SetState("sitting", CheckSitting());
        SetState("laying", CheckSitting());
    }
    
    bool CheckSitting() {
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(abdomen.transform.position, -abdomen.transform.up, bodyExtent + 0.2f);
    }

    bool CheckLaying() {
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(abdomen.transform.position, -Vector3.up, bodyExtent + 0.2f);
    }
}