using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanMotorSystem : MotorSystem
{
    private Human thisHuman;
    float max_rotation_speed = 1;
    float maxStepDistance = 2;
    List<string> actionStateLabelList;
    List<string> actionArgumentLabelList;

    protected int numActionStates;
    protected Dictionary<string, bool> actionStateDict;

    protected int numActionArguments;
    protected Dictionary<string, float> actionArgumentDict;

    public bool rotating = false;

    public float velocity = 0;

    public HumanMotorSystem(Human human) : base(human) {
        this.thisHuman = human;
        actionStateLabelList = new List<string>
        {
            "sitting down", 
            "sitting up", 
            "laying down",
            "standing up", 
            "rotating", 
            "taking steps",
            "picking up", 
            "setting down",
            "eating", 
            "drinking",
            "waking up",
            "falling asleep"
        };

        actionArgumentLabelList = new List<string>
        {        
            "movement velocity",
            "step rate",                          
            "rotation velocity",               
            "hand",
            "hand target x",
            "hand target y",
            "hand target z"
        };
    }

    public override void InitActionStates() {
        actionStateDict = new Dictionary<string, bool>();
        foreach (string item in actionStateLabelList) {
            actionStateDict[item] = false;
        }
    }

    public override void InitActionArguments() {
        actionArgumentDict = new Dictionary<string, float>();
        foreach (string item in actionArgumentLabelList) {
            actionArgumentDict[item] = 0.0f;
        }
    }


    public override void InitActionRuleDicts(){

        /* bodyStateRequirementDict["sitting down"].Add("standing");
        bodyStateObstructorDict["sitting down"].Add("sleeping");
        
        bodyStateRequirementDict["sitting up"].Add("laying");
        bodyStateObstructorDict["sitting up"].Add("sleeping");

        bodyStateRequirementDict["laying down"].Add("sitting");
        bodyStateObstructorDict["laying down"].Add("sleeping");
        
        bodyStateRequirementDict["standing up"].Add("sitting");
        bodyStateObstructorDict["standing up"].Add("sleeping");


        bodyStateRequirementDict["taking steps"].Add("standing");
        bodyStateObstructorDict["taking steps"].Add("sleeping");

        bodyStateRequirementDict["picking up"].Add("standing");
        bodyStateObstructorDict["picking up"].Add("sleeping");

        bodyStateRequirementDict["setting down"].Add("standing");
        bodyStateObstructorDict["setting down"].Add("sleeping");

        bodyStateRequirementDict["eating"].Add("standing");
        bodyStateObstructorDict["eating"].Add("sleeping");

        bodyStateRequirementDict["drinking"].Add("standing");
        bodyStateObstructorDict["drinking"].Add("sleeping");

        bodyStateRequirementDict["waking up"].Add("laying");
        bodyStateRequirementDict["waking up"].Add("sleeping");

        bodyStateRequirementDict["falling asleep"].Add("laying");
        bodyStateObstructorDict["falling asleep"].Add("sleeping"); */
    }

    public override void UpdateActionStates(){

        if (rotating) {
             this.actionStateDict["rotating"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting down")) {
            this.actionStateDict["sitting down"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting up")) {
            this.actionStateDict["sitting up"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Laying down")) {
             this.actionStateDict["laying down"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Taking steps")) {
            this.actionStateDict["taking steps"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Drinking")) {
            this.actionStateDict["drinking"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Eating")) {
                this.actionStateDict["eating"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Picking Up")) {
                this.actionStateDict["picking up"] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("SettingDown")) {
                this.actionStateDict["setting down"] = true;
        }
    }

    public override void EndAction(string actionLabel) {
        this.actionStateDict[actionLabel] = false;
    }

    public void TakeSteps(float stepProportion) {
        if (stepProportion != 0) {
            Vector3 temp = new Vector3(0, 0, stepProportion);
            thisHuman.gameObject.transform.position += temp;
            this.actionStateDict["taking steps"] = true;
        }
    }

    public void Rotate(float rotatingSpeed) {
        if (rotatingSpeed != 0) {
            this.thisHuman.gameObject.transform.Rotate(0,rotatingSpeed,0 , Space.World);
            rotating = true;
        }
    }

    public void Drink() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);
        
        this.thisHuman.GetDriveSystem().SetDriveState("thirst", 0);
        this.actionStateDict["drinking"] = true;
        
    }
    
    public void SitDown(){
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.actionStateDict["sitting down"] = true;

        this.thisHuman.GetBody().SetBodyState("standing", false);
        this.thisHuman.GetBody().SetBodyState("sitting", true);                     
    }

    public void SitUp(){
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.actionStateDict["sitting up"] = true;

        this.thisHuman.GetBody().SetBodyState("laying", false);
        this.thisHuman.GetBody().SetBodyState("sitting", true);          
    }
        
    public void StandUp(){
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.actionStateDict["standing up"] = true;

        this.thisHuman.GetBody().SetBodyState("sitting", false);
        this.thisHuman.GetBody().SetBodyState("standing", true);  
       
    }

    public void LayDown(){
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.actionStateDict["laying down"] = true;

        this.thisHuman.GetBody().SetBodyState("sitting", false);
        this.thisHuman.GetBody().SetBodyState("laying", true);  
    }

    public void Sleep(){
        //bool doingNothing = !this.actionStateArray.Any(x => x);
        Debug.Log("sleep");

        this.actionStateDict["falling asleep"] = true;
        this.thisHuman.GetBody().SetBodyState("sleeping", true);  
    }
    
    public void WakeUp(){
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.actionStateDict["waking up"] = true;
        this.thisHuman.GetBody().SetBodyState("sleeping", false);  
    }
    
    public void PickUp(float hand) {
        Collider[] pickableObj = new Collider[5];
        //bool doingNothing = !this.actionStateArray.Any(x => x);
            
        int pickUpHand = -1;
        int numObj = -1;
        if ((hand == 0) && (!this.thisHuman.GetBody().GetBodyStateDict()["holding with left hand"])) {
            pickUpHand = 0;
            //numObj = Physics.OverlapSphereNonAlloc(((HumanBody)this.thisHuman.GetBody()).leftHand.position,0.2f,pickableObj);

        } else if ((hand == 1) && (!this.thisHuman.GetBody().GetBodyStateDict()["holding with right hand"])){
            pickUpHand = 1;
            //numObj = Physics.OverlapSphereNonAlloc(((HumanBody)this.thisHuman.GetBody()).rightHand.position,0.2f,pickableObj);
        }
        if (pickUpHand != -1) {
            
            this.thisHuman.animator.SetFloat("left/rightHand",pickUpHand);
            this.actionStateDict["picking up"] = true;

            for (int i = 0; i < numObj; i++) {
                if (!pickableObj[i].CompareTag("Human") && !pickableObj[i].CompareTag("ground")) {
                    pickableObj[i].GetComponent<Rigidbody>().isKinematic = true;
                    pickableObj[i].GetComponent<Rigidbody>().useGravity = false;
                    
                    if (pickUpHand == 0){
                        this.thisHuman.GetBody().SetBodyState("holding with left hand", true);  
                    } else {
                        this.thisHuman.GetBody().SetBodyState("holding with right hand", true);
                    }
                }
            }
        } else {
            Debug.Log("can't pick up because another action is happening");
        }
    }

    public void SetDown(float hand) {}

    public void Eat(float hand) {}
}

