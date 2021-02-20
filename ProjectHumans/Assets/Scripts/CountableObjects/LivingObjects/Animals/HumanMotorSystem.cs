using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanMotorSystem : MotorSystem
{
    private Human thisHuman;    float max_rotation_speed = 1;
    float maxStepDistance = 2;

    // Where is this object type created?
    JointDrive drive;
    public bool rotating = false;

    public float velocity = 0;

    public HumanMotorSystem(Human human) : base(human) {
        this.thisHuman = human;
        stateLabelList = new List<string>
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
        this.InitStates(stateLabelList);

        argsLabelList = new List<string>
        {        
            "movement velocity",
            "step rate",                          
            "rotation velocity",               
            "hand",
            "hand target x",
            "hand target y",
            "hand target z"
        };
        this.InitActionArguments(argsLabelList);
    }

    public override void TakeAction(AI.ActionChoiceStruct actionChoiceStruct)
    {
        //https://stackoverflow.com/questions/4233536/c-sharp-store-functions-in-a-dictionary
        //actionstates
        //body states

        //using passed dict to take the action (read only)
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

    float t = 0.0f;
    float x = 0.5f;
    public void TakeSteps(float stepProportion) {
        if (stepProportion != 0) {
            this.thisHuman.gameObject.transform.Translate(this.thisHuman.gameObject.transform.forward * stepProportion * Time.deltaTime, Space.World);
        }
        this.thisHuman.GetBody().GetJointDict()["Femur_L"].targetRotation = new Quaternion(Mathf.Lerp(-stepProportion, stepProportion, t), 0, 0, 1);
        this.thisHuman.GetBody().GetJointDict()["Femur_R"].targetRotation = new Quaternion(Mathf.Lerp(stepProportion, -stepProportion, t), 0, 0, 1);

        if (t > 1.0f) {
            x = -0.5f;
        } else if (t <= 0) {
            x = 0.5f;
        }
        t += x * Time.deltaTime;
    }

    public void Rotate(float rotatingSpeed) {
        if (rotatingSpeed != 0) {
            this.thisHuman.gameObject.transform.Rotate(0, rotatingSpeed, 0, Space.World);
            rotating = true;
        }
    }

    public void Drink() {
        // This doesn't make sense to me... choice dict is something used by AI to communicate with motor, so why is it changed here?
        //this.actionChoiceDict["drinking"] = true;

        this.thisHuman.GetDriveSystem().SetState("thirst", 0);
    }
    
    public void SitDown() {
        // this.actionChoiceDict["sitting down"] = true;

        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;
        if (!this.thisHuman.GetBody().GetStateDict()["sitting"]) {
            Vector3 dir = ((-bodyTransform.up - bodyTransform.forward) / 2).normalized;
            bodyTransform.Translate(dir * 2f * Time.deltaTime, Space.World);
        }
        this.thisHuman.GetBody().SetState("standing", false);
        this.thisHuman.GetBody().SetState("sitting", true);                     
    }

    public void SitUp(){
        // this.actionChoiceDict["sitting up"] = true;

        this.thisHuman.GetBody().SetState("laying", false);
        this.thisHuman.GetBody().SetState("sitting", true);          
    }
        
    public void StandUp(){
        // this.actionChoiceDict["standing up"] = true;

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
        } else {
            Vector3 dir = ((transform.up + transform.forward) / 2).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30, 0, 0), Time.deltaTime);
            transform.Translate(dir * 2 * Time.deltaTime, Space.World);
        }

        this.thisHuman.GetBody().SetState("sitting", false);
        this.thisHuman.GetBody().SetState("standing", true);  
       
    }

    public void LayDown(){
        // this.actionChoiceDict["laying down"] = true;

        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;

        if (this.thisHuman.GetBody().GetStateDict()["sitting"]) {
            this.thisHuman.GetBody().GetSkeletonDict()["Body"].GetComponent<Rigidbody>().isKinematic = false;
        } else {
            SitDown();
        }

        this.thisHuman.GetBody().SetState("sitting", false);
        this.thisHuman.GetBody().SetState("laying", true);  
    }

    public void Sleep(){
        // this.actionChoiceDict["falling asleep"] = true;

        this.thisHuman.GetBody().SetState("sleeping", true);  
    }
    
    public void WakeUp(){
        //this.actionChoiceDict["waking up"] = true;

        this.thisHuman.GetBody().SetState("sleeping", false);  
    }
    
    public void PickUp(float hand) {}

    public void SetDown(float hand) {}

    public void Eat(float hand) {}
}

