using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanGPSAI 
{
    public Human thisHuman;
    public HumanActionChoice actionChoice;
    public FOVDetection thisfOVDetection;
    public string actionChoiceLabel;
    public string hand;

    public HumanGPSAI (Human human) {
        this.thisHuman = human;
        actionChoice = new HumanActionChoice(this.thisHuman.humanMotorSystem.actionLabelList);
        thisfOVDetection = this.thisHuman.gameObject.GetComponent<FOVDetection>();
    }
    public float counter = 0;


    public HumanActionChoice chooseAction(){


        return actionChoice;
        
    }
}
