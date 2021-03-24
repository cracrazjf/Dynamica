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
    
    public override void StandUp() {
        Vector3 toSend = abdomenTrans.position;

        if (toSend.y < thisBody.GetHeight()) {
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime));
            
        } else {
            //ArmUp(true, .025f); 
            Debug.Log("I think I'm standing");
        } 
    }

    public override void Rotate() {
        //Debug.Log("Rotating");
        float rotatingSpeed = argsDict["rotation proportion"] * thisAnimal.GetPhenotype().GetTrait("max_rotation");
        thisBody.globalPos.Rotate(0, rotatingSpeed, 0, Space.World);
    }

    
    public override void TakeSteps() {
        Debug.Log("Walking");
        float stepProportion = argsDict["step proportion"] * thisAnimal.GetPhenotype().GetTrait("max_step");
        thisBody.TranslateBodyTo(thisBody.globalPos.forward * stepProportion);
    }

    public override void PickUp() {
        Debug.Log("Tried to pick something up");
        Vector3 holdPos = thisBody.GetHolderCoords(argsDict["held position"]);
        Vector3 itemPos = GetTargetPos();

        if (itemPos.y > holdPos.y) {
            if (ArmTo(true, itemPos)) {
                GameObject holder = thisBody.GetHolder((int) argsDict["held position"]);
                FixItem(holder);
                DropArm(true);
            }
        } else {
            Kneel();
        }
    }

    public override void SetDown() {
        Debug.Log("Tried to set something down");
        int index = (int) argsDict["held position"];
        Vector3 heldPos = thisBody.GetHolderCoords(index);

        Kneel();
        if(thisBody.GetHoldings()[index] != null) {
            thisBody.RemoveObject(index);
        }
    }
    
    public override void Consume () {
        Debug.Log("Tried to eat something");
        Vector3 heldPos = thisBody.GetHolderCoords(argsDict["held position"]);

        if (ArmTo(true, thisBody.mouth.transform.position)) {
            thisBody.EatObject((int) argsDict["held position"]);
            DropArm(true);
        }
        
    }

    public override void WakeUp() {
        Debug.Log("Waking up");
        
        //thisAnimal.ToggleBodyPart("Eye_L", true);
        //thisAnimal.ToggleBodyPart("Eye_R", true);
        
        thisBody.SetState("sleeping", false);  
    }

    public override void Sleep() {
        Debug.Log("Sleeping");

        //thisAnimal.ToggleBodyPart("Eye_L", false);
        //thisAnimal.ToggleBodyPart("Eye_R", false);
        Collapse();

        thisBody.SetState("sleeping", true);
        thisBody.SleepAdjust();
        Debug.Log("Got here");  
    }
    
    public override void Rest() {
        Debug.Log("Resting");
        thisBody.RestAdjust();
    }

    public override void LookAt() {
        Quaternion toSend = Quaternion.LookRotation(thisBody.globalPos.forward);
        thisBody.RotateJointTo("Head", toSend);
    }

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

    private void BendWaist(float degree, bool passedGoal) {
        Debug.Log("Waist bend imminent?");
        Quaternion toSend = new Quaternion(degree, 0, 0, 1);

        thisBody.RotateJointBy("Abdomen", toSend);
        
    }

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

    private void ArmUp(bool rightSide, float targetHeight) {
        string arm = "Hand_L";
        if (rightSide) {
            arm = "Hand_R";
        }
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        float handHeight = armTrans.position.y;

        if (handHeight < targetHeight) {
            Debug.Log("reaching with " + arm);
            thisBody.EnsureKinematic(arm);
            armTrans.Translate(Vector3.up * (Time.deltaTime * 1));
        }
    }

    
    private void ArmForward(bool rightSide, float targetDistance) {
        string arm = "Hand_L";
        if (rightSide) {
            arm = "Hand_R";
        }
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        float handHeight = armTrans.position.x;

        if (handHeight < targetDistance) {
            Debug.Log("reaching with " + arm);
            thisBody.EnsureKinematic(arm);
            armTrans.Translate(Vector3.forward * (Time.deltaTime * 1));
        }
    }

    private bool ArmTo(bool rightSide, Vector3 translation) {
        string arm = "Hand_L";
        if (rightSide) {
            arm = "Hand_R";
        }
        thisBody.EnsureKinematic(arm);
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        Vector3 targetPos = armTrans.position + translation;
        armTrans.position = Vector3.Slerp(armTrans.position, targetPos, Time.deltaTime);
        return (armTrans.position == targetPos);
    }

    private void DropArm(bool rightSide) {
        string arm = "Hand_L";
        if (rightSide) {
            arm = "Hand_R";
        }
        thisBody.DisableKinematic(arm);
    }

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

    private void Collapse() {
        Debug.Log("Collapsing");
        thisBody.DisableKinematic("Abdomen");
    }

    private void FixItem(GameObject holder) {
        Debug.Log("Tried to affix something");
        Vector3 forCollider = new Vector3 (argsDict["target x"], argsDict["target y"], argsDict["target z"]);
        Collider[] hitColliders = Physics.OverlapSphere(forCollider, .025f);
        Rigidbody toConnect = holder.GetComponent<Rigidbody>();
        
        foreach(var hit in hitColliders) {
            if(!thisBody.GetSkeletonDict().ContainsKey(hit.gameObject.name)) {
                
                FixedJoint newJoint = hit.gameObject.AddComponent<FixedJoint>() as FixedJoint;
                newJoint.connectedBody = toConnect;
            }
        }
    }
}


