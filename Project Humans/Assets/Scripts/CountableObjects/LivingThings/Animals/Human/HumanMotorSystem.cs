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

    public List<string> actionLabelList = new List<string>{ "accelerate",   // value -1..1 translating to speed of accel./deccel.
                                                    "rotate",       // value from -1 to 1, translating into -180..180 degree rotation
                                                    "sit",          // begin to sit
                                                    "stand",        // begin to stand
                                                    "lay",          // begin to lay down
                                                    "sleep",        // begin to sleep
                                                    "wake_up",      // begin to wake
                                                    "pick_up_LH", "pick_up_RH",
                                                    "set_down_LH","set_down_RH",
                                                    "put_bag_LH", "put_bag_RH",
                                                    "get_bag_LH", "get_bag__RH",
                                                    "eat_LH", "eat_RH",
                                                    "drink_LH", "drink_RH",
                                                    "rest"};  // do we want rest to be explicit, or just not doing anything else

    public int numActions; // 15
    public Dictionary<string, int> actionIndexDict = new Dictionary<string, int>();
    public List<float> actionValueList = new List<float>(); // [1 0 0 0 0 0 1 0 0 0 0 1 0]
    public Dictionary<string, bool> actionDisplayDict = new Dictionary<string, bool>();

    public HumanMotorSystem(Human human) {
        this.thisHuman = human;
        for (int i = 0; i < actionLabelList.Count; i++){
            actionIndexDict.Add(actionLabelList[i], i);
            actionDisplayDict.Add(actionLabelList[i], true);
        }
    }

    public void takeAction(List<float> actionValueList){
        // find the highest value in actionValueList, and call the function for that action
        var maxValue = Mathf.Max(actionValueList.ToArray());
        int maxIndex = actionValueList.IndexOf(maxValue);
        if (maxIndex == 0){
            accellerate();
        }
    }

    public void accellerate(){

    }

}



