using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanSimpleAI2
{
    public Human thisHuman;
    public HumanActionChoice actionChoice;
    public FOVDetection thisfOVDetection;

    public Dictionary<string, bool> knowledgeDict = new Dictionary<string, bool>();
    
    public List<string> stateList = new List<string>();
    Vector3 randomPoint;
    int Range = 10;
    public bool newRandomPos = false;
    public bool drinked = false;

    string currentGoal = "None";

    public string targetTag = "None";

    int closestTargetIndex;
    bool doingNothing;
    
    public HumanSimpleAI2 (Human human) {
        this.thisHuman = human;
        actionChoice = new HumanActionChoice(this.thisHuman.humanMotorSystem.actionLabelList);
        thisfOVDetection = this.thisHuman.gameObject.GetComponent<FOVDetection>();
        initKnowledgeDict();
    }
    
    public void initKnowledgeDict(){
        knowledgeDict.Add("waterVisible", false);
        knowledgeDict.Add("waterReachable", false);
    }

    public HumanActionChoice chooseAction(){
        doingNothing = thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("Default");
        thisHuman.fOVDetection.inFov(thisHuman.gameObject.transform, 45, 10);

        actionChoice.initActionChoices(this.thisHuman.humanMotorSystem.actionLabelList);
         
        if (currentGoal == "Decrease Thirst"){
            decreaseThirst();
            targetTag = "water";
            foreach(KeyValuePair<string, bool> x in knowledgeDict) {
            Debug.Log(x.Key);
            }
            Debug.Log(targetTag); 
        }
        else if (currentGoal == "Decrease Sleepiness"){

        }
        else if (currentGoal == "Decrease Hunger"){

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
        
        if (thisHuman.driveSystem.stateDict["thirst"] > float.Parse(thisHuman.phenotype.traitDict["thirst_threshold"])) {
            currentGoal = "Decrease Thirst";
        }
        if (thisHuman.driveSystem.stateDict["hunger"] > float.Parse(thisHuman.phenotype.traitDict["hunger_threshold"])) {
            currentGoal = "Decrease Hunger";
        }
        if (thisHuman.driveSystem.stateDict["sleepiness"] > float.Parse(thisHuman.phenotype.traitDict["sleepiness_threshold"])) {
            currentGoal = "Decrease Sleepiness";
        }
        if (thisHuman.driveSystem.stateDict["fatigue"] > float.Parse(thisHuman.phenotype.traitDict["fatigue_threshold"])) {
            currentGoal = "Decrease Fatigue";
        }
        if (thisHuman.driveSystem.stateDict["health"] < float.Parse(thisHuman.phenotype.traitDict["health_threshold"])) {
            currentGoal = "Increase Health";
        }
        
    }

    public void decreaseThirst(){
        if (!knowledgeDict["waterReachable"]) {
            if (!knowledgeDict["waterVisible"]) {
                checkIfTargetVisible("water");
                rotate(1.0f);
                searchForThing();
            }
            else {
                checkIfTargetReachable();
                if(closestTargetIndex >= 0) {
                    goToObject(thisfOVDetection.objects_in_vision[closestTargetIndex]);
                }
                
            }
            
        }
        else{
            actionChoice.actionValueDict["drink"] = 1;
            thisHuman.driveSystem.stateDict["thirst"] = 0;
            drinked = true;
            
        }
        if (drinked && thisHuman.doingNothing) {
            thisHuman.humanMotorSystem.rotateAngle = 360;
            currentGoal = "None";
            knowledgeDict["waterReachable"] = false;
            knowledgeDict["waterVisible"] = false;
        }
        
        
    }


    public bool checkIfTargetVisible(string targetTag) {
        closestTargetIndex = -1;
        float closestTargetDistance = Mathf.Infinity;
        if (thisfOVDetection.objects_in_vision.Count != 0) {
            for (int i = 0; i < thisfOVDetection.objects_in_vision.Count; i++) {
                    if (targetTag != null) {
                        if (thisfOVDetection.objects_in_vision[i].CompareTag(targetTag)) {
                        knowledgeDict["waterVisible"] = true;
                        var distance = Vector3.Distance(thisHuman.gameObject.transform.position, thisfOVDetection.objects_in_vision[i].transform.position);
                        
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
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, thisfOVDetection.objects_in_vision[closestTargetIndex].transform.position);
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



    
    public void goToObject(GameObject target) {
        if (target != null) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, target.transform.position);
            thisHuman.gameObject.transform.LookAt(target.transform);
            actionChoice.actionValueDict["accellerate"] = 1;
            actionChoice.argumentDict["accellerationRate"] = 0.2f;
            if (thisHuman.humanMotorSystem.velocity > 1) {
                actionChoice.actionValueDict["accellerate"] = 1;
                actionChoice.argumentDict["accellerationRate"] = 0.0f;
            }
            if (distance < 2) {
                actionChoice.actionValueDict["accellerate"] = 1;
                actionChoice.argumentDict["accellerationRate"] = (0 - 1)/9.8f;
            }
        }
    }
    public void rotate(float Angle) {
        actionChoice.actionValueDict["rotate"] = 1;
        actionChoice.argumentDict["rotationAngle"] = Angle;
    }

    public void searchForThing(){
        if (thisHuman.humanMotorSystem.rotateAngle == 0) {
            if (!newRandomPos) {
            randomPoint = new Vector3(Random.Range(thisHuman.gameObject.transform.position.x - Range, 
                                thisHuman.gameObject.transform.position.x + Range), 0, 
                                Random.Range(thisHuman.gameObject.transform.position.z - Range, 
                                thisHuman.gameObject.transform.position.z + Range));
                                Debug.Log(randomPoint  + " and " + (thisHuman.gameObject.transform.position - randomPoint).magnitude);
            newRandomPos = true;
            }
            thisHuman.gameObject.transform.LookAt(randomPoint);
            actionChoice.actionValueDict["accellerate"] = 1;
            actionChoice.argumentDict["accellerationRate"] = 0.2f;
            if (thisHuman.humanMotorSystem.velocity > 1) {
                actionChoice.actionValueDict["accellerate"] = 1;
                actionChoice.argumentDict["accellerationRate"] = 0.0f;
            }
            if ((thisHuman.gameObject.transform.position - randomPoint).magnitude < 3) {
                newRandomPos = false;
            }
        }

    }
        
    
}

/*
            called .1 seconds

            .2 seconds to run
            knowledgeDict['waterReachable'] = False;
            knowledgeDict['waterVisible'] = False;

            while knowledgeDict['waterReachable'] == False:
                while knowledgeDict['waterVisible'] == False:
                    foundWater = checkIfTargetVisible(water)
                    if foundWater:
                        knowledgeDict["waterVisible"] = True
                goto(water)

            actionChoice.actionValueDict["drink"] = 1


        
        bool checkIfTargetVisible(targetTag)
            int closestTargetIndex = -1
            float closestTargetDistance = 10000000000.0f

            object_list = get_object_list()
            for (int i=0; i < len(object_list); i++):
                if objectTag == targetTag:
                    knowledgeDict['waterVisible'] = True
                    distance = getDistance(objectList[closestTargetDistance])

                    if water_distance < closestWaterDistance:
                        closestWaterDistance = waterDistance
                        closestWaterIndex = i
            return True or False depending on if target was found


        */

        //Debug.Log(knowledgeDict["waterReachable"]);

         // while (!knowledgeDict["waterReachable"]) {
        //     while(!knowledgeDict["waterVisible"]) {
        //         rotate(1.0f);
        //     }
        //     goToObject(thisfOVDetection.objects_in_vision[closestTargetIndex]);
        // }
        // actionChoice.actionValueDict["drink"] = 1;


