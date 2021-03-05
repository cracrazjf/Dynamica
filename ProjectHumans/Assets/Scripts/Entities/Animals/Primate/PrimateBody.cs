using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimateBody : AnimalBody {

    public PrimateBody(Animal animal, Vector3 position) : base(animal, position) {
    }

    public override void UpdateBodyStates() {
        SetState("sitting", CheckSitting());
        SetState("laying", CheckSitting());
    }
    
    bool CheckSitting() {
        float bodyExtent = abdomen.GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(abdomen.transform.position, -abdomen.transform.up, bodyExtent + 0.2f);
    }

    bool CheckLaying() {
        return (abdomen.transform.position.y < 1f);
    }
}