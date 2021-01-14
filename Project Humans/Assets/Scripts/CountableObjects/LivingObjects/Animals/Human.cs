using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// test change

// Missing a namespace;

public class Human : Animal
{
    public HumanSimpleAI2 humanSimpleAI2;
    public AI activeAI;
    public string activeAILabel = "HumanSimpleAI";

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

        SetMotorSystem(new HumanMotorSystem(this));
        GetMotorSystem().InitActionStates(GetMotorSystem().GetActionStateLabelList());
        GetMotorSystem().InitActionRuleDicts();
        GetMotorSystem().InitActionArguments();

        SetSensorySystem(new HumanSensorySystem(this));

        if (activeAILabel == "RNNAI"){
            activeAI = new RNNAI(GetBody().GetBodyStateIndexDict(), 
                                        GetDriveSystem().GetDriveStateIndexDict(), 
                                        GetMotorSystem().GetActionStateIndexDict(),
                                        GetMotorSystem().GetActionArgumentIndexDict(),
                                        GetPhenotype().traitDict);
        }
        else if (activeAILabel == "blankAI"){
            activeAI = new AI(GetBody().GetBodyStateIndexDict(), 
                            GetDriveSystem().GetDriveStateIndexDict(), 
                            GetMotorSystem().GetActionStateIndexDict(),
                            GetMotorSystem().GetActionArgumentIndexDict(),
                            GetPhenotype().traitDict);
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
        if (activeAILabel == "HumanSimpleAI"){
            actionChoiceStruct = humanSimpleAI2.ChooseAction(GetSensorySystem().GetVisualInput(), 
                                                            GetBody().GetBodyStateArray(),
                                                            GetMotorSystem().GetActionStateArray(),
                                                            GetDriveSystem().GetDriveStateArray(),
                                                            GetPhenotype().traitDict);

            
        }
        else {
            actionChoiceStruct = activeAI.ChooseAction(visualInputMatrix, bodyStateArray, actionStateArray, driveStateArray, GetPhenotype().traitDict);
        }

        GetMotorSystem().TakeAction(actionChoiceStruct);
        GetMotorSystem().EndAction(actionChoiceStruct);
        IncreaseAge(1);
    }
}





    





    






    