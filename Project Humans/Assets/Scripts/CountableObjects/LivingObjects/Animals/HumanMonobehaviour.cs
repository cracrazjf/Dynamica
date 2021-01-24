using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanMonobehaviour : MonoBehaviour
{
    public Human thisHuman;
    void OnAnimatorIK()
    {
        if (this.thisHuman.animator)
        {
            if (this.thisHuman.GetMotorSystem().GetActionState(this.thisHuman.GetMotorSystem().getActionStateIndex("picking up")))
            {
                Debug.Log("here");
                if (this.thisHuman.animator.GetFloat("left/rightHand") == 0 
                    && !this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with left hand")))
                {
                    this.thisHuman.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
                    this.thisHuman.animator.SetIKPosition(AvatarIKGoal.LeftHand, new Vector3
                        (this.thisHuman.getActionArgumentArray()[this.thisHuman.GetMotorSystem().GetActionArgumentIndex("hand target x")],
                        this.thisHuman.getActionArgumentArray()[this.thisHuman.GetMotorSystem().GetActionArgumentIndex("hand target y")],
                        this.thisHuman.getActionArgumentArray()[this.thisHuman.GetMotorSystem().GetActionArgumentIndex("hand target z")]));
                }
                else if (this.thisHuman.animator.GetFloat("left/rightHand") == 1 
                    && !this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with right hand")))
                {
                    this.thisHuman.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);
                    this.thisHuman.animator.SetIKPosition(AvatarIKGoal.RightHand, new Vector3
                        (this.thisHuman.getActionArgumentArray()[this.thisHuman.GetMotorSystem().GetActionArgumentIndex("hand target x")],
                        this.thisHuman.getActionArgumentArray()[this.thisHuman.GetMotorSystem().GetActionArgumentIndex("hand target y")],
                        this.thisHuman.getActionArgumentArray()[this.thisHuman.GetMotorSystem().GetActionArgumentIndex("hand target z")]));
                }
            }
        }
    }

    public void SetHuman(Human passedHuman){
        this.thisHuman = passedHuman;
    }

}