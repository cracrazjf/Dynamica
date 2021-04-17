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
}





