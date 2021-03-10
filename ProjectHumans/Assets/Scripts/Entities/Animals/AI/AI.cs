using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI 
{
    protected Animal thisAnimal;
    protected int[] decidedActions;
    protected int[] decidedArgs;
    protected float [ , ] visualInput;

    protected bool[] bodyStates;
    protected List<string> bodyStateLabelList;
    protected Dictionary<string, int> bodyStateIndexDict;

    protected float[] driveStates;
    protected List<string> driveStateLabelList;
    protected Dictionary<string, int> driveStateIndexDict;

    protected bool[] actionStates;
    protected List<string> actionStateLabelList;
    protected Dictionary<string, int> actionStateIndexDict;
    
    protected float[] actionArguments;
    protected List<string> actionArgumentLabelList;
    protected Dictionary<string, int> actionArgumentIndexDict;
    
    protected float[] traits;
    protected List<string> traitLabelList;
    protected Dictionary<string, int> traitIndexDict;

    

    bool outputDefinitionError = false;


    public AI(Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype phenotype) {

        bodyStates = body.GetStates();
        bodyStateLabelList = body.GetStateLabels();
        bodyStateIndexDict = body.GetStateIndices();

        driveStates = drives.GetStates();
        driveStateLabelList = drives.GetStateLabels();
        driveStateIndexDict = drives.GetStateIndices();

        actionStates = motor.GetStates();
        actionStateLabelList = motor.GetStateLabels();
        actionStateIndexDict = motor.GetStateIndices();

        actionArguments = motor.GetArgs();
        actionArgumentLabelList = motor.GetArgLabels();
        actionArgumentIndexDict = motor.GetArgIndices();

        traits = phenotype.GetTraits();
        traitLabelList = phenotype.GetTraitLabels();
        traitIndexDict = phenotype.GetTraitIndices();

        visualInput = senses.GetVisualInput();
    }

    // Since ChooseAction is in here, it doesn't need all these values passed. AI already has the bodyState, actionState, etc. - JC
    public virtual int[,] ChooseAction (float[ , ] visualInput){
        
        if (outputDefinitionError == false){
            Debug.Log("No ChooseAction function defined for this AI");
            outputDefinitionError = true;
        }
        
        return null;
    }

    public string GetAction() {
        return "In progress!";
    }
}