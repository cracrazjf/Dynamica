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
        GetBody().InitBodyStates(GetBody().GetBodyStateLabelList());
        GetBody().SetBodyState(GetBody().GetBodyStateIndex("standing"), true);
        visualInputCamera = this.gameObject.GetComponentInChildren<Camera>();
        
        SetDriveSystem(new HumanDriveSystem(this));
        GetDriveSystem().InitDriveStates(GetDriveSystem().GetDriveStateLabelList());
        GetDriveSystem().SetDriveState(GetDriveSystem().GetDriveStateIndex("health"), 1);

        SetSensorySystem(new HumanSensorySystem(this));

        SetMotorSystem(new HumanMotorSystem(this));
        GetMotorSystem().InitActionStates(GetMotorSystem().GetActionStateLabelList());
        GetMotorSystem().InitActionRuleDicts();
        GetMotorSystem().InitActionArguments();

        this.InitActionChoiceStruct();
        Debug.Log(this.getActionChoiceArray() == null);
        
        
        if (activeAI == "humanRNNAI"){
            humanRNNAI = new HumanRNNAI();
        }
        else{
            humanSimpleAI2 = new HumanSimpleAI2(this,
                                                GetBody().GetBodyStateIndexDict(), 
                                                GetDriveSystem().GetDriveStateIndexDict(), 
                                                GetMotorSystem().GetActionStateIndexDict(),
                                                GetMotorSystem().GetActionArgumentIndexDict(),
                                                GetPhenotype().traitDict);
        }
    }
    
    public void TestUpdate() {
        
    }

    public override void UpdateAnimal(){

        GetBody().UpdateBodyStates();
        GetDriveSystem().UpdateDrives();
        GetMotorSystem().UpdateActionStates();
        float[ , ] visualInputMatrix = GetSensorySystem().GetVisualInput();
        bool[] bodyStateArray = GetBody().GetBodyStateArray();
        bool[] actionStateArray = GetMotorSystem().GetActionStateArray();
        float[] driveStateArray = GetDriveSystem().GetDriveStateArray();
        // these two ifs needs debug
        if (activeAI == "humanRNNAI"){
            actionChoiceStruct = humanRNNAI.ChooseAction(visualInputMatrix, bodyStateArray, actionStateArray, driveStateArray, GetPhenotype().traitDict);
        }
        else {
            actionChoiceStruct = humanSimpleAI2.ChooseAction(GetSensorySystem().GetVisualInput(), 
                                                            GetBody().GetBodyStateArray(),
                                                            GetMotorSystem().GetActionStateArray(),
                                                            GetDriveSystem().GetDriveStateArray(),
                                                            this.actionChoiceStruct);
        }

        GetMotorSystem().TakeAction(actionChoiceStruct);
        
    }
}





    





    






    