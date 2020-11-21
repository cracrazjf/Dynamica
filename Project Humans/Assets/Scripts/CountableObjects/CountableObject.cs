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
    private static String objectType;
    private static float nutrition;
    private static float healthEffect;
    
    // unity variables
    public GameObject gameObject;
    public Animator animator;

    public CountableObject(string objectType, float nutrition, float healthEffect) {

    }

    public void Start() {}

    public void Update() {}

    public static float GetNutrition() {
        return nutrition;
    }

    public static float GetHealthEffect() {
        return healthEffect;
    }

    public static string GetObjectType() {
        return objectType;
    }

    public static void SetNutrition(float passed) {
        nutrition = passed;
    }

    public static void SetHealthEffect(float passed) {
        healthEffect = passed;
    }

    public static void SetObjectType(string passed) {
        objectType = passed;
    }
}
