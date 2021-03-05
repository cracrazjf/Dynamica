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
        thisBody.GetSkeletonDict()["Abdomen"].GetComponent<Rigidbody>().isKinematic = true;
        BendWaist(0.5f, 1f);
        BendKnees(0.5f);

        thisBody.SetState("standing", false);
        thisBody.SetState("sitting", true);
    }

    public override void SitUp() {
        Debug.Log("SitUp was called");
        thisBody.GetSkeletonDict()["Abdomen"].GetComponent<Rigidbody>().isKinematic = true;
        BendWaist(-0.5f, 1f);
        BendKnees(-0.5f);

        thisBody.SetState("laying", false);
        thisBody.SetState("sitting", true);          
    }

    public override void LayDown() {
        Debug.Log("Tried to lay down");
        BendWaist(0f, 0f);

        if (thisAnimal.GetBodyState("sitting") || thisAnimal.GetBodyState("laying")) {
            thisBody.GetSkeletonDict()["Abdomen"].GetComponent<Rigidbody>().isKinematic = false;
        } else {
            SitDown();
        }
        thisBody.SetState("sitting", false);
        thisBody.SetState("laying", true);  
    }
    
    public override void StandUp(){
        Debug.Log("StandUp was called");

        Vector3 goalPosition = (thisBody.GetXZPosition() + new Vector3(0, thisBody.GetHeight(), 0));
        thisBody.TranslateSkeletonTo("Abdomen", goalPosition);

        thisBody.SetState("laying", false);
        thisBody.SetState("sitting", false);
        thisBody.SetState("standing", true);  
    }

    public override void Rotate() {
        float rotatingSpeed = argsDict["rotation velocity"];
        thisBody.globalPos.Rotate(0, rotatingSpeed, 0, Space.World);
    }

    
    public override void TakeSteps() {
        float stepProportion = argsDict["step rate"];
        thisBody.globalPos.Translate(thisBody.globalPos.forward * stepProportion * Time.deltaTime, Space.World);
    }

    public override void PickUp() {
        Debug.Log("Tried to pick something up");

        if (argsDict["target y"] > thisBody.globalPos.position.y) {
            // Reach up
        } else {
            //Reach down
        }

        Vector3 heldPos = thisBody.GetHolderCoords(argsDict["held position"]);
    }

    public override void SetDown() {
        Debug.Log("Tried to set something down");
        Vector3 heldPos = thisBody.GetHolderCoords(argsDict["held position"]);
    }

    
    public override void Consume () {
        Debug.Log("Tried to eat something");
        Vector3 heldPos = thisBody.GetHolderCoords(argsDict["held position"]);

        if (heldPos.y > thisBody.globalPos.position.y) {
            // Reach up
        } else {
            //Reach down
        }
        // Stat buffs and destroy the comsumed object
    }

    public override void WakeUp() {
        Debug.Log("Waking up");
        
        thisAnimal.ToggleBodyPart("Eye_L", true);
        thisAnimal.ToggleBodyPart("Eye_R", true);
        
        thisBody.SetState("sleeping", false);  
    }

    public override void Sleep() {
        Debug.Log("Sleeping");

        thisAnimal.ToggleBodyPart("Eye_L", false);
        thisAnimal.ToggleBodyPart("Eye_R", false);

        thisBody.SetState("sleeping", true);  
    }
    
    public override void Rest() {
        Debug.Log("Resting");
    }

    public override void LookAt() {
        Debug.Log("Something caught my eye");
    }

    public void BendKnees(float degree) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);
        thisBody.RotateJoint("Tibia_L", toSend);
        thisBody.RotateJoint("Tibia_R", toSend);
    }

    public void BendWaist(float degree, float z) {
        Quaternion toSend = new Quaternion(degree, 0, 0, z);
        thisBody.RotateJoint("Hip_L", toSend);
        thisBody.RotateJoint("Hip_R", toSend);
    }
}


