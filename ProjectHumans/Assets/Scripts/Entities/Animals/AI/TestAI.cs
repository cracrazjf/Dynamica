using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestAI : AI {
    Transform animalTransform;
    List<GameObject> inSight;
    static Vector3 blankPos = new Vector3(0,0,0);
    Vector3 randomPos = blankPos;
    int updateCount;
    int actionCount;
    int numUpdates = 800;
    
    Dictionary<string, bool> bodyStateDict;
    Dictionary<string, float> driveStateDict;
    Dictionary<string, float> traitDict;


    public TestAI(Animal animal, Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype traits) : 
                    base(body, drives, motor, senses, traits) {
        thisAnimal = animal;
        bodyStateDict = body.GetStateDict();

        actionStates = motor.GetStates();
        actionStateLabelList = motor.GetStateLabels();

        actionArguments = motor.GetArgs();
        actionStateLabelList = motor.GetArgLabels();

        driveStateDict = drives.GetStateDict();
        traitDict = traits.GetTraitDict();

        updateCount = 0;
        actionCount = 0;
    }

    //public override int[,] ChooseAction(float[ , ] visualInput) {
    //    decidedActions = new int[actionStates.Length];
    //    decidedArgs = new int[actionArguments.Length];
    //    int[ , ] toReturn = new int[2 , actionStates.Length];
    //    toReturn[1,0] = 1;
    //    toReturn[1,1] = 1;
    //    if(thisAnimal.noCheats) {
    //        toReturn[0, 3] = 1;
    //    } else {
    //        int index = thisAnimal.cheatCommand;
    //        toReturn[0, index] = 1;
    //    }

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


        // "sitting down",// 0
        // "sitting up",  // 1
        // "laying down", // 2
        // "standing up", // 3
        // "rotating",    // 4
        // "taking steps",// 5
        // "picking up",  // 6
        // "setting down",// 7 
        // "consuming",   // 8
        // "waking up",   // 9
        // "sleeping",    // 10
        // "resting",     // 11
        // "looking"      // 12
    //    return toReturn;
    //}
}





