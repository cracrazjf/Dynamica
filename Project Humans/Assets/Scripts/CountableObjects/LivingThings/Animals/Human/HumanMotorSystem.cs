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

    public float velocity;
    public float maxVelocity;
    public float minVelocity;
    public float maxAcceleration_rate;
    public float max_rotation_speed;

    public List<string> actionLabelList = new List<string>{ "accellerate",   // value -1..1 translating to speed of accel./deccel.
                                                    "rotate",       // value from -1 to 1, translating into -180..180 degree rotation
                                                    "sit",          // begin to sit
                                                    "stand_up",        // begin to stand
                                                    "lay_down",          // begin to lay down
                                                    "sleep",        // begin to sleep
                                                    "wake_up",      // begin to wake
                                                    "pick_up", 
                                                    "set_down",
                                                    "put_in",
                                                    "get_from",
                                                    "drink",
                                                    "eat"
                                                    };  // do we want rest to be explicit, or just not doing anything else

    public int numActions; // 15
    
    public Dictionary<string, int> actionIndexDict = new Dictionary<string, int>();
    public List<float> actionValueList = new List<float>(); // [1 0 0 0 0 0 1 0 0 0 0 1 0]
    public Dictionary<string, bool> actionDisplayDict = new Dictionary<string, bool>();
    public Transform Eye_L;
    public Transform Eye_R;

    public HumanMotorSystem(Human human) {
        this.thisHuman = human;
        for (int i = 0; i < actionLabelList.Count; i++){
            actionIndexDict.Add(actionLabelList[i], i);
            actionDisplayDict.Add(actionLabelList[i], true);
        }
        
        maxVelocity = float.Parse(this.thisHuman.phenotype.traitDict["final_velocity"]);
        minVelocity = float.Parse(this.thisHuman.phenotype.traitDict["initial_velocity"]);
        maxAcceleration_rate = float.Parse(thisHuman.phenotype.traitDict["maxacceleration_rate"]);
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

        if (actionChoice.actionValueDict["sit"] == 1){
            sit();
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
        else if (actionChoice.actionValueDict["accellerate"] == 1){
            accellerate(actionChoice.argumentDict["movementVelocity"], actionChoice.targetPos);
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
        else if (actionChoice.actionValueDict["put_in"] == 1){
            putIn(actionChoice.argumentDict["hand"]);
        }
        else if (actionChoice.actionValueDict["get_from"] == 1){
            getFrom(actionChoice.argumentDict["hand"]);
        }
        else if (actionChoice.actionValueDict["eat"] == 1){
            eat(actionChoice.argumentDict["hand"]);
        }
        else if (actionChoice.actionValueDict["drink"] == 1) {
            drink();
        }
    }

// each human has a max velocity (10), and a max accellaration (2)
// 

    public void accellerate(float velocity, Vector3 targetPosition){
        // you can only do this while standing
        if(velocity > maxVelocity) {
            velocity = maxVelocity;
        }
        else if (velocity < minVelocity) {
            velocity = minVelocity;
        }
        thisHuman.gameObject.transform.position = Vector3.MoveTowards(thisHuman.gameObject.transform.position, targetPosition, velocity * Time.deltaTime);
    
        
        if(velocity >0) {
            this.thisHuman.animator.SetBool("moving", true);
            this.thisHuman.animator.SetFloat("Velocity", velocity);
            //Debug.Log(velocity);
        }
        if(thisHuman.gameObject.transform.position == targetPosition) {
            this.thisHuman.animator.SetBool("moving", false);
            velocity = 0;
        }
        //Debug.Log(velocity);
        

    }
    //public float degreePerSec = 0;
    public float rotateAngle = 360;
    public void rotate(float rotationAngle){
        
        float RA = rotationAngle * max_rotation_speed;
        float rotation =RA * Time.deltaTime;
        
        if (rotateAngle > rotation) {
            rotateAngle -= rotation;
        }
        else {
            rotation = rotateAngle;
            rotateAngle = 0;
            //rotated = false;
        }
        this.thisHuman.gameObject.transform.Rotate(0,rotation,0); 
        
    }
    public void drink() {
        this.thisHuman.animator.SetTrigger("Drink");
    }
    public void sit(){
        this.thisHuman.animator.SetBool("sit", true);
        this.thisHuman.animator.SetBool("keepsitting", true);
    }
        
    public void standUp(){
        this.thisHuman.animator.SetBool("sit", false);
    }

    public void layDown(){
        this.thisHuman.animator.SetBool("sit", true);
        this.thisHuman.animator.SetBool("sleep", true);
    }

    public void sleep(){
        
        this.thisHuman.animator.SetBool("sit", true);
        this.thisHuman.animator.SetBool("sleep", true);
        this.thisHuman.Eye_L.localScale = new Vector3(1,0.09f,2);
        this.thisHuman.Eye_R.localScale = new Vector3(1,0.09f,2);
    }
    
    public void wakeUp(){
        
        this.thisHuman.Eye_L.localScale = new Vector3(1,1,1);
        this.thisHuman.Eye_R.localScale = new Vector3(1,1,1);
        this.thisHuman.animator.SetBool("sleep", false);
        this.thisHuman.animator.SetBool("sit", true);
    }
    public void pickUp(float hand){
        Collider[] pickableObj = new Collider[5];
    
        if (hand == 0 && this.thisHuman.Hand_L.childCount == 0) {
            int numObj = Physics.OverlapSphereNonAlloc(this.thisHuman.Hand_L.position,0.2f,pickableObj);
            for(int i = 0; i < numObj; i++) {
                pickableObj[i].GetComponent<Rigidbody>().isKinematic = true;
                pickableObj[i].GetComponent<Rigidbody>().useGravity = false;
                pickableObj[i].transform.parent = this.thisHuman.Hand_L.transform;
                pickableObj[i].transform.localPosition = new Vector3(0,0,0);
                   
                
            }
            this.thisHuman.rigidbody.isKinematic = true;
            this.thisHuman.animator.SetBool("pickup", true);
            this.thisHuman.animator.SetFloat("PickupL/R",1);
            Debug.Log("pick up with left hand");
        }
            
        else if (hand == 1 && this.thisHuman.Hand_R.childCount == 0) {

            int numObj = Physics.OverlapSphereNonAlloc(this.thisHuman.Hand_R.position,0.2f,pickableObj);
            for(int i = 0; i < numObj; i++) {
                pickableObj[i].GetComponent<Rigidbody>().isKinematic = true;
                pickableObj[i].transform.parent = this.thisHuman.Hand_R.transform;
                pickableObj[i].transform.localPosition = new Vector3(0,0,0);
                
            }
            this.thisHuman.rigidbody.isKinematic = true;
            this.thisHuman.animator.SetBool("pickup", true);
            this.thisHuman.animator.SetFloat("PickupL/R",0);
            //Debug.Log("pick up with right hand");
        }
        else {
            //this.thisHuman.rigidbody.isKinematic = false;
            this.thisHuman.animator.SetBool("pickup", false);
            Debug.Log("can't pick");
        }
        
    }

    public void setDown(float hand){
        // check to make sure something is in the hand
        
        if (hand == 0 && this.thisHuman.Hand_L.childCount ==1) {
            this.thisHuman.animator.SetBool("setDown", true);
            this.thisHuman.animator.SetFloat("SetdownL/R",1);
            if (thisHuman.Hand_L.GetChild(0).transform.position.y < 0.1) {
                thisHuman.Hand_L.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                thisHuman.Hand_L.GetChild(0).transform.parent = null;
                Debug.Log(this.thisHuman.Hand_L.childCount);
            }
            
            Debug.Log("set down with left hand");
        }
            
        else if (hand == 1 && this.thisHuman.Hand_R.childCount ==1) {
            this.thisHuman.animator.SetBool("setDown", true);
            this.thisHuman.animator.SetFloat("Setdown/R",0);
            if (thisHuman.Hand_R.GetChild(0).transform.position.y < 0.1) {
                thisHuman.Hand_R.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                thisHuman.Hand_R.GetChild(0).transform.parent = null;
            }
            Debug.Log("set down with right hand");
        }

        else {
            this.thisHuman.animator.SetBool("setDown", false);
            Debug.Log("can't set down");
        }
    }
    
    public void putIn(float hand){
        if (hand == 0 && this.thisHuman.Hand_L.childCount == 1) {
            this.thisHuman.animator.SetTrigger("putin");
            this.thisHuman.animator.SetFloat("PutInL/R",1);
        }
        else if (hand == 1 && this.thisHuman.Hand_R.childCount == 1) {
            this.thisHuman.animator.SetTrigger("putin");
            this.thisHuman.animator.SetFloat("PutInL/R",0);
        }
        else {
            Debug.Log("nothing to put");
        }

    }

    public void getFrom(float hand){
        if (hand == 0 && this.thisHuman.Hand_L.childCount == 0) {
            this.thisHuman.animator.SetTrigger("getfrom");
            this.thisHuman.animator.SetFloat("GetFromL/R",1);
        }
        else if (hand == 1 && this.thisHuman.Hand_R.childCount == 0) {
            this.thisHuman.animator.SetTrigger("getfrom");
            this.thisHuman.animator.SetFloat("GetFrom",0);
        }
        else {
            Debug.Log("can't get from ...");
        }
    }
        public void eat(float hand) {
        if (hand == 0 && this.thisHuman.Hand_L.childCount == 1) {
            this.thisHuman.animator.SetBool("eat", true);
            this.thisHuman.animator.SetFloat("EatL/R",1);
        }
        else if (hand == 1 && this.thisHuman.Hand_R.childCount == 1) {
            this.thisHuman.animator.SetBool("eat", true);
            this.thisHuman.animator.SetFloat("EatL/R",0);
        }
        else {
            Debug.Log("can't eat");
        }
    }

}



