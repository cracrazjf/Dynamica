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
    public new String objectType;
    
    // unity variables
    public GameObject gameObject;
    public Animator animator;

    public void Start() {

    }

    public void Update() {

    }

}
