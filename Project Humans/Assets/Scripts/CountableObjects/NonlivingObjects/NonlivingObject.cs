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

abstract public class NonlivingObject : CountableObject
{
    // Traits
    public int numTraits = 0;
    public List<String> traitLabelList = new List<string>();
    public Dictionary<string, int> traitIndexDict = new Dictionary<string, int>();
    public Dictionary<string, float> traitDict = new Dictionary<string, float>();
    public Dictionary<string, bool> traitDisplayDict = new Dictionary<string, bool>();

    // State
    public int numStates = 0;
    public List<String> stateLabelList = new List<string>();
    public Dictionary<string, int> stateIndexDict = new Dictionary<string, int>();
    public Dictionary<string, float> stateDict = new Dictionary<string, float>();
    public Dictionary<string, bool> stateDisplayDict = new Dictionary<string, bool>();


}
