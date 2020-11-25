using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class HumanMotorSystem : MotorSystem
{
    private Human thisHuman;

    public HumanMotorSystem(Human human) : base(human) {

        this.thisHuman = human;
        // for (int i = 0; i < actionLabelList.Count; i++){
        //     actionIndexDict.Add(actionLabelList[i], i);
        //     actionDisplayDict.Add(actionLabelList[i], true);
        // }
        
        velocity = float.Parse(thisHuman.phenotype.traitDict["current_velocity"]);
        maxStepDistance = float.Parse(thisHuman.phenotype.traitDict["max_step_distance"]);
        maxRotationSpeed = float.Parse(thisHuman.phenotype.traitDict["max_rotation_speed"]);

    }


    public override void takeSteps(float stepProportion) {
        // needs to 
        //      bodyState = "standing"
        //      actionState = "none"
        //      sleeping = false

        //      take the steps
        //      update any appropriate body state variables

        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && ((this.thisHuman.actionState =="none") || (this.thisHuman.actionState =="taking steps"))) {
            velocity = (stepProportion * maxStepDistance);
            if (stepProportion != 0) {
                var step = stepProportion * maxStepDistance;
                thisHuman.gameObject.transform.Translate(Vector3.forward *step*Time.deltaTime);
                this.thisHuman.animator.SetBool("moving", true);
                this.thisHuman.animator.SetFloat("Velocity", 0);
                this.thisHuman.actionState = "taking steps";
            }
            else {
                this.thisHuman.animator.SetBool("moving", false);
                this.thisHuman.actionState = "none";
            }
        }
    }

    public override void rotate(float rotationAngle){
        
        float RA = rotationAngle * maxRotationSpeed;
        float rotation =RA * Time.deltaTime;
        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && ((this.thisHuman.actionState =="none") || (this.thisHuman.actionState =="rotating"))) {
            if (rotateAngle > rotation) {
                rotateAngle -= rotation;
            }
            else {
                rotation = rotateAngle;
                rotateAngle = 0;
            }
            this.thisHuman.gameObject.transform.Rotate(0,rotation,0);
            this.thisHuman.animator.SetBool("rotate", true);
            if (rotateAngle == 0) {
                this.thisHuman.animator.SetBool("rotate", false);
            }
        }
    }

    public override void drink() {
        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetTrigger("Drink");
            thisHuman.GetDriveSystem().stateDict["thirst"] = 0;
            this.thisHuman.actionState = "drinking";
        }
    }
    
    public override void sitDown(){
        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetBool("sit", true);
            this.thisHuman.actionState = "sitting down";
            this.thisHuman.animator.SetBool("keepsitting", true);
        }
    }

    public override void sitUp(){
        if ((this.thisHuman.bodyState == "laying") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetBool("sleep", false);
            this.thisHuman.actionState = "sitting up";
            this.thisHuman.animator.SetBool("keepsitting", true);
        }
    }
        
    public override void standUp(){
        if ((this.thisHuman.bodyState == "sitting") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
             this.thisHuman.animator.SetBool("sit", false);
            this.thisHuman.actionState = "standing up";
        }
       
    }

    public override void layDown(){
        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetBool("sit", true);
            this.thisHuman.animator.SetBool("sleep", true);
        }
    }

    public override void sleep(){
        Debug.Log("here");
        if ((this.thisHuman.bodyState == "laying") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.leftEye.localScale = new Vector3(1,0.09f,2);
            this.thisHuman.rightEye.localScale = new Vector3(1,0.09f,2);
            this.thisHuman.sleepingState = true;
        }
        
    }
    
    public override void wakeUp(){
        if ((this.thisHuman.bodyState == "laying") && (this.thisHuman.sleepingState == true) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.leftEye.localScale = new Vector3(1,1,1);
            this.thisHuman.rightEye.localScale = new Vector3(1,1,1);
            this.thisHuman.sleepingState = false;
        }
    }
    
    public override void pickUp(float hand){
        Collider[] pickableObj = new Collider[5];

        if (this.thisHuman.sleepingState == false){
            if (this.thisHuman.bodyState == "standing"){
                if ((this.thisHuman.actionState =="none") || (this.thisHuman.actionState =="picking up with left hand") || (this.thisHuman.actionState =="picking up with right hand")){
                    int pickUpHand = -1;
                    int numObj = -1;
                    if ((hand == 0) && (this.thisHuman.LHState == false)) {
                        pickUpHand = 0;
                        numObj = Physics.OverlapSphereNonAlloc(this.thisHuman.leftHand.position,0.2f,pickableObj);
                    }
                    else if ((hand == 1) && (this.thisHuman.RHState == false)){
                        pickUpHand = 1;
                        numObj = Physics.OverlapSphereNonAlloc(this.thisHuman.rightHand.position,0.2f,pickableObj);
                    }

                    if (pickUpHand != -1){
                        this.thisHuman.animator.ResetTrigger("set");
                        this.thisHuman.animator.SetTrigger("pick");
                        this.thisHuman.animator.SetFloat("PickupL/R",pickUpHand);
                        for(int i = 0; i < numObj; i++) {
                            if (!pickableObj[i].CompareTag("Human") && !pickableObj[i].CompareTag("ground")) {
                                pickableObj[i].GetComponent<Rigidbody>().isKinematic = true;
                                pickableObj[i].GetComponent<Rigidbody>().useGravity = false;
                                if (pickUpHand == 0){
                                    pickableObj[i].transform.parent = this.thisHuman.leftHand.transform;
                                    this.thisHuman.LHState = true;
                                    this.thisHuman.animator.ResetTrigger("pick");
                                }
                                else{
                                    pickableObj[i].transform.parent = this.thisHuman.rightHand.transform;
                                    this.thisHuman.RHState = true;
                                    this.thisHuman.animator.ResetTrigger("pick");
                                }
                                pickableObj[i].transform.localPosition = new Vector3(0,0,0);
                            }
                        }
                    }
                    else{
                        Debug.Log("can't pick up because hand is full");
                    }
                    
                }
                else{
                    Debug.Log("can't pick up because another action is happening");
                }
            }
            else{
                Debug.Log("can't pick up because not standing");
            }
        } 
        else {
            Debug.Log("can't pick up because sleeping");
        }  
    }

    public override void setDown(float hand){

        if ((hand == 0) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.LHState == true) && ((this.thisHuman.actionState == "none") || (this.thisHuman.actionState == "setting down with left hand"))) {
            this.thisHuman.animator.ResetTrigger("pick");
            this.thisHuman.animator.SetTrigger("set");
            this.thisHuman.animator.SetFloat("SetdownL/R",0);
            if (thisHuman.leftHand.GetChild(0).transform.position.y < 0.1) {
                thisHuman.leftHand.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                thisHuman.leftHand.GetChild(0).transform.parent = null;
                if(this.thisHuman.leftHand.childCount == 0) {
                    this.thisHuman.LHState = false;
                    this.thisHuman.animator.ResetTrigger("set");
                }
            }
        }
            
        else if ((hand == 1) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.RHState == true) && ((this.thisHuman.actionState == "none") || (this.thisHuman.actionState == "setting down with right hand"))) {
            this.thisHuman.animator.ResetTrigger("pick");
            this.thisHuman.animator.SetTrigger("set");
            this.thisHuman.animator.SetFloat("Setdown/R",1);
            if (thisHuman.rightHand.GetChild(0).transform.position.y < 0.1) {
                thisHuman.rightHand.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                thisHuman.rightHand.GetChild(0).transform.parent = null;
                if(this.thisHuman.rightHand.childCount == 0) {
                    this.thisHuman.RHState = false;
                    this.thisHuman.animator.ResetTrigger("set");
                }
            }
        }

        else {
            //Debug.Log("can't set down");
        }
    }

    public override void eat(float hand) {
        if ((hand == 0) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.LHState == true) && ((this.thisHuman.actionState == "none") || (this.thisHuman.actionState == "eating with left hand"))) {
            this.thisHuman.animator.SetTrigger("eat");
            this.thisHuman.animator.SetFloat("EatL/R",0);
            thisHuman.GetDriveSystem().stateDict["hunger"] = 0;
            UnityEngine.Object.Destroy(thisHuman.leftHand.GetChild(0).gameObject, 5);
            this.thisHuman.LHState = false;
        }
        else if ((hand == 1) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.RHState == true) && ((this.thisHuman.actionState == "none") || (this.thisHuman.actionState == "eating with right hand"))) {
            this.thisHuman.animator.SetTrigger("eat");
            this.thisHuman.animator.SetFloat("EatL/R",1);
            thisHuman.GetDriveSystem().stateDict["hunger"] = 0;
            UnityEngine.Object.Destroy(thisHuman.rightHand.GetChild(0).gameObject, 5);
            this.thisHuman.RHState = false;
        }
        else {
            //Debug.Log("can't eat");
        }
    }
}



