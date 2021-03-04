using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanMotorSystem : MotorSystem
{
    private Human thisHuman;
    Transform transform;
    bool pickedUp = false;

    JointDrive drive;

    public float velocity = 0;
    public bool check = false;

    float stepProportion = .01f;
    float rotatingSpeed = 2f;

    Vector3 moveToPosition = new Vector3();

    public HumanMotorSystem(Human human) : base(human) {
        this.thisHuman = human;
        stateLabelList = new List<string> {
            "sitting down",// 0
            "sitting up",  // 1
            "laying down", // 2
            "standing up", // 3
            "rotating",    // 4
            "taking steps",// 5
            "picking up",  // 6
            "setting down",// 7 
            "consuming",   // 8
            "waking up",   // 9
            "sleeping",    // 10
            "resting",     // 11
            "looking"      // 12
        };
        this.InitStates(stateLabelList);

        argsLabelList = new List<string> {
            "step rate",                          
            "rotation velocity",               
            "held position",
            "target x",
            "target y",
            "target z"
        };

        this.InitActionArguments(argsLabelList);
        this.InitActionDict();
    }

    public override void TakeAction(int[] toDoList) {
        for(int i = 0; i < toDoList.Length; i++) {
            if (i == 1) {
                actionList[i].DynamicInvoke();
            } 
        }
    }

    public override void InitActionDict() {
        actionList = new List<Action>();

        actionList.Add(SitDown);
        actionList.Add(SitUp);
        actionList.Add(LayDown);
        actionList.Add(StandUp);
        actionList.Add(Rotate);
        actionList.Add(TakeSteps);
        actionList.Add(PickUp);
        actionList.Add(SetDown);
        actionList.Add(Consume);
        actionList.Add(WakeUp);
        actionList.Add(Sleep);
        actionList.Add(Rest);
        actionList.Add(LookAt);

    }
    
    public void SitDown() {
        Debug.Log("SitDown was called");
        BendWaist(0.5f, 1f);
         this.thisHuman.GetBody().GetSkeletonDict()["Abdomen"].GetComponent<Rigidbody>().isKinematic = false;
        // Vector3 dir = ((-bodyTransform.up - bodyTransform.forward) / 2).normalized;
        // bodyTransform.Translate(dir * 1f * Time.deltaTime, Space.World);

        this.thisHuman.GetBody().SetState("standing", false);
        this.thisHuman.GetBody().SetState("sitting", true);
    }

    public void SitUp() {
        Debug.Log("SitUp was called");
        this.thisHuman.GetBody().GetSkeletonDict()["Abdomen"].GetComponent<Rigidbody>().isKinematic = true;
        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Abdomen"].transform;
        Vector3 humanPosition = this.thisHuman.gameObject.transform.position;

        if (thisHuman.GetBodyState("laying")) {
            moveToPosition = new Vector3(bodyTransform.position.x, humanPosition.y - 1.0f, bodyTransform.position.z);

        } else if (bodyTransform.localPosition.y >= -1.9 || bodyTransform.localRotation.x >= 0) {

            if(bodyTransform.localRotation.x < 0) {
                bodyTransform.Rotate(Vector3.right * 30 * Time.deltaTime);

            } else if(!thisHuman.GetBodyState("sitting")) {
                moveToPosition = new Vector3(bodyTransform.position.x, humanPosition.y - 1.2f, bodyTransform.position.z);
            }
        }
        bodyTransform.position = Vector3.MoveTowards(bodyTransform.position, moveToPosition, 1.0f * Time.deltaTime);

        this.thisHuman.GetBody().SetState("laying", false);
        this.thisHuman.GetBody().SetState("sitting", true);          
    }

    public void LayDown() {
        Debug.Log("Tried to lay down");
        BendWaist(0f, 0f);

        if (thisHuman.GetBodyState("sitting") || thisHuman.GetBodyState("laying")) {
            this.thisHuman.GetBody().GetSkeletonDict()["Abdomen"].GetComponent<Rigidbody>().isKinematic = false;
        } else {
            SitDown();
        }

        this.thisHuman.GetBody().SetState("sitting", false);
        this.thisHuman.GetBody().SetState("laying", true);  
    }
    
    public void StandUp(){
        Debug.Log("StandUp was called");

        Vector3 goalPosition = (thisHuman.GetBody().GetXZPosition() + new Vector3(0, thisHuman.GetBody().GetHeight(), 0));
        thisHuman.GetBody().TranslateSkeletonTo("Abdomen", goalPosition);

        thisHuman.GetBody().SetState("laying", false);
        thisHuman.GetBody().SetState("sitting", false);
        thisHuman.GetBody().SetState("standing", true);  
    }

    public void Rotate() {
        this.thisHuman.gameObject.transform.Rotate(0, rotatingSpeed, 0, Space.World);
    }

    
    public void TakeSteps() {
        float t = 0.0f;
        float x = 0.5f;

        this.thisHuman.gameObject.transform.Translate(this.thisHuman.gameObject.transform.forward * stepProportion * Time.deltaTime, Space.World);
            
        thisHuman.GetBody().RotateJoint("Femur_L", new Quaternion(Mathf.Lerp(-stepProportion, stepProportion, t), 0, 0, 1));
        thisHuman.GetBody().RotateJoint("Femur_R", new Quaternion(Mathf.Lerp(stepProportion, -stepProportion, t), 0, 0, 1));

        if (t > 1.0f) {
             x = -0.5f;
        } else { x = 0.5f; }
        t += x * Time.deltaTime;
    }

    public void PickUp() {
        Debug.Log("Tried to pick something up");
        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;
        Vector3 humanPosition = this.thisHuman.gameObject.transform.position;
        
        BendWaist(0.3f, 1f);

        Vector3 heldPos = thisAnimal.GetBody().GetHolderCoords(argsDict["held position"]);
    }

    public void SetDown() {}

    
    public void Consume () {
        Vector3 heldPos = thisHuman.GetBody().GetHolderCoords(argsDict["held position"]);

        if (heldPos.x > thisHuman.GetBody().GetXZPosition().x) {
            this.thisHuman.GetBody().GetJointDict()["Humerus_L"].targetRotation = new Quaternion(1.0f, 0, 0, 1);
            this.thisHuman.GetBody().GetJointDict()["Radius_L"].angularXMotion = ConfigurableJointMotion.Free;
            this.thisHuman.GetBody().GetJointDict()["Radius_L"].targetRotation = new Quaternion(2.0f, 0, 0, 1);

        } else {
            this.thisHuman.GetBody().GetJointDict()["Humerus_R"].targetRotation = new Quaternion(1.0f, 0, 0, 1);
            this.thisHuman.GetBody().GetJointDict()["Radius_R"].angularXMotion = ConfigurableJointMotion.Free;
            this.thisHuman.GetBody().GetJointDict()["Radius_R"].targetRotation = new Quaternion(2.0f, 0, 0, 1);
        }
        // Stat buffs and destroy the comsumed object
    }

    public void WakeUp(){
        thisHuman.ToggleBodyPart("Eye_L", true);
        thisHuman.ToggleBodyPart("Eye_R", true);
        
        this.thisHuman.GetBody().SetState("sleeping", false);  
    }

    public void Sleep(){
        thisHuman.ToggleBodyPart("Eye_L", false);
        thisHuman.ToggleBodyPart("Eye_R", false);

        this.thisHuman.GetBody().SetState("sleeping", true);  
    }
    
    public void Rest() {}

    public void LookAt() {}


    public void BendKnees(float degree) {}

    public void BendWaist(float degree, float z) {
        Quaternion toSend = new Quaternion(degree, 0, 0, z);
        thisHuman.GetBody().RotateJoint("Hip_L", toSend);
        thisHuman.GetBody().RotateJoint("Hip_R", toSend);
    }
}


