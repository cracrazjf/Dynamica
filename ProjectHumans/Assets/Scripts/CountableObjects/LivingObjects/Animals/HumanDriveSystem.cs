using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDriveSystem : DriveSystem {

    public Human thisHuman;

    public HumanDriveSystem(Human human) : base(human) {
        this.thisHuman = human;
        this.stateLabelList = new List<string> {
            // Originally driveStates, will be read-in eventually

            "hunger", 
            "thirst", 
            "sleepiness",
            "fatigue",
            "health",
        };
        this.InitStates(this.stateLabelList);
        SetState("health", 1.0f);
    }

    public override void UpdateDrives()
    {
        foreach (string label in stateLabelList) {
            string changeLabel = label + "_change";
            float changeValue = thisAnimal.phenotype.GetTraitDict()[changeLabel];

            // Ensure all drive states are in bounds
            if (this.stateDict[label] < 0) { this.SetState(label, 0.0f); }
            else if (this.stateDict[label] > 1) { this.SetState(label, 1.0f); }
        }

        // Hunger and thirst updates
        float currentHealth = stateDict["health"];
        if (stateDict["hunger"] >= 1.0 ) {
            float toUpdate = currentHealth - thisAnimal.phenotype.GetTraitDict()["starvation_damage"];
            this.SetState("health", toUpdate);
        }
        if (stateDict["thirst"] >= 0.0) {
            float toUpdate = currentHealth - thisAnimal.phenotype.GetTraitDict()["dehydration_damage"];
            this.SetState("health", toUpdate);
        }    
    }
}