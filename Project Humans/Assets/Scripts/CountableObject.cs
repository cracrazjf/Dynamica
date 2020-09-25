using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

// TEMP
using System.Text.Json;

// const string FILE_NAME = "test.json"

// TODO: Figure out file readout based off calling class (without overrides)

abstract public class CountableObject : MonoBehaviour
{
    String name;
    Animator animator;

    // Traits

    int NumTraits;
    unity_prefab Prefab;
    List<String> TraitLabelList;
    Dictionary<String, int> TraitIndexDict;
    List<float> TraitValueList;
    List<bool> TraitDisplayList;

    // State

    int NumStates;
    List<String> StateLabelList;
    Dictionary<String, int> StateIndexDict;
    List<float> StateValueList;
    List<bool> StateDisplayList;

    CountableObject(String name) {
        this.name = name;
    }

    public void Start() {

    }

    public void Update() {

    }

    public void InitObject() {
        const List<List<String>> attributesFromFile = this._LoadAttributes();

        this.TraitLabelList = attributesFromFile[0];
        this.StateLabelList = attributesFromFile[1];

        this.NumTraits = this.TraitLabelList.length();
        this.NumStates = this.StateLabelList.length();

        this.TraitIndexDict = new Dictionary<String, int>();
        this.StateIndexDict = new Dictionary<String, int>();

        this.TraitValueList = new List<float>();
        this.StateValueList = new List<float>();

        for (int i = 0; i < this.NumTraits; i++) {
            this.TraitIndexDict.Add(TraitLabelList[i], i);
            this.TraitValueList.Add(null);
            this.TraitDisplayList.Add(true);
        }

        for (int i = 0; i < this.NumStates; i++) {
            this.StateIndexDict.Add(StateLabelList[i], i);
            this.StateValueList.Add(null);
            this.StateDisplayList.Add(true);
        }

    }

    // TODO: finish _LoadAttributes()
    private List<List<String>> _LoadAttributes() {

        String fileString = File.ReadAllText(FILE_NAME);

        String lines = fileString.Split('\n');

        List<String> labels = new List<String>();
        List<float> values = new List<float>();
        List<List<String>> attr = new List<List<String>>();

        for (int i = 0; i < lines.length(); i++) {

            const String line = lines[i];

            if (line.includes('=')) {
                List<string> current = line.split("=");

                if (current.length == 2) {
                    labels.Add(current[0]);

                    if (current[1] == "false") {
                        // pass false
                    } else if (current[1] == true) {
                        // pass true
                    } else if (current[1].includes(',')) {
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
        attr.Add(values);

        return attr;
    }
    
};
