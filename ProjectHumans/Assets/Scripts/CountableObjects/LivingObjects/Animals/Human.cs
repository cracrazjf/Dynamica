using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// test change

// Missing a namespace;

public class Human : Animal
{
    public HumanSimpleAI humanSimpleAI;
    public AI activeAI;
    public string activeAILabel = "HumanSimpleAI";

    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome): 
            base("Human", index, position, motherGenome, fatherGenome) 
    {
        SetObjectType("Human");

        SetBody(new HumanBody(this));
        GetBody().InitBodyStates(GetBody().GetBodyStateLabelList());
        GetBody().UpdateBodyStates();
        visualInputCamera = this.gameObject.GetComponentInChildren<Camera>();
        
        SetDriveSystem(new HumanDriveSystem(this));
        GetDriveSystem().InitDriveStates(GetDriveSystem().GetDriveStateLabelList());
        GetDriveSystem().SetDriveState("health", 1.0f);

        SetMotorSystem(new HumanMotorSystem(this));
        GetMotorSystem().InitActionStates();
        GetMotorSystem().InitActionRuleDicts();
        GetMotorSystem().InitActionArguments();

        SetSensorySystem(new HumanSensorySystem(this));

        if (activeAILabel == "blankAI") {
            activeAI = new AI(GetBody().GetBodyStateDict(), 
                            GetDriveSystem().GetDriveStateDict(), 
                            GetMotorSystem().GetActionStateDict(),
                            GetMotorSystem().GetActionArgumentDict(),
                            GetPhenotype().traitDict);
        } else {
            humanSimpleAI = new HumanSimpleAI(this, GetBody().GetBodyStateDict(), 
                                                GetDriveSystem().GetDriveStateDict(), 
                                                GetMotorSystem().GetActionStateDict(),
                                                GetMotorSystem().GetActionArgumentDict(),
                                                GetPhenotype().traitDict);
            activeAI = humanSimpleAI;
        }
    }
    
    public void TestUpdate() {
        
    }

    public override void UpdateAnimal(){
        float[ , ] visualInputMatrix = GetSensorySystem().GetVisualInput();
        activeAI.actionChoiceStruct = activeAI.ChooseAction(visualInputMatrix, GetPhenotype().traitDict);
        GetBody().UpdateBodyStates();
        GetMotorSystem().TakeAction(activeAI.actionChoiceStruct);
        IncreaseAge(1);
    }
}





    





    






    