using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using UnityEngine;

// TEMP
//using System.Text.Json;

// const string FILE_NAME = "test.json"

// TODO: Figure out file readout based off calling class (without overrides)

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
