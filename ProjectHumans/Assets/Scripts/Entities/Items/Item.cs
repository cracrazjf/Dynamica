using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;


public class Item : Entity {
    public Item(string objectType, int index, Genome motherGenome, Vector3 spawn) 
    : base (objectType, index, motherGenome, spawn) {
        body = new Body(this, spawn);
    }

    public override void UpdateEntity() {
        if (this.GetSpecies() == "Apple") {
            float poison = this.GetPhenotype().GetTraitDict()["poison"];
            if (poison == 1) {
                this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color(1,0,1,1);
            }
        }
    }
}