using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;

abstract public class CountableObject
{
    private String objectType;
    private String name;
    private int index;
    
    // unity variables
    public GameObject gameObject;
    public Animator animator;

    public CountableObject(string objectType, int index) {
        SetObjectType(objectType);
        SetIndex(index);
        name = (objectType + " " + index.ToString());
    }

    public Vector3 chooseStartPosition(){
        var startPosition = new Vector3 (Random.Range(World.minPosition,World.maxPosition), 0.03f, Random.Range(World.minPosition,World.maxPosition));
        return startPosition;
    }

    public Quaternion chooseStartRotation(){
        var startRotation = Quaternion.Euler(0.0f, Random.Range(World.minPosition,World.maxPosition), 0.0f);
        return startRotation;
    }

    public string GetObjectType() {
        return objectType;
    }

    public String GetName() {
        return name;
    }

    public int GetIndex() {
        return index;
    }

    public void SetObjectType(string passed) {
        objectType = passed;
    }

    public void SetName(String passed) {
        name = passed;
    }

    public void SetIndex(int passed) {
        index = passed;
    }

    public void SetGameObject(GameObject passed) {
        gameObject = passed;
    }

}
