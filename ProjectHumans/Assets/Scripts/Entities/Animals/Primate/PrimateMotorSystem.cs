using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrimateMotorSystem : MotorSystem {

    Transform abdomenTrans;

    public PrimateMotorSystem(Animal animal) : base(animal) {
        Debug.Log("An ape was born!");
        abdomenTrans = thisBody.abdomen.transform;
    }
    
    // Functional
    public override void SitDown() {
        //Debug.Log("SitDown was called");
        Crouch();
        //Kneel();
    }

    public override void SitUp() {
        //Debug.Log("SitUp was called");
        if (thisBody.GetState("laying")) {
            BendLegs(45f, 0f, true);
        } else {
            thisBody.RotateJointTo("Hips", new Quaternion(0,0,0,0)); 

            BendLegs(0f, 0f, false);
            BendKnees(0f, false);
        }          
    }

    public override void LayDown() {
        Debug.Log("Tried to lay down");

        if (thisAnimal.GetBodyState("sitting") || thisAnimal.GetBodyState("laying")) {
            Collapse();
        } else {
            SitDown();
        }  
    }
    
    // Functional
    public override void StandUp() {
        Vector3 toSend = abdomenTrans.position;

        if (toSend.y < thisBody.GetHeight()) {
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime));
            
        } else {
            //ArmUp(true, .025f); 
            Debug.Log("I think I'm standing");
        } 
    }

    // Functional
    public override void Rotate() {
        //Debug.Log("Rotating");
        float rotatingSpeed = argsDict["rotation proportion"] * thisAnimal.GetPhenotype().GetTrait("max_rotation");
        thisBody.globalPos.Rotate(0, rotatingSpeed, 0, Space.World);
    }

    // Functional
    public override void TakeSteps() {
        Debug.Log("Walking");
        float stepProportion = argsDict["step proportion"] * thisAnimal.GetPhenotype().GetTrait("max_step");
        thisBody.TranslateBodyTo(thisBody.globalPos.forward * stepProportion);
    }

    // Untested
    public override void PickUp() {
        Debug.Log("Tried to pick something up");

        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (argsDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        } 

        Vector3 holdPos = thisBody.GetHolderCoords(argsDict["held position"]);
        Vector3 itemPos = GetTargetPos();

        if (itemPos.y > holdPos.y) {
            if (ArmTo(true, itemPos)) {
                
                FixItem(holder);
                DropArm(true);
            }
        } else {
            Kneel();
        }
    }

    // Untested
    public override void SetDown() {
        Debug.Log("Tried to set something down");

        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (argsDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        }

        if (CheckSitting()) {
            RemoveFromHolder(holder);
        } else {
            Kneel();
        }
    }
    
    // Nonfunctional - no ArmTo anymore
    public override void Consume () {
        Debug.Log("Tried to eat something");
        
        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (argsDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        }

        if (ArmTo(true, thisBody.head.transform.position)) {
            thisBody.EatObject(holder);
            DropArm();
        }
    }

    // Functional
    public override void WakeUp() {
        Debug.Log("Waking up");
        
        //thisAnimal.ToggleBodyPart("Eye_L", true);
        //thisAnimal.ToggleBodyPart("Eye_R", true);
        
        thisBody.SetState("sleeping", false);  
    }

    // Nonfunctional - no sleep adjustment
    public override void Sleep() {
        Debug.Log("Sleeping");

        //thisAnimal.ToggleBodyPart("Eye_L", false);
        //thisAnimal.ToggleBodyPart("Eye_R", false);
        Collapse();

        thisBody.SetState("sleeping", true);
        thisBody.SleepAdjust();
    }
    
    // Functional
    public override void Rest() {
        Debug.Log("Resting");
        thisBody.RestAdjust();
    }

    // Functional (but hated)
    public override void LookAt() {
        Quaternion toSend = Quaternion.LookRotation(thisBody.globalPos.forward);
        thisBody.RotateJointTo("Head", toSend);
    }

    // Functional
    private void BendKnees(float degree, bool passedGoal) {
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
    private void BendLegs(float xDegree, float yDegree, bool passedGoal) {
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
    private bool ArmToGoal() {
        string arm = "Hand_L";
        if (argsDict["active right"] == 1f) {
            arm = "Hand_R";
        }

        float xTrans = argsDict["reach proportion x"] * thisBody.xMax;
        float yTrans = argsDict["reach proportion y"] * thisBody.yMax;
        float zTrans = argsDict["reach proportion z"] * thisBody.zMax;
        Vector3 netTrans = new Vector3(xTrans, yTrans, zTrans);

        thisBody.EnsureKinematic(arm);
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        Vector3 targetPos = globalPos.position + netTrans;
        armTrans.position = Vector3.Slerp(armTrans.position, targetPos, Time.deltaTime);

        return (armTrans.position == targetPos);
    }

    // Functional
    private void DropArm() {
        string arm = "Hand_L";
        if (argsDict["active right"] == 1f) {
            arm = "Hand_R";
        }

        thisBody.DisableKinematic(arm);
    }

    // Functional
    private void Kneel(){
        Vector3 toSend = abdomenTrans.position;
        if (toSend.y > thisBody.GetHeight()/2 + 0.5) {

            BendLegs(30f, 0f, false);
            BendKnees(-45f, false);

            ArmUp(true, 2.5f); 
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));
        } else {
            Debug.Log("I think I'm kneeling");
        }
    }

    // Functional
    private void Crouch(){
        Vector3 toSend = abdomenTrans.position;
        double crouchHeight = thisBody.GetHeight()/1.5 + 0.5;
        if (toSend.y > crouchHeight) {

            BendLegs(80f, 0f, false);
            BendKnees(-30f, false);
            
            Vector3 newVec = new Vector3(1f, 0.5f, 0.25f);
            ArmTo(true, newVec);
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));
            
        } else {
            Debug.Log("I think I'm squatting");
        }
    }

    // Functional
    private void Collapse() {
        Debug.Log("Collapsing");
        thisBody.DisableKinematic("Abdomen");
    }

    // Untested
    private void GrabItem() {
        Debug.Log("Tried to affix something");

        GameObject holder = thisBody.GetSkeleton("Hand_L");
        if (argsDict["active right"] == 1f) {
            holder = thisBody.GetSkeleton("Hand_R");
        }

        Vector3 forCollider = holder.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(forCollider, .025f);
        Rigidbody toConnect = holder.GetComponent<Rigidbody>();
        
        foreach(var hit in hitColliders) {
            if(!thisBody.GetSkeletonDict().ContainsKey(hit.gameObject.name)) {
                
                FixedJoint newJoint = hit.gameObject.AddComponent<FixedJoint>() as FixedJoint;
                newJoint.connectedBody = toConnect;
            }
        }
    }

    // Untested
    private void RemoveFromHolder(GameObject holder) {
        if (thisBody.GetHoldings()[holder.name] != null) {
                thisBody.RemoveObject(holder.name);
        }
    }
}


