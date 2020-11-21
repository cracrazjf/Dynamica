using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class HumanMotorSystem
{
    
    public Animal thisAnimal; // we need this if we want to access drive system
    public LivingObject thisLivingObject; // we need this if we want to access genome or phenotype
    public Human thisHuman; // we need this if we want to access 
    public float rotateAngle = 360;
    public float maxStepDistance;
    public float max_rotation_speed;
    public float velocity;

    public List<string> actionLabelList = new List<string>{ "take_step",   // value -1..1 translating to speed of accel./deccel.
                                                    "rotate",       // value from -1 to 1, translating into -180..180 degree rotation
                                                    "sit_up",          // begin to sit up
                                                    "sit_down",          // begin to sit down
                                                    "stand_up",        // begin to stand
                                                    "lay_down",          // begin to lay down
                                                    "sleep",        // begin to sleep
                                                    "wake_up",      // begin to wake
                                                    "pick_up", 
                                                    "set_down",
                                                    "drink",
                                                    "eat"
                                                    };  // do we want rest to be explicit, or just not doing anything else

    // 

    public Dictionary<string, List<List<string>>> actionRequirementsDict = new Dictionary<string, List<List<string>>>();
    // these lists contain states from the human nervous system bodyStateInputLabelList
    /* 
        body states: standing, sitting, laying, holding LH, holding RH
        sleeping states: asleep, awake
        action states: eating, drinking, picking up, setting down, stepping, rotating, sitting up, sitting down, standing up, laying down
        
        rotate: standing
        eat: standing sitting


    */


    public int numActions; // 12
    
    public Dictionary<string, int> actionIndexDict = new Dictionary<string, int>();
    public List<float> actionValueList = new List<float>(); // [1 0 0 0 0 0 1 0 0 0 0 1 0]
    public Dictionary<string, bool> actionDisplayDict = new Dictionary<string, bool>();
    public Transform Eye_L;
    public Transform Eye_R;

    public int sittingDownIndex;
    public int standingUpIndex;
    public int layingDownIndex;
    public int holdingLHIndex;
    public int holdingRHIndex;

    public bool sleeping = false;

    public HumanMotorSystem(Human human) {
        this.thisHuman = human;
        for (int i = 0; i < actionLabelList.Count; i++){
            actionIndexDict.Add(actionLabelList[i], i);
            actionDisplayDict.Add(actionLabelList[i], true);
        }
        
        velocity = float.Parse(thisHuman.phenotype.traitDict["current_velocity"]);
        maxStepDistance = float.Parse(thisHuman.phenotype.traitDict["maxStepDistance"]);
        max_rotation_speed = float.Parse(thisHuman.phenotype.traitDict["max_rotation_speed"]);


    }

    public void takeAction(HumanActionChoice actionChoice){
        // find the highest value in actionValueList, and call the function for that action
        // if action_array[0] == 1:
        //      sit()
        // else if action_array[1] == 1:
        //      stand_up()
        // else if action_array[2] == 1:
        //      eat(action_argument[2])

        if (actionChoice.actionValueDict["sit_down"] == 1){
            sitDown();
        }
        if (actionChoice.actionValueDict["sit_up"] == 1) {
            sitUp();
        }
        else if (actionChoice.actionValueDict["stand_up"] == 1){
            standUp();
        }
        else if (actionChoice.actionValueDict["lay_down"] == 1){
            layDown();
        }
        else if (actionChoice.actionValueDict["sleep"] == 1){
            sleep();
        }
        else if (actionChoice.actionValueDict["wake_up"] == 1){
            wakeUp();
        }
        else if (actionChoice.actionValueDict["take_step"] == 1){
            takeSteps(actionChoice.argumentDict["stepRate"]);
        }
        else if (actionChoice.actionValueDict["rotate"] == 1){
            rotate(actionChoice.argumentDict["rotationAngle"]);
        }
        else if (actionChoice.actionValueDict["pick_up"] == 1){
            pickUp(actionChoice.argumentDict["hand"]);
        }
        else if (actionChoice.actionValueDict["set_down"] == 1){
            setDown(actionChoice.argumentDict["hand"]);
        }
        else if (actionChoice.actionValueDict["eat"] == 1){
            eat(actionChoice.argumentDict["hand"]);
        }
        else if (actionChoice.actionValueDict["drink"] == 1) {
            drink();
        }
    }
// "take_steps",  
// "rotate",       
// "sit_up",         
// "sit_down",         
// "stand_up",        
// "lay_down",         
// "sleep",        
// "wake_up",      
// "pick_up", 
// "set_down",
// "put_in",
// "get_from",
// "drink",
// "eat"


// "standing", 
// "sitting", 
// "laying", 

// "awake", 1
// "asleep",

// "holding LH",
// "holding RH", 

// "sitting down", 
// "sitting up", 
// "laying down",
// "standing up", 
// "rotating", 
// "taking steps",
// "picking up", 
// "setting down",
// "eating", 
// "drinking"};


// bodyPositionState = ""
// sleepingState = 
// LHState = ""
// RHState = ""
// actionState = ""


    public void takeSteps(float stepProportion) {
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

    public void rotate(float rotationAngle){
        
        float RA = rotationAngle * max_rotation_speed;
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

    public void drink() {
        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetTrigger("Drink");
            thisHuman.driveSystem.stateDict["thirst"] = 0;
            this.thisHuman.actionState = "drinking";
        }
    }
    
    public void sitDown(){
        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetBool("sit", true);
            this.thisHuman.actionState = "sitting down";
            this.thisHuman.animator.SetBool("keepsitting", true);
        }
    }

    public void sitUp(){
        if ((this.thisHuman.bodyState == "laying") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetBool("sleep", false);
            this.thisHuman.actionState = "sitting up";
            this.thisHuman.animator.SetBool("keepsitting", true);
        }
    }
        
    public void standUp(){
        if ((this.thisHuman.bodyState == "sitting") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
             this.thisHuman.animator.SetBool("sit", false);
            this.thisHuman.actionState = "standing up";
        }
       
    }

    public void layDown(){
        if ((this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.animator.SetBool("sit", true);
            this.thisHuman.animator.SetBool("sleep", true);
        }
    }

    public void sleep(){
        Debug.Log("here");
        if ((this.thisHuman.bodyState == "laying") && (this.thisHuman.sleepingState == false) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.leftEye.localScale = new Vector3(1,0.09f,2);
            this.thisHuman.rightEye.localScale = new Vector3(1,0.09f,2);
            this.thisHuman.sleepingState = true;
        }
        
    }
    
    public void wakeUp(){
        if ((this.thisHuman.bodyState == "laying") && (this.thisHuman.sleepingState == true) && (this.thisHuman.actionState =="none")) {
            this.thisHuman.leftEye.localScale = new Vector3(1,1,1);
            this.thisHuman.rightEye.localScale = new Vector3(1,1,1);
            this.thisHuman.sleepingState = false;
        }
    }
    
    public void pickUp(float hand){
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
                            if (!pickableObj[i].CompareTag("human") && !pickableObj[i].CompareTag("ground")) {
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
    // if ((hand == 0) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.LHState == false)) {
        //     int numObj = Physics.OverlapSphereNonAlloc(this.thisHuman.Hand_L.position,0.2f,pickableObj);
        //     for(int i = 0; i < numObj; i++) {
        //         if (!pickableObj[i].CompareTag("human") && !pickableObj[i].CompareTag("ground")) {
        //             pickableObj[i].GetComponent<Rigidbody>().isKinematic = true;
        //             pickableObj[i].GetComponent<Rigidbody>().useGravity = false;
        //             pickableObj[i].transform.parent = this.thisHuman.Hand_L.transform;
        //             pickableObj[i].transform.localPosition = new Vector3(0,0,0);
        //             this.thisHuman.LHState = true;
        //         }
        //     }
        //     this.thisHuman.animator.SetBool("pickup", true);
        //     this.thisHuman.animator.SetFloat("PickupL/R",1);
        //     this.thisHuman.actionState = "picking up with left hand";
        // }
            
        // else if ((hand == 1) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.RHState == false)) {

        //     int numObj = Physics.OverlapSphereNonAlloc(this.thisHuman.Hand_R.position,0.2f,pickableObj);
        //     for(int i = 0; i < numObj; i++) {
        //         if (!pickableObj[i].CompareTag("human") && !pickableObj[i].CompareTag("ground")) {
        //             pickableObj[i].GetComponent<Rigidbody>().isKinematic = true;
        //             pickableObj[i].transform.parent = this.thisHuman.Hand_R.transform;
        //             pickableObj[i].transform.localPosition = new Vector3(0,0,0);
        //             this.thisHuman.RHState = true;
        //         }
        //     }
        //     this.thisHuman.rigidbody.isKinematic = true;
        //     this.thisHuman.animator.SetBool("pickup", true);
        //     this.thisHuman.animator.SetFloat("PickupL/R",0);
        //     this.thisHuman.actionState = "picking up with right hand";
        // }
        // else {
        //     this.thisHuman.animator.SetBool("pickup", false);
        //     Debug.Log("can't pick");
        // }

    public void setDown(float hand){

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

    public void eat(float hand) {
        if ((hand == 0) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.LHState == true) && ((this.thisHuman.actionState == "none") || (this.thisHuman.actionState == "eating with left hand"))) {
            this.thisHuman.animator.SetTrigger("eat");
            this.thisHuman.animator.SetFloat("EatL/R",0);
            thisHuman.driveSystem.stateDict["hunger"] = 0;
            UnityEngine.Object.Destroy(thisHuman.leftHand.GetChild(0).gameObject, 5);
            this.thisHuman.LHState = false;
        }
        else if ((hand == 1) && (this.thisHuman.bodyState == "standing") && (this.thisHuman.sleepingState == false) && (this.thisHuman.RHState == true) && ((this.thisHuman.actionState == "none") || (this.thisHuman.actionState == "eating with right hand"))) {
            this.thisHuman.animator.SetTrigger("eat");
            this.thisHuman.animator.SetFloat("EatL/R",1);
            thisHuman.driveSystem.stateDict["hunger"] = 0;
            UnityEngine.Object.Destroy(thisHuman.rightHand.GetChild(0).gameObject, 5);
            this.thisHuman.RHState = false;
        }
        else {
            //Debug.Log("can't eat");
        }
    }

}



