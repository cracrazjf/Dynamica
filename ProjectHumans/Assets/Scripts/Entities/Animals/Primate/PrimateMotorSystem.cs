using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System.Linq;

public class PrimateMotorSystem : MotorSystem {

    Transform abdomenTrans;
    PrimateBody primateBody;

    public PrimateMotorSystem(Animal animal) : base(animal) {
        Debug.Log("An ape was born!");
        abdomenTrans = thisBody.abdomen.transform;
        primateBody = (PrimateBody)thisBody;
    }
    
    public override void Crouch() {
        if (stateDict["crouching"] == -1f) {
            CrouchDown();
        } else {}
    }

    public override void Sit() {
        if (stateDict["sitting"] == -1f) {
            SitDown();
        } else {}
    }

    public override void Lay() {
        if (stateDict["laying down"] == -1f) {
            LayDown();
        } else {}
    }

    public override void Stand() {
        if (stateDict["standing up"] == 1f) {
            StandUp();
        } else {}
    }

    public override void Rotate() {
        float degree = stateDict["rotating"];
        float rotatingSpeed = degree * thisAnimal.GetPhenotype().GetTrait("max_rotation");

        thisBody.globalPos.Rotate(0, rotatingSpeed, 0, Space.World);
    }
    
    public override void TakeSteps() {
        Debug.Log("Walking");

        float degree = stateDict["walking"];
        float stepProportion = degree * thisAnimal.GetPhenotype().GetTrait("max_step");

        thisBody.TranslateBodyTo(thisBody.globalPos.forward * stepProportion);

    }
    
    public override void UseHand() {
        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (stateDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        }

        if (stateDict["hand action"] == 1f) {
            GrabWithHolder(holder);
        } else {
            RemoveFromHolder(holder);
        }
    }

    public override void Consume() {
        Debug.Log("Tried to eat something");
        
        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (stateDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        }

        if (ArmTo(thisBody.head.transform.position)) {
            thisBody.EatObject(holder.name);
            DropArm();
        }
    }

    public override void Sleep() {
        if (stateDict["cosuming"] == 1f) {
            FallAsleep();
        } else {
            WakeUp();
        }
    }
    
    public override void Rest() {
        Debug.Log("Resting");
        thisBody.RestAdjust();
    }
    
    public override void Look() {
        if (stateDict["looking"] == 1f) {
            LookAt();
        } else {
            LookForward();
        }
    }

    // BEGIN HELPER FUNCTIONS


    // Functional
    void SitDown() {
        CrouchDown();
    }

    void SitUp() {
        //Debug.Log("SitUp was called");
        if (thisBody.GetState("laying") == 1f) {
            BendLegs(45f, 0f, true);
        } else {
            thisBody.RotateJointTo("Hips", new Quaternion(0,0,0,0)); 

            BendLegs(0f, 0f, false);
            BendKnees(0f, false);
        }          
    }

    void LayDown() {
        Debug.Log("Tried to lay down");

        if ((thisAnimal.GetBodyState("sitting")) || (thisAnimal.GetBodyState("laying"))) {
            Collapse();
        } else {
            SitDown();
        }  
    }
    
    // Functional
    void StandUp() {
        Vector3 toSend = abdomenTrans.position;

        if (toSend.y < thisBody.GetHeight()) {
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime));
            
        } else {
            Debug.Log("I think I'm standing");
        } 
    }

    // Untested
    void PickUp() {
        Debug.Log("Tried to pick something up");

        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (stateDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        } 
        
        if (ArmToGoal()) {    
            GrabWithHolder(holder);
            DropArm();
        }
    }

    // Untested
    void SetDown() {
        Debug.Log("Tried to set something down");

        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (stateDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        }

        if (thisBody.CheckSitting()) {
            RemoveFromHolder(holder);
        } else {
            KneelDown();
        }
    }

    // Functional
    void WakeUp() {
        Debug.Log("Waking up");
        
        //thisAnimal.ToggleBodyPart("Eye_L", true);
        //thisAnimal.ToggleBodyPart("Eye_R", true);
        
        thisBody.SetState("sleeping", 0f);  
    }

    // Nonfunctional - no sleep adjustment
    void FallAsleep() {
        Debug.Log("Sleeping");

        //thisAnimal.ToggleBodyPart("Eye_L", false);
        //thisAnimal.ToggleBodyPart("Eye_R", false);
        Collapse();

        thisBody.SetState("sleeping", 1f);
        thisBody.SleepAdjust();
    }

    // Functional (but hated)
    void LookForward() {
        Quaternion toSend = Quaternion.LookRotation(thisBody.globalPos.forward);
        thisBody.RotateJointTo("Head", toSend);
    }

    // Need to look at active hand
    void LookAt() {
        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (stateDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        }
        Vector3 direction = (holder.transform.position - thisBody.head.transform.position).normalized;
        Quaternion toSend = Quaternion.LookRotation(direction);

        thisBody.RotateJointTo("Head", toSend);
    }

    // Untested
    bool ArmTo(Vector3 targetPos) {
        string arm = "Hand_L";
        if (stateDict["active right"] == 1f) {
            arm = "Hand_R";
        }

        thisBody.EnsureKinematic(arm);
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        armTrans.position = Vector3.Slerp(armTrans.position, targetPos, Time.deltaTime);

        return (armTrans.position == targetPos);
    }

    // Functional
    void BendKnees(float degree, bool passedGoal) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);

        if(passedGoal) {
            thisBody.RotateJointTo("Tibia_L", toSend);
            thisBody.RotateJointTo("Tibia_R", toSend);
        } else {
            thisBody.RotateJointBy("Tibia_L", toSend);
            thisBody.RotateJointBy("Tibia_R", toSend);
        }
    }

    // Functional
    void BendLegs(float xDegree, float yDegree, bool passedGoal) {
        Quaternion sendLeft = new Quaternion(xDegree, yDegree, 0, 0);
        Quaternion sendRight = new Quaternion(xDegree, -yDegree, 0, 0);

        if(passedGoal) {
            thisBody.RotateJointTo("Femur_L", sendLeft);
            thisBody.RotateJointTo("Femur_R", sendRight);
        } else {
            thisBody.RotateJointBy("Femur_L", sendLeft);
            thisBody.RotateJointBy("Femur_R", sendRight);
        }
    }

    // Untested
    bool ArmToGoal() {
        string arm = "Hand_L";
        Vector3 goalPos = primateBody.localStartLeft;

        if (stateDict["active right"] == 1f) {
            arm = "Hand_R";
            goalPos = primateBody.localStartRight;
        }

        float xTrans = stateDict["reach proportion x"] * thisBody.xMax;
        float yTrans = stateDict["reach proportion y"] * thisBody.yMax;
        float zTrans = stateDict["reach proportion z"] * thisBody.zMax;
        goalPos += new Vector3(xTrans, yTrans, zTrans);

        thisBody.EnsureKinematic(arm);
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        armTrans.position = Vector3.Slerp(armTrans.position, goalPos, Time.deltaTime);

        return (armTrans.position == goalPos);
    }

    // Functional
    void DropArm() {
        string arm = "Hand_L";
        if (stateDict["active right"] == 1f) {
            arm = "Hand_R";
        }

        thisBody.DisableKinematic(arm);
    }

    // Functional
    void KneelDown() {
        Vector3 toSend = abdomenTrans.position;
        if (toSend.y > thisBody.GetHeight()/2 + 0.5) {

            BendLegs(30f, 0f, false);
            BendKnees(-45f, false);
 
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));
        } else {
            Debug.Log("I think I'm kneeling");
        }
    }

    // Functional
    void CrouchDown() {
        Debug.Log("Crouch was called");
        Vector3 toSend = abdomenTrans.position;
        double crouchHeight = thisBody.GetHeight()/1.5 + 0.5;
        if (toSend.y > crouchHeight) {

            BendLegs(-1f, 0f, false);
            BendLegs(80f, 0f, false);
            BendKnees(-30f, false);
        
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));
            
        } else {
            Debug.Log("I think I'm squatting");
        }
    }

    // Functional
    void Collapse() {
        Debug.Log("Collapsing");
        thisBody.DisableKinematic("Abdomen");
    }

    // Untested
    void GrabWithHolder(GameObject holder) {
        Debug.Log("Tried to affix something");

        Vector3 forCollider = holder.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(forCollider, .025f);
        Rigidbody toConnect = holder.GetComponent<Rigidbody>();
        
        foreach(var hit in hitColliders) {
            if(!thisBody.GetSkeletonDict().ContainsKey(hit.gameObject.name)) {
                
                if (CheckMovableObject(hit.gameObject)) {
                    FixedJoint newJoint = hit.gameObject.AddComponent<FixedJoint>() as FixedJoint;
                    newJoint.connectedBody = toConnect;
                }
            }
        }
    }

    // Untested
    void RemoveFromHolder(GameObject holder) {
        string holderName = holder.name;
        if (thisBody.GetHeld(holderName) != null) {
                thisBody.RemoveObject(holderName);
        }
    }

    // Untested
    bool CheckMovableObject(GameObject toLift) {
        float liftMass = toLift.GetComponent<Rigidbody>().mass;

        if (liftMass < thisBody.rigidbody.mass) {
            return true;
        } 
        return false;
    }
}


