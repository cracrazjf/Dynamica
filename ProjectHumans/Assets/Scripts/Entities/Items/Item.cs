using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;


public class Item : Entity {
    public Item(string objectType, int index, Genome motherGenome, Vector3 spawn) 
    : base (objectType, index, motherGenome, spawn, false) {
        body = new Body(this, spawn);
    }

    public override void UpdateEntity() {
        if (this.GetSpecies() == "Crabapple") {
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f,0.75f,.25f,1f);
        }
    }
}