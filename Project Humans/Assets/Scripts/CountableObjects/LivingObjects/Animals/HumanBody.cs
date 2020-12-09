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

    public HumanBody(Human human) : base(human) {
        
        this.thisHuman = human;

        if (this.phenotype.traitDict["sex"] == "0")
        {   
            humanPrefab = Resources.Load("HumanMalePrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, startPosition, startRotation) as GameObject;
            this.thisHuman.gameObject.name = GetName();

            gameObject.SetActive(true);
        } else {
            humanPrefab = Resources.Load("HumanFemalePrefab",typeof(GameObject)) as GameObject;
            this.thisHuman.gameObject = GameObject.Instantiate(humanPrefab, startPosition, startRotation) as GameObject;
            this.thisHuman.gameObject.name = GetName();

            this.thisHuman.gameObject.SetActive(true);

            bodyStateLabelList = new List<string>
            {
                "standing", 
                "sitting", 
                "laying",
                "holding with left hand",
                "holding with right hand",
                "sleeping"
            };
        }

        this.thisHuman.gameObject.AddComponent<HumanMonobehaviour>();
        animator = this.thisHuman.gameObject.GetComponent<Animator>();

        leftEye = this.thisHuman.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2);
        rightEye = this.thisHuman.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(3);

        leftHand = this.thisHuman.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(5);
        rightHand = this.thisHuman.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5);

        rigidbody = this.thisHuman.gameObject.GetComponent<Rigidbody>();
    }
    
    public override void UpdateBodyStates() {
  
        this.SetBodyStateArray(new bool[this.GetNumBodyStates()]);
        
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

        // checked to see if the hands had stuff in them
    }

}