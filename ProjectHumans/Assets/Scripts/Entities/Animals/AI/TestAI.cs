using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

public class TestAI : AI {
    protected  Matrix<float> decidedActions;
    Transform animalTransform;
    List<GameObject> inSight;
    static Vector3 blankPos = new Vector3(0,0,0);
    Vector3 randomPos = blankPos;
    
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
    }

    // public override Vector<float> ChooseAction(Matrix<float> visualInput) {
    //     var toReturn = Vector<float>.Build.Dense(15);
    //     toReturn[1] = 1;

    //     return toReturn;
    // }

    // Untested but should be good to go
    public override Matrix<float> ChooseAction() {
        decidedActions = Matrix<float>.Build.Dense(actionStates.Count, 1);
        
        //check for input cheats
        if (thisAnimal.noCheats) {
            decidedActions[3, 0] = 1f;
        } else {
            int index = thisAnimal.cheatCommand;
            decidedActions[index, 0] = thisAnimal.cheatArgs;
        }
        return decidedActions;
    }
}





