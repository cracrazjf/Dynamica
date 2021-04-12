using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System.Linq;

public class PrimateMotorSystem : MotorSystem {

    Transform abdomenTrans;
    PrimateBody primateBody;
    Vector3 goalPos;
    int footUpdates = 0;
    bool rightStep = true; // change this at some point like handedness

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
        } else { SitUp(); }
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
        float degree = stateDict["rotating"] * 0.5f;
        float rotatingSpeed = degree * thisAnimal.GetPhenotype().GetTrait("max_rotation");

        thisBody.globalPos.Rotate(0, rotatingSpeed, 0, Space.World);
    }
    
    public override void TakeSteps() {
        Debug.Log("Walking");

        float degree = stateDict["taking steps"];
        float stepProportion = degree * thisAnimal.GetPhenotype().GetTrait("max_step") * 0.5f;

        if (footUpdates < 500) {
            LegUp(rightStep);
            footUpdates++;
        } else {
            footUpdates = 0;
            rightStep = !rightStep;
        }

        abdomenTrans.Translate(thisBody.globalPos.forward * stepProportion * Time.deltaTime);
    }
    
    public override void UseHand() {
        GameObject holder = GetActiveHand();
        if (stateDict["hand action"] == 1f) {
            PickUp();
        } else {
            DropArm();
        }
    }

    public override void Consume() {
        Debug.Log("Tried to eat something");
        GameObject holder = GetActiveHand();

        if (ArmTo(thisBody.head.transform.position)) {
            thisBody.EatObject(holder.name);
            DropArm();
        }
    }

    public override void Sleep() {
        if (stateDict["consuming"] == 1f) {
            FallAsleep();
        } else { WakeUp(); }
    }
    
    public override void Rest() {
        Debug.Log("Resting");
        thisBody.RestAdjust();
    }
    
    public override void Look() {
        if (stateDict["looking"] == 1f) {
            LookAt();
        } else { LookForward(); }
    }

    // BEGIN HELPER FUNCTIONS

    void SitDown() {
        if (thisBody.CheckCrouching()) {
            Vector3 toSend = abdomenTrans.position;
            double sitHeight = thisBody.GetHeight() / 4.0;

            if (toSend.y > sitHeight) {
                thisBody.DisableKinematic("Abdomen");

            } else {
                thisBody.EnsureKinematic("Abdomen");
                Debug.Log("I think I'm sitting");
            }
        } else {
            CrouchDown();
        }
    }

    void SitUp() {
        if (thisBody.GetState("laying") == 1f) {
            BendLegs(45f, 0f);
        } else {
            thisBody.RotateJointTo("Hips", new Quaternion(0,0,0,0)); 

            BendLegs(0f, 0f);
            BendKnees(0f);
        }          
    }

    void LayDown() {
        Debug.Log("Tried to lay down");

        if (thisAnimal.GetBodyState("sitting")) {
            Collapse();
        } else { SitDown(); }  
    }
    
    // Functional
    void StandUp() {
        Vector3 toSend = abdomenTrans.position;
        if (toSend.y < thisBody.GetHeight()) {
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime));
            
        } else { Debug.Log("I think I'm standing"); } 
    }

    // Untested
    void PickUp() {
        Debug.Log("Tried to pick something up");
        GameObject holder = GetActiveHand();
        
        if (ArmToGoal()) {    
            GrabWithHolder(holder);
            DropArm();
        } 
    }

    // Untested
    void SetDown() {
        Debug.Log("Tried to set something down");
        GameObject holder = GetActiveHand();

        if (thisBody.CheckSitting()) {
            RemoveFromHolder(holder);
        } else { KneelDown(); }
    }

    // Functional
    void WakeUp() {
        Debug.Log("Waking up");
        ToggleEyes(true);
        thisBody.SetState("sleeping", -1f);  
    }

    // Nonfunctional - no sleep adjustment
    void FallAsleep() {
        Debug.Log("Sleeping");
        ToggleEyes(false);
        
        if (thisBody.CheckLaying()) {
            thisBody.SetState("sleeping", 1f);
            thisBody.SleepAdjust();
        } else { LayDown(); }
    }

    // Functional (but hated)
    void LookForward() {
        Quaternion toSend = Quaternion.LookRotation(thisBody.globalPos.forward);
        thisBody.RotateJointTo("Head", toSend);
    }

    // Need to look at active hand
    void LookAt() {
        GameObject holder = GetActiveHand();
        Vector3 direction = (holder.transform.position - thisBody.head.transform.position).normalized;
        Quaternion toSend = Quaternion.LookRotation(direction);

        thisBody.RotateJointTo("Head", toSend);
    }

    // Untested
    bool ArmTo(Vector3 targetPos) {
        string arm = GetActiveHand().name;

        thisBody.EnsureKinematic(arm);
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        armTrans.position = Vector3.Slerp(armTrans.position, targetPos, Time.deltaTime);

        return (armTrans.position == targetPos);
    }

    // Functional
    void BendKnees(float degree) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);

        thisBody.RotateJointBy("Tibia_L", toSend);
        thisBody.RotateJointBy("Tibia_R", toSend);
    }

    void LegUp(bool right) {
        Quaternion forward = new Quaternion(30, 0, 0, 0);
        Quaternion back = new Quaternion(-30, 0, 0, 0);

        if(right) {
            thisBody.RotateJointBy("Tibia_L", back);
            thisBody.RotateJointBy("Tibia_R", forward);
        } else {
            thisBody.RotateJointBy("Tibia_L", forward);
            thisBody.RotateJointBy("Tibia_R", back);
        }
    }

    



    // Functional
    void BendLegs(float xDegree, float yDegree) {
        Quaternion sendLeft = new Quaternion(xDegree, yDegree, 0, 0);
        Quaternion sendRight = new Quaternion(xDegree, -yDegree, 0, 0);

        thisBody.RotateJointBy("Femur_L", sendLeft);
        thisBody.RotateJointBy("Femur_R", sendRight);
    }

    // Untested
    bool ArmToGoal() {
        // string arm = GetActiveHand().name;
        // if (stateDict["active right"] == 1f) {
        //     goalPos = primateBody.localStartRight;
        // } else { Vector3 goalPos = primateBody.localStartLeft; }

        // float xTrans = stateDict["RP x"] * thisBody.xMax;
        // float yTrans = stateDict["RP y"] * thisBody.yMax;
        // float zTrans = stateDict["RP z"] * thisBody.zMax;
        // Vector3 toAdd = new Vector3(xTrans, yTrans, zTrans);

        // // Ensure values are actually set to something
        // if (toAdd.x == 0f && toAdd.y == 0f) {)

        Transform armTrans = GetActiveHand().transform;
        Debug.Log("Reaching default amount");
        Vector3 goalPos = abdomenTrans.position + new Vector3(.5f, -.5f, 1f);
        thisBody.EnsureKinematic(armTrans.name);
        armTrans.position = Vector3.Slerp(armTrans.position, goalPos, Time.deltaTime);

        return (armTrans.position == goalPos);
    }

    // Functional
    void DropArm() {
        string arm = GetActiveHand().name;
        thisBody.DisableKinematic(arm);
    }

    // Functional
    void KneelDown() {
        Vector3 toSend = abdomenTrans.position;
        if (toSend.y > thisBody.GetHeight()/2 + 0.5) {

            BendLegs(30f, 0f);
            BendKnees(-45f);
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));

        } else { Debug.Log("I think I'm kneeling"); }
    }

    // Functional
    void CrouchDown() {
        Debug.Log("Crouch was called");

        if (thisBody.CheckCrouching()) {
            BendLegs(-1f, 0f);
            BendLegs(80f, 0f);
            BendKnees(-30f);
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));
            
        } else { thisBody.SetState("crouching", 1f); }
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
                } else { Debug.Log("Object too big to lift!"); }
            } else { Debug.Log("Nothing to pick up!"); }
        }
    }

    void RemoveFromHolder(GameObject holder) {
        string holderName = holder.name;
        if (thisBody.GetHeld(holderName) != null) { thisBody.RemoveObject(holderName); }
    }

    bool CheckMovableObject(GameObject toLift) {
        float liftMass = toLift.GetComponent<Rigidbody>().mass;
        return (liftMass < thisBody.rigidbody.mass);
    }

    GameObject GetActiveHand() { 
        if (stateDict["active right"] == 1f)  {
            return thisBody.GetSkeleton("Hand_R");
        } else { return thisBody.GetSkeleton("Hand_L"); }
    }

    void ToggleEyes(bool open) {
        thisBody.GetSkeleton("Eye_R").SetActive(open);
        thisBody.GetSkeleton("Eye_L").SetActive(open);
    }
}


