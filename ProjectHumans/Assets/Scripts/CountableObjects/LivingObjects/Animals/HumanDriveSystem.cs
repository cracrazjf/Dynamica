using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDriveSystem : DriveSystem {

    public Human thisHuman;

    public List<string> humanDriveStateLabelList;

    public HumanDriveSystem(Human human) : base(human) {
        this.thisHuman = human;
        driveStateLabelList = new List<string>
        {
            "hunger", 
            "thirst", 
            "sleepiness",
            "fatigue",
            "health",
        };
    }

    public override void UpdateDrives()
    {
        foreach (string label in driveStateLabelList) {
            string changeLabel = label + "_change";
            
            float changeValue = thisAnimal.phenotype.traitDict[changeLabel];
            driveStateDict[label] += changeValue;

            if (driveStateDict[label] < 0) { driveStateDict[label] = 0; }
            else if (driveStateDict[label] > 1) { driveStateDict[label] = 1; }
        }

        if (driveStateDict["hunger"] >= 1.0 ) {
            driveStateDict["health"] -= thisAnimal.phenotype.traitDict["starvation_damage"];
        }
        if (driveStateDict["thirst"] >= 0.0) {
            driveStateDict["health"] -= thisAnimal.phenotype.traitDict["dehydration_damage"];
        }    
    }
}