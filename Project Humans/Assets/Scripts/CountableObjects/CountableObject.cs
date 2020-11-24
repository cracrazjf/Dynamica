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
    private static int index;
    private static float nutrition;
    private static float healthEffect;
    public String name;
    
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

    public static float GetNutrition() {
        return nutrition;
    }

    public static float GetHealthEffect() {
        return healthEffect;
    }

    public static string GetObjectType() {
        return objectType;
    }

    public static int GetIndex() {
        return index;
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

    public static void SetIndex(int passed) {
        index = passed;
    }

}
