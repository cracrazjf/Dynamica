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

    Vector3 moveToPosition = new Vector3();

    public HumanMotorSystem(Human human) : base(human) {
        this.thisHuman = human;
        stateLabelList = new List<string> {
            "sitting down", 
            "sitting up", 
            "laying down",
            "standing up", 
            "rotating", 
            "taking steps",
            "picking up", 
            "setting down",
            "consuming",
            "waking up",
            "falling asleep"
        };
        this.InitStates(stateLabelList);

        argsLabelList = new List<string> {
            "step rate",                          
            "rotation velocity",               
            "hand",
            "hand target x",
            "hand target y",
            "hand target z"
        };
        this.InitActionArguments(argsLabelList);
    }

    float t = 0.0f;
    float x = 0.5f;
    public void TakeSteps(float stepProportion) {
        this.thisHuman.gameObject.transform.Translate(this.thisHuman.gameObject.transform.forward * stepProportion * Time.deltaTime, Space.World);
            
        thisHuman.GetBody().RotateJoint("Hip_L", new Quaternion(Mathf.Lerp(-stepProportion, stepProportion, t), 0, 0, 1));
        thisHuman.GetBody().RotateJoint("Hip_R", new Quaternion(Mathf.Lerp(stepProportion, -stepProportion, t), 0, 0, 1));

        if (t > 1.0f) {
             x = -0.5f;
        } else { x = 0.5f; }
        t += x * Time.deltaTime;
    }

    public void Rotate(float rotatingSpeed) {
        this.thisHuman.gameObject.transform.Rotate(0, rotatingSpeed, 0, Space.World);
    }

    public void Drink() {}
    
    public void SitDown() {
        
        BendWaist(0.5f, 1f);

        // Vector3 dir = ((-bodyTransform.up - bodyTransform.forward) / 2).normalized;
        // bodyTransform.Translate(dir * 1f * Time.deltaTime, Space.World);

        this.thisHuman.GetBody().SetState("standing", false);
        this.thisHuman.GetBody().SetState("sitting", true);
    }

    public void SitUp() {

        this.thisHuman.GetBody().GetSkeletonDict()["Body"].GetComponent<Rigidbody>().isKinematic = true;
        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;
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

    
    public void StandUp(){

        this.thisHuman.GetBody().GetSkeletonDict()["Body"].GetComponent<Rigidbody>().isKinematic = true;
        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;
        Vector3 humanPosition = this.thisHuman.gameObject.transform.position;
        
        if (thisHuman.GetBodyState("sitting")) {
            moveToPosition = new Vector3(humanPosition.x, humanPosition.y + 0.2f, humanPosition.z);
        }  else if (bodyTransform.position.y >= humanPosition.y + 0.2f){
            moveToPosition = humanPosition;
        }
        bodyTransform.position = Vector3.MoveTowards(bodyTransform.position, moveToPosition, 1.0f * Time.deltaTime);


        this.thisHuman.GetBody().SetState("sitting", false);
        this.thisHuman.GetBody().SetState("standing", true);  
    }

    public void LayDown() {
        BendWaist(0f, 0f);

        if (thisHuman.GetBodyState("sitting") || thisHuman.GetBodyState("laying")) {
            this.thisHuman.GetBody().GetSkeletonDict()["Body"].GetComponent<Rigidbody>().isKinematic = false;
        } else {
            SitDown();
        }

        this.thisHuman.GetBody().SetState("sitting", false);
        this.thisHuman.GetBody().SetState("laying", true);  
    }

    public void Sleep(){

        thisHuman.ToggleBodyPart("Eye_L", false);
        thisHuman.ToggleBodyPart("Eye_R", false);

        this.thisHuman.GetBody().SetState("sleeping", true);  
    }
    
    public void WakeUp(){
        thisHuman.ToggleBodyPart("Eye_L", true);
        thisHuman.ToggleBodyPart("Eye_R", true);
        
        this.thisHuman.GetBody().SetState("sleeping", false);  
    }
    
    public void PickUp(float hand) {
        Transform bodyTransform = this.thisHuman.GetBody().GetSkeletonDict()["Body"].transform;
        Vector3 humanPosition = this.thisHuman.gameObject.transform.position;
        if(!pickedUp) {
            BendWaist(0.3f, 1f);
            if (bodyTransform.localPosition.y <= -1) {
                bodyTransform.localRotation = Quaternion.Euler(45, 0, 0);
                if (bodyTransform.localEulerAngles.x >= 45) {
                    if (bodyTransform.localPosition.y <= -1.3f) {
                        if (hand == 0) {
                            thisHuman.GetBody().RotateJoint("Humerus_L", new Quaternion(1.5f, 0, 0, 1));

                        } else {
                            thisHuman.GetBody().RotateJoint("Humerus_R", new Quaternion(1.5f, 0, 0, 1));
                        }
                    } else {
                        bodyTransform.Translate(transform.forward * 2 * Time.deltaTime);
                    }
                }
            } else {
                moveToPosition = new Vector3(humanPosition.x, humanPosition.y - 0.8f, humanPosition.z);
            }
            bodyTransform.position = Vector3.MoveTowards(bodyTransform.position, moveToPosition, 1.0f * Time.deltaTime);
        } else {
            if (hand == 0) {
                thisHuman.GetBody().RotateJoint("Humerus_L", new Quaternion(0, 0, 0, 1));

            } else {
                thisHuman.GetBody().RotateJoint("Humerus_R", new Quaternion(0, 0, 0, 1));
            }
            bodyTransform.localRotation = Quaternion.Euler(0, 0, 0);
            moveToPosition = new Vector3(humanPosition.x, humanPosition.y, humanPosition.z);
            bodyTransform.position = Vector3.MoveTowards(bodyTransform.position, moveToPosition, 1.0f * Time.deltaTime);
            BendWaist(0f, 0f);
        }
    }

    public void SetDown(float hand) {}

    public void Consume (float hand) {
        if (hand == 0) {
            this.thisHuman.GetBody().GetJointDict()["Humerus_L"].targetRotation = new Quaternion(1.0f, 0, 0, 1);
            this.thisHuman.GetBody().GetJointDict()["Radius_L"].angularXMotion = ConfigurableJointMotion.Free;
            this.thisHuman.GetBody().GetJointDict()["Radius_L"].targetRotation = new Quaternion(2.0f, 0, 0, 1);

        } else {
            this.thisHuman.GetBody().GetJointDict()["Humerus_R"].targetRotation = new Quaternion(1.0f, 0, 0, 1);
            this.thisHuman.GetBody().GetJointDict()["Radius_R"].angularXMotion = ConfigurableJointMotion.Free;
            this.thisHuman.GetBody().GetJointDict()["Radius_R"].targetRotation = new Quaternion(2.0f, 0, 0, 1);
        }
    }

    public void BendKnees(float degree) {}

    public void BendWaist(float degree, float z) {
        Quaternion toSend = new Quaternion(degree, 0, 0, z);
        thisHuman.GetBody().RotateJoint("Hip_L", toSend);
        thisHuman.GetBody().RotateJoint("Hip_R", toSend);
    }
}


