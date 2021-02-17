using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanMotorSystem : MotorSystem
{
    private Human thisHuman;
    List<string> actionStateLabelList;
    List<string> actionArgumentLabelList;
    Transform transform;

    JointDrive drive;
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
        actionChoiceDict = new Dictionary<string, bool>();
        foreach (string item in actionStateLabelList) {
            actionChoiceDict[item] = false;
        }
    }

    public override void InitActionArguments() {
        actionArgumentDict = new Dictionary<string, float>();
        foreach (string item in actionArgumentLabelList) {
            actionArgumentDict[item] = 0.0f;
        }
    }

    public override void TakeAction(AI.ActionChoiceStruct actionChoiceStruct)
    {
        transform = this.thisHuman.gameObject.transform;
        //if (actionChoiceStruct.actionChoiceDict["taking steps"])
        //{
        //    TakeSteps(actionChoiceStruct.actionArgumentDict["step rate"]);
        //}

        SitDown();
        //StandUp();
        TakeSteps(1);
        //LayDown();
    }

    public override void InitActionRuleDicts(){
    }

    public override void UpdateActionStates(){
    }

    float t = 0.0f;
    float x = 0.5f;
    public void TakeSteps(float stepProportion) {
        if (stepProportion != 0)
        {
            this.thisHuman.gameObject.transform.Translate(this.thisHuman.gameObject.transform.forward * stepProportion * Time.deltaTime, Space.World);
        }
        this.thisHuman.GetBody().GetJointDict()["Hip_L"].targetRotation = new Quaternion(Mathf.Lerp(-stepProportion, stepProportion, t), 0, 0, 1);
        this.thisHuman.GetBody().GetJointDict()["Hip_R"].targetRotation = new Quaternion(Mathf.Lerp(stepProportion, -stepProportion, t), 0, 0, 1);

        if (t > 1.0f)
        {
            x = -0.5f;
        }
        else if (t <= 0)
        {
            x = 0.5f;
        }
        t += x * Time.deltaTime;
    }

    public void Rotate(float rotatingSpeed) {
        if (rotatingSpeed != 0) {
            this.thisHuman.gameObject.transform.Rotate(0,rotatingSpeed,0 , Space.World);
            rotating = true;
        }
    }

    public void Drink() {
        
        this.thisHuman.GetDriveSystem().SetDriveState("thirst", 0);
        this.actionChoiceDict["drinking"] = true;
        
    }
    
    public void SitDown(){
        this.actionChoiceDict["sitting down"] = true;
        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;
        if (!this.thisHuman.GetBody().GetBodyStateDict()["sitting"])
        {
            Vector3 dir = ((-bodyTransform.up - bodyTransform.forward) / 2).normalized;
            bodyTransform.Translate(dir * 2f * Time.deltaTime, Space.World);
        }
        this.thisHuman.GetBody().SetBodyState("standing", false);
        this.thisHuman.GetBody().SetBodyState("sitting", true);                     
    }

    public void SitUp(){

        this.actionChoiceDict["sitting up"] = true;
        

        this.thisHuman.GetBody().SetBodyState("laying", false);
        this.thisHuman.GetBody().SetBodyState("sitting", true);          
    }
        
    public void StandUp(){

        this.actionChoiceDict["standing up"] = true;

        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;
        if (transform.position.y >= -2.0f)
        {
            this.thisHuman.GetBody().GetJointDict()["Leg_L"].targetRotation = Quaternion.Euler(0, 0, 0);
            this.thisHuman.GetBody().GetJointDict()["Leg_R"].targetRotation = Quaternion.Euler(0, 0, 0);
            drive.positionDamper = 100;
            drive.positionSpring = 100;
            drive.maximumForce = 3.402823e+38f;
            this.thisHuman.GetBody().GetJointDict()["Leg_L"].angularXDrive = drive;
            this.thisHuman.GetBody().GetJointDict()["Leg_R"].angularXDrive = drive;
            bodyTransform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
            if (bodyTransform.localPosition.y < 0f)
            {
                bodyTransform.Translate(Vector3.up * 2 * Time.deltaTime, Space.World);
            }
        }
        else
        {
            Vector3 dir = ((transform.up + transform.forward) / 2).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30, 0, 0), Time.deltaTime);
            transform.Translate(dir * 2 * Time.deltaTime, Space.World);
        }

        this.thisHuman.GetBody().SetBodyState("sitting", false);
        this.thisHuman.GetBody().SetBodyState("standing", true);  
       
    }

    public void LayDown(){
        this.actionChoiceDict["laying down"] = true;
        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;

        if (this.thisHuman.GetBody().GetBodyStateDict()["sitting"])
        {
            this.thisHuman.GetBody().GetSkeletonDict()["Body"].GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            SitDown();
        }

        this.thisHuman.GetBody().SetBodyState("sitting", false);
        this.thisHuman.GetBody().SetBodyState("laying", true);  
    }

    public void Sleep(){

        this.actionChoiceDict["falling asleep"] = true;
        this.thisHuman.GetBody().SetBodyState("sleeping", true);  
    }
    
    public void WakeUp(){
        this.actionChoiceDict["waking up"] = true;
        this.thisHuman.GetBody().SetBodyState("sleeping", false);  
    }
    
    public void PickUp(float hand) {
        
    }

    public void SetDown(float hand) {}

    public void Eat(float hand) {}
}

