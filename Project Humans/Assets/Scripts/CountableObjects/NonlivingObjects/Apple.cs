using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : NonlivingObject
{
    public static float worldSize = 20.0f;
    public static float maxPosition = World.worldSize / 2;
    public static float minPosition = -World.worldSize / 2;
    public GameObject applePrefab;

    public Apple(int index) : base("Apple", index, 10f, 10f) {
        // it could read its nutrition and mass from a config file
        // set those values, including the name, here, instead of passing them into the constructor
        Vector3 startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 0f, Random.Range(minPosition,maxPosition));
        applePrefab = Resources.Load("ApplePrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(applePrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
        
    }
}
