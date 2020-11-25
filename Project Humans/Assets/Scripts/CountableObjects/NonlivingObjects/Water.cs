using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : NonlivingObject
{
    public static float worldSize = 20.0f;
    public static float maxPosition = World.worldSize / 2;
    public static float minPosition = -World.worldSize / 2;
    public GameObject waterPrefab;

    public Water(int index) : base("Water", index, 10f, 10f){
        Vector3 startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 0f, Random.Range(minPosition,maxPosition));
        waterPrefab = Resources.Load("WaterPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(waterPrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        this.gameObject.name = GetName();
        gameObject.SetActive(true);
        
    }
}
