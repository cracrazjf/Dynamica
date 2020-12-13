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

    public Apple(int index, Nullable<Vector3> position, NonlivingObjectInfo passedNonlivingObjectInfo) : base("Apple", index, position, passedNonlivingObjectInfo) {

        applePrefab = Resources.Load("ApplePrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(applePrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        float poison = this.GetConstant("poison");
        if (poison == 1) {
            this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color(1,0,1,1);
        }
        
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
    }
}
