using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanTestAI 
{
    public Human thisHuman;
    public List<float> actionValueList = new List<float>();

    public HumanTestAI (Human human) {
        this.thisHuman = human;
    }

    public List<float> chooseAction(){
        for (int i = 0; i < thisHuman.humanMotorSystem.numActions; i++) {
            actionValueList.Add(0.0f);
        }

        actionValueList[0] = 1.0f;
        return actionValueList;
        
    }
        // "accelerate",   // value -1..1 translating to speed of accel./deccel.
        // "rotate",       // value from -1 to 1, translating into -180..180 degree rotation
        // "sit",          // begin to sit
        // "stand",        // begin to stand
        // "lay",          // begin to lay down
        // "sleep",        // begin to sleep
        // "wake_up",      // begin to wake
        // "pick_up_LH", "pick_up_RH",
        // "set_down_LH","set_down_RH",
        // "put_bag_LH", "put_bag_RH",
        // "get_bag_LH", "get_bag__RH",
        // "eat_LH", "eat_RH",
        // "drink_LH", "drink_RH",
        // "rest"

}

    