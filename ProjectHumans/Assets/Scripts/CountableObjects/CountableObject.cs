using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;

abstract public class CountableObject
{
    public string displayName;
    private String objectType;
    private String name;
    private int index;
    public Vector3 startPosition;
    public Quaternion startRotation;
    public int age;
    
    // unity variables
    public GameObject gameObject;
    public Animator animator;

    public CountableObject(string objectType, int index) {
        SetObjectType(objectType);
        SetIndex(index);
        name = (objectType + " " + index.ToString());

        startPosition = chooseStartPosition(null);
        startRotation = chooseStartRotation();
    }

    public Vector3 chooseStartPosition(Nullable<Vector3> position){
        Vector3 newStartPosition = new Vector3();
        if (position != null) {
            newStartPosition = (Vector3)position;
        } else { 
            newStartPosition = new Vector3 (Random.Range(World.minPosition, World.maxPosition), 0.5f, Random.Range(World.minPosition, World.maxPosition)); 
        }
        return newStartPosition;
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

    public void SetAge(int newAge){
        this.age = newAge;
    }

    public int GetAge(){
        return this.age;
    }

    public string GetDisplayName() {
        return displayName;
    }

    public void SetDisplayName(string named) {
        this.displayName = named;
    }

    public void IncreaseAge(int amount){
        this.age += amount;
    }

}
