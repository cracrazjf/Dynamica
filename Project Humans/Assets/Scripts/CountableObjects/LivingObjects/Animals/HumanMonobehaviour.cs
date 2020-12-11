using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanMonobehaviour : MonoBehaviour
{
    List<Human> humanList = new List<Human>();

    void Update() {
        for(int i= 0; i< World.animalList.Count; i++) {
            if (World.animalList[i].GetObjectType() == "human") {
                humanList.Add(((Human)World.animalList[i]));
            }
        }
                
    }
    
    // why is this iterating through the whole human list, rather than just calling lines 28,29 or 32,33 from the picking up function?
    // void OnAnimatorIK()
    // {
    //     for(int i= 0; i< humanList.Count; i++) {
    //         if (humanList[i].animator) {

    //             if (humanList[i].nervousSystem.GetActionState(humanList[i].nervousSystem.actionStateIndexDict["picking up with left hand"])) {  
    //                 humanList[i].animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
    //                 humanList[i].animator.SetIKPosition(AvatarIKGoal.LeftHand,  humanList[i].humanSimpleAI2.pickUpPosition);
    //             }
    //             else if (humanList[i].nervousSystem.GetActionState(humanList[i].nervousSystem.actionStateIndexDict["picking up with right hand"])) {
    //                 humanList[i].animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //                 humanList[i].animator.SetIKPosition(AvatarIKGoal.RightHand, humanList[i].humanSimpleAI2.pickUpPosition);
    //             }
    //         }
    //     }
    // }
}