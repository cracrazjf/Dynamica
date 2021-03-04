using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;


abstract public class Item : Entity {
    public Item(string objectType, int index, Genome motherGenome, Transform spawn) : 
            base (objectType, index, motherGenome, spawn) {}


    public override void UpdateEntity() {
        if (this.objectType == "Apple") {
            float poison = this.GetPhenotype().GetTraitDict()["poison"];
            if (poison == 1) {
                this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color(1,0,1,1);
            }
        }
    }
}