using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Water : Item {

    public GameObject waterPrefab;

    public Water(int index, Genome passedInfo) : base("Water", index, passedInfo) {

        waterPrefab = Resources.Load("Prefabs/WaterPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(waterPrefab, startPosition, Quaternion.Euler(0f, 1f, 0f)) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
    }
}
