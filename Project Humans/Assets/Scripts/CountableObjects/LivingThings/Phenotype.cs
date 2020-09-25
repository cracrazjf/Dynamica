using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phenotype : MonoBehaviour
{
    // Vekicity and accerlerationRate
    public float initialVelocity = 0.0f;
    public float finalVelocity = 50.0f;
    public float currentVelocity = 0.0f;
    public float accelerationRate = 0.01f;

    //rotate angle and rotate speed;
    public float rotation;
    public float rotationleft = 360;
    public float rotationspeed = 10;

    // the 5 Change values 
    public List<string> traitLabelList = new List<string>(new string[] { "hunger_change", 
                                                                         "thirst_change", 
                                                                         "sleepiness_change", 
                                                                         "fatigue_change", 
                                                                         "health_change",
                                                                         "hunger_threshold",
                                                                         "thirst_threshold",
                                                                         "sleepiness_threshold",
                                                                         "fatigue_threshold",
                                                                         "health_threshold"});
    public int numTraits; 
    public Dictionary<string, int> traitIndexDict = new Dictionary<string, int>();
    public List<float> traitValueList = new List<float>();
    public List<float> traitDisplayList = new List<float>();


    private void Start()
    {
        human = gameObject.GetComponent<Human>();


        numTraits = traitLabelList.Count;
        for (int i = 0; i < numTraits; i++)
        {
            traitIndexDict.Add(traitLabelList[i], i);

            traitDisplayList.Add(0);
            


            traitValueList.Add(0.1f);  // hunger change
            traitValueList.Add(0.001f);  // thirst change
            traitValueList.Add(0.001f);  // sleepiness change
            traitValueList.Add(0.001f);  // fatigue change
            traitValueList.Add(0.001f);  // health change
            traitValueList.Add(0.5f);  // hunger threshold
            traitValueList.Add(0.5f);  // thirst threshold
            traitValueList.Add(0.5f);  // sleepiness threshold
            traitValueList.Add(0.5f);  // fatigue threshold
            traitValueList.Add(0.5f);  // health threshold
        }
    }
    private void Update()
    {
        
    }
}

