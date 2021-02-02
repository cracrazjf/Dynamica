using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBody : Body {

    public Human thisHuman;
    public Transform leftEye;
    public Transform rightEye;

    public Transform leftHand;
    public Transform rightHand;

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
            bodyStateLabelList = new List<string>
            {
                "standing", 
                "sitting", 
                "laying",
                "holding with left hand",
                "holding with right hand",
                "sleeping"
            };

        this.thisHuman.gameObject.AddComponent<HumanMonobehaviour>();
        this.thisHuman.gameObject.GetComponent<HumanMonobehaviour>().SetHuman(this.thisHuman);
        this.thisHuman.animator = this.thisHuman.gameObject.GetComponent<Animator>();

        leftEye = this.thisHuman.gameObject.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Neck_M/Head_M/Eye_L");
        rightEye = this.thisHuman.gameObject.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Neck_M/Head_M/Eye_R");

        leftHand = this.thisHuman.gameObject.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/Elbow_L/Wrist_L/LeftHand");
        rightHand = this.thisHuman.gameObject.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/Elbow_R/Wrist_R/RightHand");

        rigidbody = this.thisHuman.gameObject.GetComponent<Rigidbody>();
    }
    
    public override void UpdateBodyStates() {
        objectTypeInLH = "None";
        objectTypeInRH = "None";
        this.bodyStateArray = (new bool[this.GetNumBodyStates()]);
        
        if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Laying Loop")) {
            this.SetBodyState(this.bodyStateIndexDict["laying"], true);
            this.SetBodyState(this.bodyStateIndexDict["standing"], false);
            this.SetBodyState(this.bodyStateIndexDict["sitting"], false);
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Sit Loop")) {
            this.SetBodyState(this.bodyStateIndexDict["laying"], false);
            this.SetBodyState(this.bodyStateIndexDict["standing"], false);
            this.SetBodyState(this.bodyStateIndexDict["sitting"], true);
        }
        else {
            this.SetBodyState(this.bodyStateIndexDict["laying"], false);
            this.SetBodyState(this.bodyStateIndexDict["standing"], true);
            this.SetBodyState(this.bodyStateIndexDict["sitting"], false);
        }
        if(leftHand.childCount > 0) {
            this.SetBodyState(this.bodyStateIndexDict["holding with left hand"], true);
            objectTypeInLH = leftHand.GetChild(0).tag;
        }
        if(rightHand.childCount > 0) {
            this.SetBodyState(this.bodyStateIndexDict["holding with right hand"], true);
            objectTypeInRH = rightHand.GetChild(0).tag;
        }
    }
}