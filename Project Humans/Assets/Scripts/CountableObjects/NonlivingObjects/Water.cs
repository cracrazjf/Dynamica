using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Water : NonlivingObject
{

    public GameObject waterPrefab;

    public Water(int index, Nullable<Vector3> position, Dictionary<string,List<string>> propertyDict) : base("Water", index, position, propertyDict) {

        waterPrefab = Resources.Load("WaterPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(waterPrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
    }
}
