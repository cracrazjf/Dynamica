using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : NonlivingObject
{
    public static float worldSize = 20.0f;
    public static float maxPosition = World.worldSize / 2;
    public static float minPosition = -World.worldSize / 2;
    public GameObject applePrefab;

    public Apple(string objectType) : base(objectType, 10f, 10f) {
        Vector3 startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 0f, Random.Range(minPosition,maxPosition));

        applePrefab = Resources.Load("ApplePrefab",typeof(GameObject)) as GameObject;
        this.gameObject = GameObject.Instantiate(applePrefab, startPosition, Quaternion.Euler(0f, 0f, 0f)) as GameObject;// instantiate
        this.gameObject.name = GetObjectType(); 

        gameObject.SetActive(true);
        
    }

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {}
}
