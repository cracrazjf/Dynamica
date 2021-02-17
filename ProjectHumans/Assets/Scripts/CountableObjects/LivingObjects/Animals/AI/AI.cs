using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI 
{
    public ActionChoiceStruct actionChoiceStruct;

    protected int numBodyStates;
    protected Dictionary<string, bool> bodyStateDict;
    protected List<string> bodyStateLabelList;
    
    protected int numDriveStates;
    protected Dictionary<string, float> driveStateDict;
    protected List<string> driveStateLabelList;

    protected int numActionStates;
    protected List<string> actionStateLabelList;
    protected Dictionary<string, bool> actionStateDict;
    
    protected int numActionArguments;
    protected Dictionary<string, float> actionArgumentDict;
    protected List<string> actionArgumentLabelList;
    

    protected int numTraits;
    protected List<string> traitLabelList;
    protected Dictionary<string, float> traitDict;

    bool outputDefinitionError = false;

    public AI(Dictionary<string, bool> bodyStateDict, Dictionary<string, float> driveStateDict, Dictionary<string, bool> actionStateDict, 
              Dictionary<string, float> actionArgumentDict, Dictionary<string, float> traitDict) {
        this.bodyStateDict = bodyStateDict;
        this.driveStateDict = driveStateDict;
        this.actionStateDict = actionStateDict;
        this.actionArgumentDict = actionArgumentDict;
        this.traitDict = traitDict;
    }

    // Moved this here from Animal... Action choices are an AI thing, not inherent to life (a sponge is not considered to choose actions) - JC
    public struct ActionChoiceStruct {
        public Dictionary<string, bool> actionChoiceDict;
        public Dictionary<string, float> actionArgumentDict;
    };

    public ActionChoiceStruct GetActionChoiceStruct() {
        return actionChoiceStruct;
        }
        

    public virtual void InitActionChoiceStruct() {
        actionChoiceStruct = new ActionChoiceStruct();
        actionChoiceStruct.actionChoiceDict = new Dictionary<string, bool>();
        actionChoiceStruct.actionArgumentDict = new Dictionary<string, float>();
    }

    // Since ChooseAction is in here, it doesn't need all these values passed. AI already has the bodyState, actionState, etc. - JC
    public virtual ActionChoiceStruct ChooseAction (float[ , ] visualInput, Dictionary<string, float> traitDict){
        
        if (outputDefinitionError == false){
            Debug.Log("No ChooseAction function defined for this AI");
            outputDefinitionError = true;
        }
        
        return actionChoiceStruct;
    }

}