using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;

public class Apple : NonlivingObject
{
    public static float worldSize = 20.0f;
    public static float maxPosition = World.worldSize / 2;
    public static float minPosition = -World.worldSize / 2;
    public GameObject applePrefab;

    public Apple(int index, Nullable<Vector3> position, Dictionary<string,List<string>> propertyDict) : base("Apple", index, position, propertyDict) {

        applePrefab = Resources.Load("ApplePrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(applePrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
        if(this.propertyDict["color"][0] == "purple") {
            this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color(1,0,1,1);
        }
        
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
    }
}
