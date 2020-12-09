using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// test change

// Missing a namespace;

public class Human : Animal
{
    public HumanSimpleAI2 humanSimpleAI2;
    public HumanRNNAI humanRNNAI;
    public string activeAI = "humanSimpleAI2";

    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome): 
            base("Human", index, position, motherGenome, fatherGenome) 
    {
        SetObjectType("human");

        SetBody(new HumanBody(this));
        GetBody().InitBodyStates();
        GetBody().SetBodyState(GetBody().GetBodyStateIndex("standing"), true);

        SetDriveSystem(new HumanDriveSystem(this));
        GetDriveSystem().InitDriveStates();
        GetDriveSystem().SetDriveState(GetDriveSystem().GetDriveStateIndex("health"), 1);

        SetSensorySystem(new HumanSensorySystem(this));

        SetMotorSystem(new HumanMotorSystem(this));
        InitActionStates();
        InitActionArguments();
        
        if (activeAI == "humanRNNAI"){
            humanRNNAI = new HumanRNNAI();
        }
        else{
            humanSimpleAI2 = new HumanSimpleAI2(this);
        }
    }
    
    public void TestUpdate() {
        GetMotorSystem().rotate(1);
    }

    public override void UpdateAnimal(){

        GetBody().UpdateBodyStates();
        GetDriveSystem().UpdateDrives();
        GetMotorSystem().UpdateActionStates();
        float[ , ] visualInputMatrix = GetSensorySystem().GetVisualInput();
        bool[] bodyStateArray = GetSensorySystem().GetBodyStateArray();
        bool[] actionStateArray = GetSensorySystem().GetActionStateArray();
        float[] driveStateArray = GetDriveSystem().GetDriveStatesArray();

        if (activeAI == "humanRNNAI"){
            actionChoiceStruct = humanRNNAI.chooseAction(visualInputMatrix, bodyStateArray, actionStateArray, driveStateArray, GetPhenotype().traitDict));
        }
        else {
            actionChoiceStruct = humanSimpleAI2.chooseAction();
        }

        GetMotorSystem().takeAction(actionChoiceStruct);
        
    }

}





    





    






    