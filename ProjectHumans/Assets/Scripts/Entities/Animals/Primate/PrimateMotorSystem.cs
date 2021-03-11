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
        if (thisBody.GetState("laying")) {
            //thisBody.DisableKinematic("Abdomen");
            thisBody.RotateJointTo("Hips", new Quaternion(1,0,0,0));
        } else {
            //hisBody.DisableKinematic("Abdomen");
            thisBody.RotateJointTo("Hips", new Quaternion(0,0,0,0)); 
        }
        thisBody.SetState("sitting", true);          
    }

    public override void LayDown() {
        Debug.Log("Tried to lay down");

        if (thisAnimal.GetBodyState("sitting") || thisAnimal.GetBodyState("laying")) {
            Collapse();
            thisBody.SetState("laying", true);  
            thisBody.SetState("sitting", false);
        } else {
            SitDown();
        }  
    }
    
    //doesnt work later on because the body got to move more than the abdomen? so nowhere for ab to go?
    public override void StandUp() {
        Vector3 toSend = thisBody.globalPos.position;

        if (toSend.y < thisBody.GetHeight()) {
            Vector3 localHold = thisBody.abdomen.transform.localPosition;

            // These cause unity to crash
            // BendLegs(0f, true);
            // BendKnees(0f, true);
            // BendWaist(0f, true);

            thisBody.globalPos.Translate(Vector3.up * (Time.deltaTime));
            thisBody.abdomen.transform.localPosition = localHold;
            
        } else {
            Debug.Log("I think I'm standing");
        }
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
        thisBody.TranslateBodyTo(thisBody.globalPos.forward * stepProportion);
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
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);

        if(passedGoal) {
            thisBody.RotateJointTo("Hips", toSend);
        } else {
            thisBody.RotateJointBy("Hips", toSend);
        }
    }

    private void BendLegs(float degree, bool passedGoal) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);

        if(passedGoal) {
            thisBody.RotateJointTo("Femur_L", toSend);
            thisBody.RotateJointTo("Femur_R", toSend);
        } else {
            thisBody.RotateJointBy("Femur_L", toSend);
            thisBody.RotateJointBy("Femur_R", toSend);
        }
    }

    private void Crouch(){
        Vector3 toSend = thisBody.globalPos.position;
        if (toSend.y > thisBody.GetHeight()/2 + 0.5) {
            Vector3 localHold = thisBody.abdomen.transform.localPosition;

            BendLegs(60f, false);
            BendKnees(-45f, false);
            BendWaist(-30f, false);

            thisBody.globalPos.Translate(Vector3.up * (Time.deltaTime * -1));
            thisBody.abdomen.transform.localPosition = localHold;
            
        } else {
            Debug.Log("I think I'm sitting");
        }
    }

    private void Collapse() {
        Debug.Log("Collapsing");
        thisBody.DisableKinematic("Abdomen");
        Vector3 globalAdjust = new Vector3(0f, thisBody.displacement, 0f);
        thisBody.globalPos.transform.position = thisBody.abdomen.transform.position + globalAdjust;
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


