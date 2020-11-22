using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Missing a namespace;

public class HumanMonobehaviour : MonoBehaviour
{
    void OnAnimatorIK()
        {
            Debug.Log("here");
            // for(int i= 0; i< World.humanList.Count; i++) {
            //     if (World.humanList[i].animator) {
                   
            //         if (((Human) World.humanList[i]).GetActionChoice().actionValueDict["pick_up"] == 1) {
                        
            //             if (World.humanList[i].GetActionChoice().argumentDict["hand"] == 0) {
                            
            //                 World.humanList[i].animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            //                 World.humanList[i].animator.SetIKPosition(AvatarIKGoal.LeftHand, ((Human) World.humanList[i]).humanSimpleAI2.pickUpPosition);
                            
            //             }
            //             else if (World.humanList[i].GetActionChoice().argumentDict["hand"] == 1) {
            //                 World.humanList[i].animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            //                 World.humanList[i].animator.SetIKPosition(AvatarIKGoal.RightHand, ((Human) World.humanList[i]).humanSimpleAI2.pickUpPosition);
            //             }
            //         }
            //     }
            // }
            
        }
}