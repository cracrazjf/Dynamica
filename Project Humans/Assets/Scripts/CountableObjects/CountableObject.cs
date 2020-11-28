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
    private int index;
    private float nutrition;
    private float healthEffect;
    private String name;
    
    // unity variables
    public GameObject gameObject;
    public Animator animator;

    public CountableObject(string objectType, int index,  float nutrition, float healthEffect) {
        SetObjectType(objectType);
        SetIndex(index);
        SetNutrition(nutrition);
        SetHealthEffect(healthEffect);

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

    public float GetNutrition() {
        return nutrition;
    }

    public float GetHealthEffect() {
        return healthEffect;
    }

    public string GetObjectType() {
        return objectType;
    }

    public int GetIndex() {
        return index;
    }

    public String GetName() {
        return name;
    }

    public void SetNutrition(float passed) {
        nutrition = passed;
    }

    public void SetHealthEffect(float passed) {
        healthEffect = passed;
    }

    public void SetObjectType(string passed) {
        objectType = passed;
    }

    public void SetIndex(int passed) {
        index = passed;
    }

    public void SetName(String passed) {
        name = passed;
    }

}
