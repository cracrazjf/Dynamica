using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class PrimateBody : AnimalBody {

    public Vector3 abAdjRight;
    public Vector3 abAdjLeft;
    public Vector3 footAdjRight;
    public Vector3 footAdjLeft;

    public PrimateBody(Animal animal, Vector3 position) : base(animal, position) {
        abAdjLeft = GetSkeleton("Hand_L").transform.position - GetSkeleton("Abdomen").transform.position; 
        abAdjRight = GetSkeleton("Hand_R").transform.position - GetSkeleton("Abdomen").transform.position;

        footAdjLeft = GetSkeleton("Foot_L").transform.position - GetSkeleton("Abdomen").transform.position; 
        footAdjRight = GetSkeleton("Foot_R").transform.position - GetSkeleton("Abdomen").transform.position;  

        LegList = new List<string>();
        LegList.Add("Femur_R");
        LegList.Add("Femur_L");
        
    }

    public override void UpdateBodyStates() {
        if (CheckSitting()) {
            SetState("sitting", 1f);
        }
        if (CheckLaying()) {
            SetState("laying", 1f);
        }
    }
    
    public override bool CheckSitting() {
        // float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        // return Physics.Raycast(abdomen.transform.position, -abdomen.transform.up, bodyExtent + 0.2f);
        return CheckCrouchingTop();
    }

    public override bool CheckCrouching() { 
        Debug.Log("Used wrong CheckCrounching");
        return true;
    }


    public bool CheckCrouchingBottom() { 
        Vector3 toSend = abdomen.transform.position;
        double minCrouchHeight = GetHeight()/1.5 + 0.5;
        //Debug.Log("Checking to see if more " + toSend.y + " " + minCrouchHeight);
        return (toSend.y > minCrouchHeight);
    }

    public bool CheckCrouchingTop() { 
        Vector3 toSend = abdomen.transform.position;
        double minCrouchHeight = GetHeight()/1.5 + 0.5;
        //Debug.Log("Checking to see if less " + toSend.y + " " + minCrouchHeight);
        return (toSend.y < minCrouchHeight);
    }

    public override bool CheckLaying() {
        return (abdomen.transform.position.y < 1f);
    }
}