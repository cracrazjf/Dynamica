using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDriveSystem : DriveSystem {

    public Human thisHuman;

    private List<string> humanDriveStateLabelList = new List<string>
    {
        "hunger", 
        "thirst", 
        "sleepiness",
        "fatigue",
        "health",
    };

    public HumanDriveSystem(Human human) : base(human) {
        this.thisHuman = human;
    }

    public override void UpdateDrives()
    {
        for (int i = 0; i < numStates; i++) {
            string driveStateLabel = driveStateLabelList[i];
            string changeLabel = driveStateLabel + "_change";
            
            // add some error checking to make sure the labels in config and stateLabelList match
            float changeValue = float.Parse(thisAnimal.phenotype.traitDict[changeLabel]);
            driveStateArray[i] += changeValue;
            if (driveStateArray[i] < 0){
                driveStateArray[i] = 0;
            }
            else if (driveStateArray[i] > 1){
                driveStateArray[i] = 1;
            }
        }

        if (driveStateArray[driveStateIndexDict["hunger"]] >= 1.0){
            // add check to make sure starvation_damage is a key in the dict
            driveStateArray[driveStateIndexDict["health"]] -= float.Parse(thisAnimal.phenotype.traitDict["starvation_damage"]);
        }
        if (driveStateArray[driveStateIndexDict["thirst"]] >= 0.0){
            // add check to make sure dehydration_damage is a key in the dict
            driveStateArray[driveStateIndexDict["health"]] -= float.Parse(thisAnimal.phenotype.traitDict["dehydration_damage"]);
        }
            
    }

}