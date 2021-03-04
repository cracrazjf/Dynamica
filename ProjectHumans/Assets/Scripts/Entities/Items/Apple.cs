using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;

public class Apple : Item {
    public GameObject applePrefab;

    public Apple(int index, Genome passedInfo) : base("Apple", index, passedInfo) {

        applePrefab = Resources.Load("Prefabs/ApplePrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(applePrefab, startPosition, Quaternion.Euler(0f, 1f, 0f)) as GameObject;
        
        
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
    }

    public override void NonlivingObjectUpdate() {
        float poison = this.GetPhenotype().GetTraitDict()["poison"];
        if (poison == 1) {
            this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color(1,0,1,1);
        }
    }
}
