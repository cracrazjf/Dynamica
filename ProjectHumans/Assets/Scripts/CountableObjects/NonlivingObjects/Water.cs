using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Water : NonlivingObject
{

    public GameObject waterPrefab;

    public Water(int index, NonlivingObjectInfo passedNonlivingObjectInfo) : base("Water", index, passedNonlivingObjectInfo) {

        waterPrefab = Resources.Load("Prefabs/WaterPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(waterPrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
    }
}
