using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;

public class Apple : NonlivingObject
{
    public GameObject applePrefab;

    public Apple(int index, NonlivingObjectInfo passedNonlivingObjectInfo) : base("Apple", index, passedNonlivingObjectInfo) {

        applePrefab = Resources.Load("Prefabs/ApplePrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(applePrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        
        
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
    }

    public override void NonlivingObjectLateUpdate() {
        float poison = this.GetConstant("poison");
        if (poison == 1) {
            this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color(1,0,1,1);
        }
    }
}
