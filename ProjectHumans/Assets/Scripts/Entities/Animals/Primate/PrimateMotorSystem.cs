using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrimateMotorSystem : MotorSystem {

    public PrimateMotorSystem(Animal animal) : base(animal) {
        Debug.Log("An ape was born!");
    }
    
    public override void SitDown() {
        Debug.Log("SitDown was called");
        Crouch();

        thisBody.SetState("standing", false);
        thisBody.SetState("sitting", true);
    }

    public override void SitUp() {
        Debug.Log("SitUp was called");
        if(thisBody.GetState("sitting")) { 
            LockLegs(); 
        } 

        StandUp();

        thisBody.SetState("laying", false);
        thisBody.SetState("sitting", true);          
    }

    public override void LayDown() {
        Debug.Log("Tried to lay down");
        //BendWaist(0f, 0f);

        if (thisAnimal.GetBodyState("sitting") || thisAnimal.GetBodyState("laying")) {
            Collapse();
        } else {
            SitDown();
        }
        thisBody.SetState("sitting", false);
        thisBody.SetState("laying", true);  
    }
    
    public override void StandUp(){
        //Debug.Log("StandUp was called");

        Vector3 goalPosition = (thisBody.GetXZPosition() + new Vector3(0, thisBody.GetHeight(), 0));
        thisBody.EnsureKinematic("Abdomen");
        thisBody.TranslateSkeletonTo("Abdomen", goalPosition);

        thisBody.SetState("laying", false);
        thisBody.SetState("sitting", false);
        thisBody.SetState("standing", true);  
    }

    public override void Rotate() {
        //Debug.Log("Rotating");
        float rotatingSpeed = argsDict["rotation proportion"] * thisAnimal.GetPhenotype().GetTrait("max_rotation");
        thisBody.globalPos.Rotate(0, rotatingSpeed, 0, Space.World);
    }

    
    public override void TakeSteps() {
        Debug.Log("Walking");
        float stepProportion = argsDict["step proportion"] * thisAnimal.GetPhenotype().GetTrait("max_step");
        thisBody.globalPos.Translate(thisBody.globalPos.forward * stepProportion);
    }

    public override void PickUp() {
        Debug.Log("Tried to pick something up");
        Vector3 heldPos = thisBody.GetHolderCoords(argsDict["held position"]);

        if (argsDict["target y"] > heldPos.y) {
            // Reach up
        } else {
            //Reach down
        }
        GameObject holder = thisBody.GetHolder((int) argsDict["held position"]);
        FixItem(holder);
    }

    public override void SetDown() {
        Debug.Log("Tried to set something down");
        int index = (int) argsDict["held position"];
        Vector3 heldPos = thisBody.GetHolderCoords(index);

        //movement
        if(thisBody.GetHoldings()[index] != null) {
            //thisBody.RemoveObject(index);
        }
    }
    
    public override void Consume () {
        Debug.Log("Tried to eat something");
        Vector3 heldPos = thisBody.GetHolderCoords(argsDict["held position"]);

        if (heldPos.y > thisBody.globalPos.position.y) {
            // Reach up
        } else {
            //Reach down
        }
        thisBody.EatObject((int) argsDict["held position"]);
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
        thisBody.RotateJoint("Head", toSend);
    }

    private void BendKnees(float degree) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);
        thisBody.RotateJoint("Tibia_L", toSend);
        thisBody.RotateJoint("Tibia_R", toSend);
    }

    private void BendWaist(float degree) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);
        thisBody.DisableKinematic("Abdomen");
        thisBody.RotateJoint("Hips", toSend);
        
    }

    private void LegUp(float degree) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);
        thisBody.RotateJoint("Femur_L", toSend);
        thisBody.RotateJoint("Femur_R", toSend);
    }

    private void LockLegs() {
        //thisBody.ToggleKinematic("Femur_R");
        thisBody.ToggleKinematic("Tibia_R");
        //thisBody.ToggleKinematic("Femur_L");
        thisBody.ToggleKinematic("Tibia_L");
    }

    private void Crouch(){
        Vector3 toSend = thisBody.GetSkeletonDict()["Abdomen"].transform.position;
        if (toSend.y > thisBody.GetHeight()/2) {
            BendWaist(-30f);
            LegUp(60f);
            BendKnees(-45f);
            float crouchHeight = thisBody.GetHeight()/3;
            toSend.y = crouchHeight;

            thisBody.TranslateSkeletonTo("Abdomen", toSend);
        } else {
            thisBody.EnsureKinematic("Abdomen");
        }
    }

    private void Collapse() {
        Debug.Log("Tried to collapse");
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


