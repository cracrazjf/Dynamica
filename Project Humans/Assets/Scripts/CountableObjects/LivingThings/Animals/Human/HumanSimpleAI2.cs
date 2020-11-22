using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanSimpleAI2
{
    public Human thisHuman;
    public ActionChoice actionChoice;
    public FOVDetection thisfOVDetection;
    Vector3 randomPoint;
    int Range = 10;
    string currentGoal = "None";
    string targetTag = "None";
    public Vector3 pickUpPosition = new Vector3();
    bool doingNothing;
    bool newRandomPos = false;
    string objectTypeInLH = "None";
    string objectTypeInRH = "None";
    
    public HumanSimpleAI2 (Human human) {
        this.thisHuman = human;
        actionChoice = new ActionChoice(this.thisHuman.GetMotorSystem().actionLabelList);
        thisfOVDetection = this.thisHuman.gameObject.GetComponent<FOVDetection>();
    }

    public ActionChoice chooseAction(){
        //Debug.Log(currentGoal);
        doingNothing = thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Default");
        thisHuman.GetFOVDetection().inFov(thisHuman.gameObject.transform, 45, 10);
        actionChoice.initActionChoices(this.thisHuman.GetMotorSystem().actionLabelList);
        if (currentGoal == "Decrease Thirst"){
            decreaseThirst();
        }
        else if (currentGoal == "Decrease Sleepiness"){
            decreaseSleepiness();
        }
        else if (currentGoal == "Decrease Hunger"){
            decreaseHunger();
        }
        else if (currentGoal == "Decrease Fatigue"){

        }
        else if (currentGoal == "Increase Health") {

        }
        else{
            chooseGoal();
        }

        return actionChoice;
        
    }

    public void chooseGoal(){
        
        if (thisHuman.GetDriveSystem().stateDict["thirst"] > float.Parse(thisHuman.phenotype.traitDict["thirst_threshold"])) {
            currentGoal = "Decrease Thirst";
        }
        if (thisHuman.GetDriveSystem().stateDict["hunger"] > float.Parse(thisHuman.phenotype.traitDict["hunger_threshold"])) {
            currentGoal = "Decrease Hunger";
        }
        if (thisHuman.GetDriveSystem().stateDict["sleepiness"] > float.Parse(thisHuman.phenotype.traitDict["sleepiness_threshold"])) {
            currentGoal = "Decrease Sleepiness";
        }
        if (thisHuman.GetDriveSystem().stateDict["fatigue"] > float.Parse(thisHuman.phenotype.traitDict["fatigue_threshold"])) {
            currentGoal = "Decrease Fatigue";
        }
        if (thisHuman.GetDriveSystem().stateDict["health"] < float.Parse(thisHuman.phenotype.traitDict["health_threshold"])) {
            currentGoal = "Increase Health";
        }
        thisHuman.GetMotorSystem().rotateAngle = 360;
        
    }

    public void decreaseThirst(){
        // check if sleeping

        if (this.thisHuman.bodyState == "standing") {
            if (checkIfTargetVisible("water").Count > 0) {
                List<GameObject> targets = checkIfTargetVisible("water");
                GameObject target = caculateCloestObject(targets);
                if (checkIfTargetReachable(target)) {
                    actionChoice.actionValueDict["drink"] = 1;
                    currentGoal = "None";
                }
                else {
                    goToObject(target);
                }
            }
            else {
                rotate(1.0f);
                searchForThing();
            }
        }
        else {
            if (this.thisHuman.bodyState == "sitting") {
                actionChoice.actionValueDict["stand_up"] = 1;
            }
            if (this.thisHuman.bodyState == "laying") {
                actionChoice.actionValueDict["sit_up"] = 1;
            }
            if (this.thisHuman.sleepingState) {
                actionChoice.actionValueDict["wake_up"] = 1;
            }
        }
    }
    
    public void decreaseHunger(){
        if (this.thisHuman.bodyState == "standing") {
            if (objectTypeInLH == "food"){
                actionChoice.argumentDict["hand"] = 0;
                actionChoice.actionValueDict["eat"] = 1;
                if (!this.thisHuman.LHState) {
                    objectTypeInLH = "None";
                    currentGoal = "None";
                }
                
            }
            else{
                if (objectTypeInRH == "food"){
                    actionChoice.actionValueDict["eat"] = 1;
                    actionChoice.argumentDict["hand"] = 1;
                    currentGoal = "None";
                    objectTypeInRH = "None";
                }
                else{
                    if (checkIfTargetVisible("food").Count > 0) {
                        List<GameObject> targets = checkIfTargetVisible("food");
                        GameObject target = caculateCloestObject(targets);
                        if (checkIfTargetReachable(target)) {
                            if ((objectTypeInLH != "None") && (objectTypeInRH != "None")){
                                actionChoice.actionValueDict["set_down"] = 1;
                                actionChoice.argumentDict["hand"] = 0;
                                objectTypeInLH = "None";
                            }
                            else{
                                if (objectTypeInLH == "None") {
                                    pickUpPosition = target.transform.position;
                                    actionChoice.argumentDict["hand"] = 0;
                                    actionChoice.actionValueDict["pick_up"] = 1;
                                    if (this.thisHuman.LHState){
                                        objectTypeInLH = "food";
                                    }
                                }
                                else if (objectTypeInRH == "None"){
                                    pickUpPosition = target.transform.position;
                                    actionChoice.argumentDict["hand"] = 1;
                                    actionChoice.actionValueDict["pick_up"] = 1;
                                    if (this.thisHuman.LHState){
                                        objectTypeInRH = "food";
                                    }
                                }
                            }
                        }
                        else {
                            goToObject(target);
                        }
                    }
                    else {
                        rotate(1.0f);
                        searchForThing();
                    }
                }
            }
        }
        else {
           if (this.thisHuman.bodyState == "sitting") {
                actionChoice.actionValueDict["stand_up"] = 1;
            }
            if (this.thisHuman.bodyState == "laying") {
                actionChoice.actionValueDict["sit_up"] = 1;
            }
            if (this.thisHuman.sleepingState) {
                actionChoice.actionValueDict["wake_up"] = 1;
            }
        }
    }
    

    public void decreaseSleepiness() {
        if (this.thisHuman.bodyState == "laying") {
            actionChoice.actionValueDict["sleep"] = 1;
        }
        else {
            actionChoice.actionValueDict["lay_down"] = 1;
        }
        
        
    }
    
    // this checks if visible and calculates closest
    // break this into those separate
    public List<GameObject> checkIfTargetVisible(string targetTag) {
        List<GameObject> targets = new List<GameObject>();
        
        if (thisfOVDetection.objects_in_vision.Count != 0) {
            for (int i = 0; i < thisfOVDetection.objects_in_vision.Count; i++) {
                    if (targetTag != null) {
                        if (thisfOVDetection.objects_in_vision[i].CompareTag(targetTag)) {
                            targets.Add(thisfOVDetection.objects_in_vision[i]);
                    }
                }
            }
        }
        return targets;
    }

    public GameObject caculateCloestObject(List<GameObject> targets) {
        GameObject cloestObject = null;
        float closestTargetDistance = Mathf.Infinity;
        for (int i = 0; i <targets.Count; i++) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, targets[i].transform.position);
            if (distance < closestTargetDistance) {
                closestTargetDistance = distance;
                cloestObject = targets[i];
            }
        }
        return cloestObject;

    }

    // this checks if reachable
    public bool checkIfTargetReachable(GameObject target) {
        if (target != null) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, target.transform.position);
            if (distance <=1 && thisHuman.GetMotorSystem().velocity == 0) {
                return true;
            }
            else {
                return false;
            }
        }
        return false;
        
    }

    public void goToObject(GameObject target) {
        if (target != null) {
            thisHuman.gameObject.transform.LookAt(target.transform);
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position,target.transform.position);
            if (distance > 0) {
                if (distance > 1) {
                    var stepRate = 1.0f;
                    var step = stepRate * thisHuman.GetMotorSystem().maxStepDistance;
                    if (step >= distance) {
                        var nextStep = distance;
                        actionChoice.actionValueDict["take_step"] = 1;
                        actionChoice.argumentDict["stepRate"] = nextStep/thisHuman.GetMotorSystem().maxStepDistance;
                    }
                    else {
                        actionChoice.actionValueDict["take_step"] = 1;
                        actionChoice.argumentDict["stepRate"] = 0.5f;
                    }
                }
                else if (distance <= 1){
                    actionChoice.actionValueDict["take_step"] = 1;
                    actionChoice.argumentDict["stepRate"] = 0;
                }
            }
        }
    }

    public void rotate(float Angle) {
        actionChoice.actionValueDict["rotate"] = 1;
        actionChoice.argumentDict["rotationAngle"] = Angle;
    }

    public void searchForThing(){
        if (thisHuman.GetMotorSystem().rotateAngle == 0) {
            if (!newRandomPos) {
            randomPoint = new Vector3(Random.Range(thisHuman.gameObject.transform.position.x - Range, 
                                thisHuman.gameObject.transform.position.x + Range), 0, 
                                Random.Range(thisHuman.gameObject.transform.position.z - Range, 
                                thisHuman.gameObject.transform.position.z + Range));
                                Debug.Log(randomPoint  + " and " + (thisHuman.gameObject.transform.position - randomPoint).magnitude);
            newRandomPos = true;
            }
            thisHuman.gameObject.transform.LookAt(randomPoint);
            actionChoice.actionValueDict["take_step"] = 1;
            actionChoice.argumentDict["stepRate"] = 0.5f;
            if ((thisHuman.gameObject.transform.position - randomPoint).magnitude < 3) {
                newRandomPos = false;
            }
        }

    }
        
    
}



