using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimateBody : AnimalBody {

    public Vector3 localStartRight;
    public Vector3 localStartLeft;

    public PrimateBody(Animal animal, Vector3 position) : base(animal, position) {
        localStartLeft = GetSkeleton("Hand_L").transform.localPosition;
        localStartRight = GetSkeleton("Hand_R").transform.localPosition;

        // so in the future we can move hand pos to localStart + (proportion * max)
    }

    public override void UpdateBodyStates() {
        SetState("sitting", CheckSitting());
        SetState("laying", CheckSitting());
    }
    
    public override bool CheckSitting() {
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(abdomen.transform.position, -abdomen.transform.up, bodyExtent + 0.2f);
    }

    bool CheckLaying() {
        return (abdomen.transform.position.y < 1f);
    }
}