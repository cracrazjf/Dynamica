using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanTestAI 
{
    public Human thisHuman;
    public HumanActionChoice actionChoice;
    public FOVDetection thisfOVDetection;
    public string actionChoiceLabel;
    public string hand;

    public HumanTestAI (Human human) {
        this.thisHuman = human;
        actionChoice = new HumanActionChoice(this.thisHuman.humanMotorSystem.actionLabelList);
        thisfOVDetection = this.thisHuman.gameObject.GetComponent<FOVDetection>();
    }
    public float counter = 0;


    public HumanActionChoice chooseAction(){
        actionChoiceLabel = "sit";
        float hand = 0.0f;

        
        actionChoice.argumentDict.Clear();
        actionChoice.actionValueDict.Clear();
        actionChoice.initActionChoices(this.thisHuman.humanMotorSystem.actionLabelList);
        
        actionChoice.actionValueDict[actionChoiceLabel] = 1;

        if (actionChoiceLabel == "accellerate"){
            actionChoice.argumentDict["accellerationRate"] = 0.2f;
        }

        else if (actionChoiceLabel == "rotate"){
            actionChoice.argumentDict["rotationAngle"] = 0.2f;
        }

        else if (actionChoiceLabel == "pick_up" || actionChoiceLabel == "set_down" || actionChoiceLabel == "put_in" || actionChoiceLabel == "get_from" || actionChoiceLabel == "eat"){
            actionChoice.argumentDict["hand"] = hand;
        }

        return actionChoice;
        
    }
}


    