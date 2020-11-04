using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanSimpleAI
{
    public Human thisHuman;
    public HumanActionChoice actionChoice;
    public FOVDetection thisfOVDetection;
    
    public List<string> stateList = new List<string>();
    Vector3 randomPoint;
    int Range = 10;
    public bool newRandomPos = false;
    public bool drinked = false;
    

    public HumanSimpleAI (Human human) {
        this.thisHuman = human;
        actionChoice = new HumanActionChoice(this.thisHuman.humanMotorSystem.actionLabelList);
        thisfOVDetection = this.thisHuman.gameObject.GetComponent<FOVDetection>();
    }
    public double counter = 0;
    
    
    public HumanActionChoice chooseAction(){
        float hand = 0.0f;
        float accellerationRate = 0.0f;
        float rotationAngle = 0.0f;
        actionChoice.argumentDict.Clear();
        actionChoice.actionValueDict.Clear();
        actionChoice.initActionChoices(this.thisHuman.humanMotorSystem.actionLabelList);
        string actionChoiceLabel = null;
        
        // sleep and wake;
        // Debug.Log(thisHuman.driveSystem.stateDict["wakefulness"]);
        if(thisHuman.driveSystem.stateDict["wakefulness"] < float.Parse(thisHuman.phenotype.traitDict["sleepiness_threshold"])) {
            stateList.Add("sleepy");
            if (stateList.Contains("sleepy")) {
                actionChoiceLabel = "sleep";
                thisHuman.phenotype.traitDict["wakefulness_change"] = "0.01";
            }
        }
        
        if (thisHuman.driveSystem.stateDict["wakefulness"] == 1 && stateList.Contains("sleepy")) {
            actionChoiceLabel = "wake_up";
            stateList.Add("Sit");
            stateList.Remove("sleepy");
            thisHuman.phenotype.traitDict["wakefulness_change"] = "-0.01";
            
            
        }
        if (thisHuman.animator.GetCurrentAnimatorStateInfo(0).IsName("SitLoop")){
            actionChoiceLabel = "stand_up";
                
        }

        
        // Debug.Log(thisHuman.driveSystem.stateDict["thirst"]);
        if (thisHuman.driveSystem.stateDict["thirst"] < float.Parse(thisHuman.phenotype.traitDict["thirst_threshold"])) {
            if (thisfOVDetection.objects_in_vision.Count != 0) {
                for (int i = 0; i < thisfOVDetection.objects_in_vision.Count; i++) {
                    if (thisfOVDetection.objects_in_vision[i].CompareTag("water")) {
                        var distance = Vector3.Distance(thisHuman.gameObject.transform.position, thisfOVDetection.objects_in_vision[i].transform.position);
                        if (distance <= 1.3 && thisHuman.humanMotorSystem.velocity  == 0) {
                            actionChoiceLabel = "drink";
                            
                            thisHuman.driveSystem.stateDict["thirst"] = 1;
                            
                            
                        }
                        else {
                            thisHuman.gameObject.transform.LookAt(thisfOVDetection.objects_in_vision[i].transform);
                            actionChoiceLabel = "accellerate";
                            accellerationRate = 0.2f;
                            if (thisHuman.humanMotorSystem.velocity > 1) {
                                actionChoiceLabel = "accellerate";
                                accellerationRate = 0;
                            }
                            if (distance < 2) {
                                actionChoiceLabel = "accellerate";
                                accellerationRate = (0 - 1)/9.8f;
                            }
                        }
                    }
                    else {

                        actionChoiceLabel = "rotate";
                        rotationAngle = 1.0f;
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
                            actionChoiceLabel = "accellerate";
                            accellerationRate = 0.2f;
                            if (thisHuman.humanMotorSystem.velocity > 1) {
                                actionChoiceLabel = "accellerate";
                                accellerationRate = 0;
                            }
                            if ((thisHuman.gameObject.transform.position - randomPoint).magnitude < 3) {
                                newRandomPos = false;
                            }
                        }
                        

                    }
                }
            }
            else {

                actionChoiceLabel = "rotate";
                rotationAngle = 1.0f;
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
                    actionChoiceLabel = "accellerate";
                    accellerationRate = 0.2f;
                    if (thisHuman.humanMotorSystem.velocity > 1) {
                        actionChoiceLabel = "accellerate";
                        accellerationRate = 0;
                    }
                    if ((thisHuman.gameObject.transform.position - randomPoint).magnitude < 3) {
                            newRandomPos = false;
                    }
                }
            } 
        }
        if (thisHuman.driveSystem.stateDict["thirst"] == 1) {
            thisHuman.humanMotorSystem.rotateAngle = 360;
        }
        

        if(actionChoiceLabel != null) {
            actionChoice.actionValueDict[actionChoiceLabel] = 1;

        }
        

        if (actionChoiceLabel == "accellerate"){
            actionChoice.argumentDict["accellerationRate"] = accellerationRate;
        }

        else if (actionChoiceLabel == "rotate"){
            actionChoice.argumentDict["rotationAngle"] = rotationAngle;
        }

        else if (actionChoiceLabel == "pick_up" || actionChoiceLabel == "set_down" || actionChoiceLabel == "put_in" || actionChoiceLabel == "get_from" || actionChoiceLabel == "eat"){
            actionChoice.argumentDict["hand"] = hand;
        }
        
        return actionChoice;
        
    }

    
}


    