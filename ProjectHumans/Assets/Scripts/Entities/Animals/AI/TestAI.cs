using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

public class TestAI : AI {
    Transform animalTransform;
    List<GameObject> inSight;
    static Vector3 blankPos = new Vector3(0,0,0);
    Vector3 randomPos = blankPos;
    int updateCount;
    int actionCount;
    int numUpdates = 800;
    
    Dictionary<string, float> bodyStateDict;
    Dictionary<string, float> driveStateDict;
    Dictionary<string, float> traitDict;


    public TestAI(Animal animal, Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype traits) : 
                    base(body, drives, motor, senses, traits) {
        thisAnimal = animal;
        bodyStateDict = body.GetStateDict();

        actionStates = motor.GetStates();
        actionStateLabelList = motor.GetStateLabels();

        driveStateDict = drives.GetStateDict();
        traitDict = traits.GetTraitDict();

        updateCount = 0;
        actionCount = 0;
    }

    // public override Vector<float> ChooseAction(Matrix<float> visualInput) {
    //     var toReturn = Vector<float>.Build.Dense(15);
    //     toReturn[1] = 1;

    //     return toReturn;
    // }

    // Untested but should be good to go
    public override Vector<float> ChooseAction() {
        decidedActions = Vector<float>.Build.Dense(actionStates.Count);
        
        //check for input cheats
        if (thisAnimal.noCheats) {
            decidedActions[3] = 1;
        } else {
            int index = thisAnimal.cheatCommand;
            decidedActions[index] = thisAnimal.cheatArgs;
        }
        return decidedActions;
    }

        //Debug.DrawRay(animalTransform.position, animalTransform.forward * 10, Color.yellow);

        // if (updateCount < numUpdates) {
        //     decidedActions[actionCount] = 1;
        //     updateCount += 1;
        // } else {
        //     updateCount = 0;
        //     if (actionCount < actionStates.Length -1) {
        //         actionCount +=1;
        //     } else {
        //         actionCount= 0;
        //     }
        // }


        // "crouching",   // 0, negative is down 
        // "sitting",     // 1, negative is down
        // "laying down", // 2 -1 or 1 (or 0 if not switched)
        // "standing up", // 3 -1 or 1 (or 0 if not switched)
        // "rotating",    // 4, now a proportion
        // "taking steps",// 5, now a proportion
        // "hand action", // 6, release/maintain/grab
        // "consuming",   // 7, set to consumable if ongoing
        // "sleeping",    // 8, awake/maintain/fall asleep
        // "resting",     // 9 -1 or 1 (or 0 if not switched)
        // "looking",     // 10 -1 or 1 (or 0 if not switched)
        // "RP x",        // 11  -1 to 1, proportion of max range from start pos
        // "RP y",        // 12
        // "RP z"         // 13
        // "active hand", // 7  -1 or 1 (or 0 if not switched)
        
    //    return toReturn;
    //}
}





