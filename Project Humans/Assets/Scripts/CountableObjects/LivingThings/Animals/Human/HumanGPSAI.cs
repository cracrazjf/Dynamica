using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanGPSAI 
{

    public Human thisHuman;
    public List<float> actionValueList = new List<float>();

    public HumanGPSAI (Human human) {
        this.thisHuman = human;
    }

    public List<float> chooseAction(){
        for (int i = 0; i < thisHuman.humanMotorSystem.numActions; )
            actionValueList.Add(0.0f);
        


        return actionValueList;
        
    }

}