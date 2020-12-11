using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanMonobehaviour : MonoBehaviour
{
    Human thisHuman;

    
    public HumanMonobehaviour(Human passedHuman) {
        this.thisHuman = passedHuman;
    }

    void Update(){
            
    }
    
    // why is this iterating through the whole human list, rather than just calling lines 28,29 or 32,33 from the picking up function?

    void OnAnimatorIK()
    {
        if (this.thisHuman.animator){
            if (this.thisHuman.GetMotorSystem().GetActionState(this.thisHuman.GetMotorSystem().getActionStateIndex("picking up"))) {
                int index = this.thisHuman.GetMotorSystem().GetActionArgumentIndex("hand");
                if (this.thisHuman.GetMotorSystem().GetActionArgumentArray()[index] == 0){
                    this.thisHuman.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
                    this.thisHuman.animator.SetIKPosition(AvatarIKGoal.LeftHand,  this.thisHuman.humanSimpleAI2.pickUpPosition);
                }
                else if (this.thisHuman.GetMotorSystem().GetActionArgumentArray()[index] == 1){      
                    this.thisHuman.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    this.thisHuman.animator.SetIKPosition(AvatarIKGoal.RightHand, this.thisHuman.humanSimpleAI2.pickUpPosition);
                }

            }
        }
    }

    public void SetHuman(Human passedHuman){
        this.thisHuman = passedHuman;
    }

}