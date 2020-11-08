using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))] 

public class IKControl : MonoBehaviour {
    
    protected Animator animator;
    
    public bool ikActive = false;
    public Transform rightFootObj = null;
    public Transform leftFootObj = null;


    void Start () 
    {
        animator = GetComponent<Animator>();
    }
    
    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if(animator) {
            
            //if the IK is active, set the position and rotation directly to the goal. 
            if(ikActive) {

                // Set the look target position, if one has been assigned
                // if(lookObj != null) {
                //     animator.SetLookAtWeight(1);
                //     animator.SetLookAtPosition(lookObj.position);
                // }    

                // Set the right hand target position and rotation, if one has been assigned
                if(rightFootObj != null) {
                   
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot,1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot,1);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot,rightFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot,rightFootObj.rotation);
                }
                if (leftFootObj != null) {
                     animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot,1);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot,leftFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot,leftFootObj.rotation);

                }
            }
            
            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {          
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot,0); 

                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot,0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot,0); 

            }
        }
    }    
}
