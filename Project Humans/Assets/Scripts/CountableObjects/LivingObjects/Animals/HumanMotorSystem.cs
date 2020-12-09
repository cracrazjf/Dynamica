using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMotorSystem : MotorSystem
{
    private Human thisHuman;

    public HumanMotorSystem(Human human) : base(human) {
        this.thisHuman = human;
        humanActionStateLabelList = new List<string>
        {
            "sitting down", 
            "sitting up", 
            "laying down",
            "standing up", 
            "rotating", 
            "taking steps",
            "picking up with left hand", 
            "picking up with right hand", 
            "setting down with left hand",
            "setting down with right hand",
            "eating with left hand", 
            "eating with right hand",
            "drinking",
            "waking up",
            "falling asleep"
        };

        actionArgumentLabelList = new List<string>
        {        
            "movement velocity",
            "step rate",            
            "rotation angle"               
            "rotation velocity",               
            "hand"
        };

        InitActionRuleDicts();
    }

    public override void InitActionRuleDicts(){
        bodyStateRequirementDict["taking steps"].add("standing");
        bodyStateObstructorDict["taking steps"].add("sleeping");

        bodyStateRequirementDict["rotating"].add("standing");
        bodyStateObstructorDict["rotating"].add("sleeping");

        bodyStateRequirementDict["drinking"].add("standing");
        bodyStateObstructorDict["drinking"].add("sleeping");

        bodyStateRequirementDict["eating"].add("standing");
        bodyStateObstructorDict["eating"].add("sleeping");

        bodyStateRequirementDict["falling asleep"].add("laying");
        bodyStateObstructorDict["falling asleep"].add("sleeping");
    }

    public override void UpdateActionStates(){
        this.actionStateArray = new bool[this.numActionStates]);

        if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Rotate")) {
             this.actionStateArray[actionStateIndexDict["rotating"]] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Sit down")) {
            this.actionStateArray[actionStateIndexDict["sitting down"]] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Sit up")) {
            this.actionStateArray[actionStateIndexDict["sitting up"]] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Lay down")) {
             this.actionStateArray[actionStateIndexDict["laying down"]] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Taking steps")) {
            this.actionStateArray[actionStateIndexDict["taking steps"]] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Drink")) {
            this.actionStateArray[actionStateIndexDict["drinking"]] = true;
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Eat")) {
            if (this.animator.GetFloat("EatL/R") == 0) {
                this.actionStateArray[actionStateIndexDict["eating with left hand"]] = true;
            }
            if (this.thisHuman.animator.GetFloat("EatL/R") == 1) {
                this.actionStateArray[actionStateIndexDict["eating with right hand"]] = true;
            }
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("PickUp")) {
            if (this.animator.GetFloat("PickupL/R") == 0) {
                this.actionStateArray[actionStateIndexDict["picking up with left hand"]] = true;
            }
            if (this.thisHuman.animator.GetFloat("PickupL/R") == 1) {
                this.actionStateArray[actionStateIndexDict["picking up with right hand"]] = true;
            }
        }
        else if (this.thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("SetDown")) {
            if (this.animator.GetFloat("SetdownL/R") == 0) {
                 this.actionStateArray[actionStateIndexDict["setting down with left hand"]] = true;
            }
            if (this.thisHuman.animator.GetFloat("SetdownL/R") == 1) {
                 this.actionStateArray[actionStateIndexDict["setting down with right hand"]] = true;
            }
        }
    }

    public override bool checkActionLegality(string action){
        bool legal = false;
        
        // action == "waking up"

        List<string> requiredBodyStateList = bodyStateRequirementDict[action];
        // ["laying", "sleeping"]
        for (int i = 0; i < requiredBodyStateList.Count(); i++){
            string bodyState = requiredBodyStateList[i];
            if (this.thisHuman.Body.GetBodyState(bodyState)){
                legal = true;
            }
            else{
                legal = false;
            }
        }
        return legal;
    }

    public override void TakeAction(List<float[]> actionChoiceList){

        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);

        if (doingNothing) {
            if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting down"]]){
                SitDown();
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting up"]]){
                SitUp();
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["standing up"]]){
                StandUp();
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["laying down"]]){
                LayDown();
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["falling asleep"]]){
                Sleep();
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["waking up"]]){
                WakeUp();
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["eating"]]){
                Eat(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]]);
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["drinking"]]){
                Drink();
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["taking steps"]]){
                TakeSteps(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["step rate"]]);
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["rotating"]]){
                Rotate(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["rotation angle"]]);
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["picking up"]]){
                PickUp(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]]);
            }
            else if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["setting down"]]){
                SetDown(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]]);
            }
                
        }
        else{
            if (actionStateArray[actionStateIndexDict["taking steps"]] == true){
                if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["taking steps"]]){
                    TakeSteps(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["step rate"]]);
                }
            }

            else if (actionStateArray[actionStateIndexDict["rotating"]] == true){
                if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["rotating"]]){
                    Rotate(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["rotation angle"]]);
                }
            }

            else if (actionStateArray[actionStateIndexDict["picking up"]] == true){
                if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["picking up"]]){
                    PickUp(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]]);
                }
            }

            else if (actionStateArray[actionStateIndexDict["setting down"]] == true){
                if (actionChoiceStruct.actionChoiceArray[actionStateIndexDict["setting down"]]){
                   Setdown(actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]]);
                }
            }
        }
    }   

    public void TakeSteps(float stepProportion) {
        // call check to see if legal
        if (checkActionLegality("take steps")){
            velocity = (stepProportion * maxStepDistance);
            if (stepProportion != 0) {
                var step = stepProportion * maxStepDistance;
                thisHuman.gameObject.transform.Translate(Vector3.forward *step*Time.deltaTime);
                this.thisHuman.animator.SetBool("moving", true);
                this.thisHuman.animator.SetFloat("Velocity", 0);
                this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
                this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["taking steps"], true);
            }
            else {
                this.thisHuman.animator.SetBool("moving", false);
                this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            }
        }
    }

    public void rotate(float rotationAngle){

        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        
        float rotation = rotationAngle * float.Parse(thisHuman.phenotype.traitDict["max_rotation_speed"]) * Time.deltaTime;
        //what works for take steps works here
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["standing"])) 
            && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && ((doingNothing) || this.thisHuman.nervousSystem.GetActionState(this.thisHuman.nervousSystem.actionStateIndexDict["rotating"]))) {

            if (rotateAngle > rotation) {
                rotateAngle -= rotation;
            }
            else {
                rotation = rotateAngle;
                rotateAngle = 0;
            }
            this.thisHuman.gameObject.transform.Rotate(0,rotation,0);
            this.thisHuman.animator.SetBool("rotate", true);
            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["rotating"], true);

            if (rotateAngle == 0) {
                this.thisHuman.animator.SetBool("rotate", false);
                this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            }
        }
    }

    public void drink() {
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        //same
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["standing"])) 
            && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && (doingNothing)) {

            this.thisHuman.animator.SetTrigger("Drink");
            // if you can drink while drinking, this cant be called here. should be once per completed drink
            // and it should be a call to a function in driveSystem, telling it what was drank
            thisHuman.GetDriveSystem().stateDict["thirst"] = 0;
            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["drinking"], true);
        }
    }
    
    public void sitDown(){
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        //same
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["standing"])) 
            && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && (doingNothing)) {

            this.thisHuman.animator.SetBool("sit", true);
            this.thisHuman.animator.SetBool("keepsitting", true);
            
            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["sitting down"], true);

            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["standing"], false);
            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["sitting"], true);
        }
    }

    public void sitUp(){
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        //same
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["laying"])) 
            && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && (doingNothing)) {

            this.thisHuman.animator.SetBool("sleep", false);
            this.thisHuman.animator.SetBool("keepsitting", true);

            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["sitting up"], true);

            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["laying"], false);
            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["sitting"], true);
        }
    }
        
    public void standUp(){
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        //same
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sitting"])) 
            && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && (doingNothing)) {

            this.thisHuman.animator.SetBool("sit", false);

            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["standing up"], true);

            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["sitting"], false);
            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["standing"], true);
        }
       
    }

    public void layDown(){
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        //same
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sitting"])) 
            && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && (doingNothing)) {

            this.thisHuman.animator.SetBool("sit", true); // shouldnt this be false???
            this.thisHuman.animator.SetBool("sleep", true);

            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["laying down"], true);

            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["sitting"], false);
            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["laying"], true);
        }
    }

    public void sleep(){
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        //same
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["laying"])) 
            && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && (doingNothing)) {

            this.thisHuman.leftEye.localScale = new Vector3(1, 0.09f, 2);
            this.thisHuman.rightEye.localScale = new Vector3(1, 0.09f, 2);

            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["sleeping"], true);
        }
    }
    
    public void wakeUp(){
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);
        // same
        if ((this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["laying"])) 
            && (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])) 
            && (doingNothing)) {
            this.thisHuman.leftEye.localScale = new Vector3(1,1,1);
            this.thisHuman.rightEye.localScale = new Vector3(1,1,1);

            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.actionStateIndexDict["sleeping"], false);

        }
    }
    
    public void pickUp(float hand){
        Collider[] pickableObj = new Collider[5];
        bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);

        if (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])){
            if (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["standing"])){
                if ((doingNothing) 
                    || (this.thisHuman.nervousSystem.GetActionState(this.thisHuman.nervousSystem.actionStateIndexDict["picking up with left hand"])) 
                    || (this.thisHuman.nervousSystem.GetActionState(this.thisHuman.nervousSystem.actionStateIndexDict["picking up with right hand"]))){
                    int pickUpHand = -1;
                    int numObj = -1;
                    if ((hand == 0) && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["holding with left hand"]))) {
                        pickUpHand = 0;
                        numObj = Physics.OverlapSphereNonAlloc(this.thisHuman.leftHand.position,0.2f,pickableObj);
                    }
                    else if ((hand == 1) && (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["holding with right hand"]))){
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
                                    this.thisHuman.animator.ResetTrigger("pick");
                                    pickableObj[i].transform.localPosition = new Vector3(0.1311f, 0.0341f, 0.073f);

                                    this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
                                    this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["picking up with left hand"], true);
                                    this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.bodyStateIndexDict["holding with left hand"], true);
                                }
                                else{
                                    pickableObj[i].transform.parent = this.thisHuman.rightHand.transform;
                                    this.thisHuman.animator.ResetTrigger("pick");
                                    pickableObj[i].transform.localPosition = new Vector3(-0.1593f, -0.026f, -0.0665f);

                                    this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
                                    this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["picking up with right hand"], true);
                                    this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.bodyStateIndexDict["holding with right hand"], true);
                                }
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

    // change the rest of the actions so that they are using the new nervous system data structures
    // check for other places, like simpleAI, that use the old way of keeping track
    // then we need to arrange everything in MotorSystem vs. HumanMotorSystem, so it is like Nervous System

    public void setDown(float hand){

        if (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])){
            if (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["standing"])){

                bool doingNothing = !this.thisHuman.nervousSystem.GetActionStateArray().Any(x => x);

                if ((hand == 0) 
                    && (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["holding with left hand"])) 
                    && ((doingNothing) || (this.thisHuman.nervousSystem.GetActionState(this.thisHuman.nervousSystem.actionStateIndexDict["setting down with left hand"])))) 
                {
                    this.thisHuman.animator.ResetTrigger("pick");
                    this.thisHuman.animator.SetTrigger("set");
                    this.thisHuman.animator.SetFloat("SetdownL/R",0);
                    if (thisHuman.leftHand.GetChild(0).transform.position.y < 0.1) {
                        thisHuman.leftHand.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                        thisHuman.leftHand.GetChild(0).transform.parent = null;
                        if (this.thisHuman.leftHand.childCount == 0) {
                            this.thisHuman.animator.ResetTrigger("set");
                            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
                            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["setting down with left hand"], true);
                            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.bodyStateIndexDict["holding with left hand"], false);
                        }
                    }
                }
                    
                else if ((hand == 1) 
                    && (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["holding with right hand"])) 
                    && ((doingNothing) || (this.thisHuman.nervousSystem.GetActionState(this.thisHuman.nervousSystem.actionStateIndexDict["setting down with right hand"])))) 
                {

                    this.thisHuman.animator.ResetTrigger("pick");
                    this.thisHuman.animator.SetTrigger("set");
                    this.thisHuman.animator.SetFloat("Setdown/R",1);
                    if (thisHuman.rightHand.GetChild(0).transform.position.y < 0.1) {
                        thisHuman.rightHand.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                        thisHuman.rightHand.GetChild(0).transform.parent = null;
                        if (this.thisHuman.rightHand.childCount == 0) 
                        {
                            this.thisHuman.animator.ResetTrigger("set");
                            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
                            this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["setting down with right hand"], true);
                            this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.bodyStateIndexDict["holding with right hand"], false);
                        }
                    }
                }

                else {
                    Debug.Log("Can't set down because doing something or nothing in the specified hand");
                }
            }
            else {
                Debug.Log("Can't set down because not standing");
            }
        }
        else {
            Debug.Log("Can't set down because sleeping");
        }

    }

    public void eat(float hand) {
        bool success = false;

        if (!this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["sleeping"])){
            if (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["standing"])){

                if ((hand == 0) 
                    && (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["holding with left hand"])) 
                    && ((doingNothing) || (this.thisHuman.nervousSystem.GetActionState(this.thisHuman.nervousSystem.actionStateIndexDict["eating with left hand"]))))
                {
                    success = true;
                }

                else if ((hand == 1)
                    && (this.thisHuman.nervousSystem.GetBodyState(this.thisHuman.nervousSystem.bodyStateIndexDict["holding with right hand"])) 
                    && ((doingNothing) || (this.thisHuman.nervousSystem.GetActionState(this.thisHuman.nervousSystem.actionStateIndexDict["eating with right hand"]))))
                {
                    success = true;
                }
                else {
                    //Debug.Log("can't eat");
                }
            }
        }

        if (success){
            // this needs to be fixed the same way as thirst.
            // should probably be a call to drive system with what was eaten, and the code for how drive system should be updated should be located there
            thisHuman.GetDriveSystem().stateDict["hunger"] = 0;
            this.thisHuman.animator.SetTrigger("eat");
            this.thisHuman.animator.SetFloat("EatL/R", hand);
            this.thisHuman.NervousSystem.setActionArray(new bool[this.thisHuman.NervousSystem.numActionStates]);
            if (hand == 0){
                UnityEngine.Object.Destroy(thisHuman.leftHand.GetChild(0).gameObject, 5);                 
                this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["eating with left hand"], true);
                this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.bodyStateIndexDict["holding with left hand"], false);
            }
            else{
                UnityEngine.Object.Destroy(thisHuman.rightHand.GetChild(0).gameObject, 5);
                this.thisHuman.NervousSystem.setActionState(this.thisHuman.NervousSystem.actionStateIndexDict["eating with right hand"], true);
                this.thisHuman.NervousSystem.setBodyState(this.thisHuman.NervousSystem.bodyStateIndexDict["holding with right hand"], false);
            }

        }

    }
}



