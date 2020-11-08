using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanSimpleAI3
{
    public Human thisHuman;
    public HumanActionChoice actionChoice;

    public Dictionary<string, bool> knowledgeDict = new Dictionary<string, bool>();
    public Dictionary<string, List<string>> subGoalListDict = new Dictionary<string, List<string>>();
    
    public List<string> stateList = new List<string>();
    Vector3 randomPoint;
    int Range = 10;
    public bool newRandomPos = false;

    string currentGoal;
    string currentSubGoal = "None";
    int currentSubGoalIndex;
    List<string> currentSubGoalList = new List<string>();

    public string targetTag = "None";

    int closestTargetIndex;
    bool doingNothing;
    public int counter;
    
    public HumanSimpleAI3 (Human human) {
        this.thisHuman = human;
        actionChoice = new HumanActionChoice(this.thisHuman.humanMotorSystem.actionLabelList);
        initKnowledgeDict();
        initActionKnowledgeDict();
    }
    
    public void initKnowledgeDict(){
        knowledgeDict.Add("waterVisible", false);
        knowledgeDict.Add("waterReachable", false);
    }

    public void initActionKnowledgeDict(){
        subGoalListDict.Add("Decrease Thirst", new List<string> {"Drink Water", "GoTo Water", "SearchFor Water"});
        subGoalListDict.Add("Decrease Hunger", new List<string> {"Eat Food", "PickUp Food", "GoTo Food", "SearchFor Food"});
        subGoalListDict.Add("Decrease Sleepiness", new List<string> {"Sleep"});
        subGoalListDict.Add("Decrease Fatigue", new List<string> {"GoTo Water", "SearchFor Water"});
    }

    public HumanActionChoice chooseAction(float[ , , ] visualInput){
        doingNothing = thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Default");
        if (doingNothing){ // or an exception has occurred
            if (currentSubGoal == "None"){
                chooseCurrentGoals();
            }
            else{
                if (currentSubGoalIndex == 0){
                    //update knowledge dict with whatever facts have changed given that youve accomplished the goal
                    chooseCurrentGoals();
                }
                else{
                    // update knowledge dict with whatever facts have changed given that youve accomplished the action
                    currentSubGoalIndex --;
                    currentSubGoal = currentSubGoalList[currentSubGoalIndex];
                    Debug.Log(currentSubGoal);
                    
                }
            }
            setActionChoiceValues();
            
        }
        return actionChoice;
    }

    public void setActionChoiceValues() {
        // take the current subgoal, which we know we can do, and set actionChoice.actionValueArray and .actionArgumentArray appropriately
        //int actionIndex;

        if (currentSubGoal == "Sleep"){
            
            actionChoice.actionValueDict["sleep"] = 1;
        }
        if (currentSubGoal == "Drink Water"){
            actionChoice.actionValueDict["drink"] = 1;
        }
        if (currentSubGoal == "Eat Food"){
            actionChoice.actionValueDict["Eat"] = 1;
            // if food in LH:
                actionChoice.argumentDict["Hand"] = 1;

        }
        if (currentSubGoal == "SitDown"){
            
            actionChoice.actionValueDict["sit"] = 1;
        }
        if (currentSubGoal == "Rest"){
            
            actionChoice.actionValueDict["sleep"] = 1;
            
        }
        if (currentSubGoal == "SearchFor Water") {
            searchForThing(1,"water");
        }
        if (currentSubGoal == "GoTo Water") {
            if (closestTargetIndex >= 0) {
                goToObject(thisHuman.fOVDetection.objects_in_vision[closestTargetIndex]);
            }
            
        }
    }

    public void chooseCurrentGoals(){
        
        if (thisHuman.driveSystem.stateDict["thirst"] > float.Parse(thisHuman.phenotype.traitDict["thirst_threshold"])) {
            currentGoal = "Decrease Thirst";
        }
        else if (thisHuman.driveSystem.stateDict["hunger"] > float.Parse(thisHuman.phenotype.traitDict["hunger_threshold"])) {
            currentGoal = "Decrease Hunger";
        }
        else if (thisHuman.driveSystem.stateDict["sleepiness"] > float.Parse(thisHuman.phenotype.traitDict["sleepiness_threshold"])) {
            currentGoal = "Decrease Sleepiness";
        }
        else if (thisHuman.driveSystem.stateDict["fatigue"] > float.Parse(thisHuman.phenotype.traitDict["fatigue_threshold"])) {
            currentGoal = "Decrease Fatigue";
        }
        else{
            currentGoal = "Decrease Fatigue";
        }
        
        currentSubGoalList = subGoalListDict[currentGoal];
        // foreach(var x in currentSubGoalList) {
        //     Debug.Log(x.ToString());
        // }
        

        for (int i=currentSubGoalList.Count-1; i>= 0; i--){
            string subGoal = currentSubGoalList[i];
            bool subGoalPossible = checkSubGoalPossible(subGoal);
            if (subGoalPossible) {
                currentSubGoal = subGoal;
                currentSubGoalIndex = i;
            }
           
        }
        
        
    }
    

    public bool checkSubGoalPossible(string subGoal) {
        if (subGoal == "Drink Water") {
            if (doingNothing) {
                return true;
            }
        }
        if (subGoal == "GoTo Water") {
            if (checkIfTargetVisible("water")) {
                if (!checkIfTargetReachable()) {
                    return true;
                }
            }
        }
        if (subGoal == "SearchFor Water") {
            if(!checkIfTargetVisible("water")) {
                return true;
            }
        }
        if (subGoal == "SearchFor Water") {
            return true;
        }
        return false;
    }
    

    public bool checkIfTargetVisible(string targetTag) {
        closestTargetIndex = -1;
        float closestTargetDistance = Mathf.Infinity;
        if (thisHuman.fOVDetection.objects_in_vision.Count != 0) {
            for (int i = 0; i < thisHuman.fOVDetection.objects_in_vision.Count; i++) {
                    if (targetTag != null) {
                        if (thisHuman.fOVDetection.objects_in_vision[i].CompareTag(targetTag)) {
                        knowledgeDict["waterVisible"] = true;
                        var distance = Vector3.Distance(thisHuman.gameObject.transform.position, thisHuman.fOVDetection.objects_in_vision[i].transform.position);
                        
                        if (distance < closestTargetDistance) {
                            closestTargetDistance = distance;
                            closestTargetIndex = i;
                        }
                    }
                    else {
                        knowledgeDict["waterVisible"] = false;
                    }
                }
                
            }
        }
        else {
            knowledgeDict["waterVisible"] = false;
            
        }
        if (knowledgeDict["waterVisible"]) {
            return true;
        }
        else {
            return false;
        }
    }

    public bool checkIfTargetReachable() {
        if (closestTargetIndex >= 0) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, thisHuman.fOVDetection.objects_in_vision[closestTargetIndex].transform.position);
            if (distance < 1 && thisHuman.humanMotorSystem.velocity == 0) {
                knowledgeDict["waterReachable"] = true;
            }
            else {
                knowledgeDict["waterReachable"] = false;
            }
        }
        if (knowledgeDict["waterReachable"]) {
            return true;
        }
        else {
            return false;
        }
    }

/// THIS IS ALL CODE THAT GETS CALLED BY chooseCurrentAction
// 

    public void goToObject(GameObject target) {
        if (target != null) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, target.transform.position);
            var direction = (target.transform.position - thisHuman.gameObject.transform.position);
            var x = target.transform.position.x - (1/distance) * Mathf.Abs(direction.x);
            var z = target.transform.position.z - (1/distance) * Mathf.Abs(direction.z);
            Vector3 targetPos = new Vector3(x,0,z);
            thisHuman.gameObject.transform.LookAt(target.transform);
            actionChoice.targetPos = targetPos;
            actionChoice.argumentDict["movementVelocity"] = 1.0f;
            actionChoice.actionValueDict["accellerate"] = 1;
        }
    }

    public void searchForThing(float Angle, string targetTag){
        checkIfTargetVisible(targetTag);
        checkIfTargetReachable();
        actionChoice.actionValueDict["rotate"] = 1;
        actionChoice.argumentDict["rotationAngle"] = Angle;
        if (thisHuman.humanMotorSystem.rotateAngle == 0) {
            if (!newRandomPos) {
            randomPoint = new Vector3(Random.Range(thisHuman.gameObject.transform.position.x - Range, 
                                thisHuman.gameObject.transform.position.x + Range), 0, 
                                Random.Range(thisHuman.gameObject.transform.position.z - Range, 
                                thisHuman.gameObject.transform.position.z + Range));
                                //Debug.Log(randomPoint  + " and " + (thisHuman.gameObject.transform.position - randomPoint).magnitude);
            newRandomPos = true;
            }
            thisHuman.gameObject.transform.LookAt(randomPoint);
            actionChoice.targetPos = randomPoint;
            actionChoice.argumentDict["movementVelocity"] = 1.0f;
            actionChoice.actionValueDict["accellerate"] = 1;
            if ((thisHuman.gameObject.transform.position - randomPoint).magnitude < 3) {
                newRandomPos = false;
            }
        }

    }
        
    
}


