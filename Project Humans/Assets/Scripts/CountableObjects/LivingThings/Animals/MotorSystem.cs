using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSystem 
{
    
    public Animal thisAnimal; // we need this if we want to access drive system
    public LivingObject thisLivingObject; // we need this if we want to access genome or phenotype
    public float rotateAngle = 360;
    public float maxStepDistance;
    public float maxRotationSpeed;
    public float velocity;

    public List<string> actionLabelList = new List<string> {
        "take_step",            // value -1..1 translating to speed of accel./deccel.
        "rotate",               // value from -1 to 1, translating into -180..180 degree rotation
        "sit_up",               // begin to sit up
        "sit_down",             // begin to sit down
        "stand_up",             // begin to stand
        "lay_down",             // begin to lay down
        "sleep",                // begin to sleep
        "wake_up",              // begin to wake
        "pick_up", 
        "set_down",
        "drink",
        "eat"
        };

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

    public MotorSystem(Animal passed) {
        this.thisAnimal = passed;
        for (int i = 0; i < actionLabelList.Count; i++){
            actionIndexDict.Add(actionLabelList[i], i);
            actionDisplayDict.Add(actionLabelList[i], true);
        }
        
        // velocity = float.Parse(thisAnimal.phenotype.traitDict["currentVelocity"]);
        // maxStepDistance = float.Parse(thisAnimal.phenotype.traitDict["maxStepDistance"]);
        // maxRotationSpeed = float.Parse(thisAnimal.phenotype.traitDict["maxRotationSpeed"]);


    }

    public void takeAction(ActionChoice actionChoice){

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

    public virtual void takeSteps(float stepProportion) {}

    public virtual void rotate(float rotationAngle){}

    public virtual void drink(){}
    
    public virtual void sitDown(){}

    public virtual void sitUp(){}
        
    public virtual void standUp(){}

    public virtual void layDown(){}

    public virtual void sleep(){}
    
    public virtual void wakeUp(){}
    
    public virtual void pickUp(float hand){}

    public virtual void setDown(float hand){}

    public virtual void eat(float hand) {}
}