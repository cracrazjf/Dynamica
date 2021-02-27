using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Human : Animal
{
    public HumanSimpleAI humanSimpleAI;
    public string activeAILabel = "HumanSimpleAI";

    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(int index, Genome motherGenome, Genome fatherGenome): 
            base("Human", index, motherGenome, fatherGenome) {
        SetObjectType("Human");
        
        // All of these are getting passed empty lists right now, need to read in state arrays
        SetBody(new HumanBody(this));
        GetBody().UpdateBodyStates();
        visualInputCamera = this.gameObject.GetComponentInChildren<Camera>();
        
        SetDriveSystem(new HumanDriveSystem(this));
        SetMotorSystem(new HumanMotorSystem(this));
        SetSensorySystem(new HumanSensorySystem(this));


        if (activeAILabel == "blankAI") {
            activeAI = new AI(GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype());
        } else {
            humanSimpleAI = new HumanSimpleAI(this, GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype());
            activeAI = humanSimpleAI;
        }
    }

    public override void UpdateAnimal() {
        this.GetDriveSystem().UpdateDrives();
        float[ , ] visualInputMatrix = GetSensorySystem().GetVisualInput();
        string toSend = activeAI.ChooseAction(visualInputMatrix, GetPhenotype().GetTraitDict());
        
        this.GetMotorSystem().TakeAction(toSend);
        GetBody().ResolveAltitude();

        IncreaseAge(1);
    }

    public bool GetBodyState(string state) {
        return this.GetBody().GetStateDict()[state];
    }
    public void ToggleBodyPart(string part, bool toggle) {
        this.GetBody().GetSkeletonDict()[part].gameObject.SetActive(toggle);
    }
}





    





    






    