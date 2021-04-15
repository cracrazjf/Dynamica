using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class PrimateBody : AnimalBody {

    public Vector3 abAdjRight;
    public Vector3 abAdjLeft;

    public PrimateBody(Animal animal, Vector3 position) : base(animal, position) {
        abAdjLeft = GetSkeleton("Hand_L").transform.position - GetSkeleton("Abdomen").transform.position; 
        abAdjRight = GetSkeleton("Hand_R").transform.position - GetSkeleton("Abdomen").transform.position; 
    }

    public override void UpdateBodyStates() {
        if (CheckSitting()) {
            SetState("sitting", 1f);
        }
        if (CheckLaying()) {
            SetState("Laying", 1f);
        }
    
    }
    
    public override bool CheckSitting() {
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(abdomen.transform.position, -abdomen.transform.up, bodyExtent + 0.2f);
    }

    public override bool CheckCrouching() { 
        Vector3 toSend = abdomen.transform.position;
        double crouchHeight = GetHeight()/1.5 + 0.5;
        return (toSend.y > crouchHeight);
    }

    public override bool CheckLaying() {
        return (abdomen.transform.position.y < 1f);
    }
}