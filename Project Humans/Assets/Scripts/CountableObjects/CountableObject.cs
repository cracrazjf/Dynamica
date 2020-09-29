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

abstract public class CountableObject : MonoBehaviour
{
    public new String name;
    //public Animator animator;

    // Traits

    public int NumTraits;
    //GameObject prefab;
    public List<String> TraitLabelList;
    public Dictionary<String, int> TraitIndexDict;
    public List<float> TraitValueList;
    public List<bool> TraitDisplayList;

    // State

    public int NumStates;
    public List<String> StateLabelList;
    public Dictionary<String, int> StateIndexDict;
    public List<float> StateValueList;
    public List<bool> StateDisplayList;

    public CountableObject() {
        
    }

    public void Start() {

    }

    public void Update() {

    }

    public void InitObject() {
        List<List<String>> attributesFromFile = this._LoadAttributes();

        this.TraitLabelList = attributesFromFile[0];
        this.StateLabelList = attributesFromFile[1];

        this.NumTraits = this.TraitLabelList.Count;
        this.NumStates = this.StateLabelList.Count;

        this.TraitIndexDict = new Dictionary<String, int>();
        this.StateIndexDict = new Dictionary<String, int>();

        this.TraitValueList = new List<float>();
        this.StateValueList = new List<float>();

        for (int i = 0; i < this.NumTraits; i++) {
            this.TraitIndexDict.Add(TraitLabelList[i], i);
            this.TraitValueList.Add(0);
            this.TraitDisplayList.Add(true);
        }

        for (int i = 0; i < this.NumStates; i++) {
            this.StateIndexDict.Add(StateLabelList[i], i);
            this.StateValueList.Add(0);
            this.StateDisplayList.Add(true);
        }

    }
    
    // TODO: finish _LoadAttributes()
    private List<List<String>> _LoadAttributes() {

         String fileString = File.ReadAllText("FILE_NAME"); // should include using system.IO, and should include "" for File_Name;

         String[] lines = fileString.Split('\n'); // string.split returns a array of string;

         List<String> labels = new List<String>();
         List<float> values = new List<float>();
         List<List<String>> attr = new List<List<String>>();

         for (int i = 0; i < lines.Length; i++) {

            String line = lines[i]; // do not know why should write const

             if (line.Contains("=")) { // changed '=' to "="
                String[] current = line.Split('='); // should not use List for this, split should be Split; and "=" should be '='

                 if (current.Length == 2) {
                     labels.Add(current[0]);

                     if (current[1] == "false") {
                         // pass false
                     } else if (current[1] == "true") {
                         // pass true
                     } else if (current[1].Contains(",")) {
                        current[1].Split(',');
                     } else {
                        float.Parse(current[1]);
                     }

                 }

             } else {
                 // sys out comment line
                 Debug.Log(line);
            }

        }



        /*
            for trait in traits:
                labels.Add(trait)
                values.Add(traits.Get(trait))
        */

        /*
            Switch:
                includes ',':
                    --> array
                includes isNumber:
                    --> float
                includes true/false:
                    --> bool
                else:
                    error
        
        */
        attr.Add(labels);
        //attr.Add(values); // can not convert float to string;

        return attr;
    }
    
    
};
