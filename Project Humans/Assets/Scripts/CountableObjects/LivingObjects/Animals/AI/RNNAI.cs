using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RNNAI : AI
{
    // List<string> layerLabelList = new List<string>();
    // List<int> layerSizeList = new List<int>();
    // List<List<string>> layerConnectionList = new List<List<string>>();
    // Dictionary<string, float> parameterDict;

    // float[,] zh_r;
    // float[,] zh_g;
    // float[,] zh_b;

    // float[] zh_bias;
    // float[,] zh_bodyInput;
    // float[,] zh_driveInput;
    // float[,] zh_actionStateInput;
    // float[,] zh_actionArgumentInput;
    
    // float [,] zh_lasth;

    // float[] zActionOutput_bias;
    // float[,] zActionOutput_h;

    // float[] zActionArgumentOutput_bias;
    // float[,] zActionArgumentOutput_h;

    // float[] zh;
    // float[] h;
    // float[] zActionOutput;
    // float[] actionOutput;
    // float[] zActionArgumentOutput;
    // float[] actionArgumentOutput;

    // List<NDArray> costs;
    // float[] driveStateValues;
    // float[] lastDriveState;

    // /*
    //     bodyStateIndexDict[""]

    // */

    public RNNAI (Dictionary<string, int> bodyStateIndexDict,
                       Dictionary<string, int> driveStateIndexDict,
                       Dictionary<string, int> actionStateIndexDict, 
                       Dictionary<string, int> actionArgumentIndexDict,
                       Dictionary<string, float> traitDict) : base(bodyStateIndexDict, driveStateIndexDict, actionStateIndexDict, actionArgumentIndexDict, traitDict) {

        // InitParameters();
        // InitLayers();
        // InitWeights();
    }

    // public void InitParameters(){
    //     parameterDict = new Dictionary<string, float>();
    //     parameterDict.Add("H_Size",64);
    //     parameterDict.Add("Weight_Init_Std", 0.01f);
    // }

    // public void InitLayers(){
    //     zh = new float[(int) parameterDict["H_Size"]];
    //     h = new float[(int) parameterDict["H_Size"]];
    //     zActionArgumentOutput = new float[numActionArguments];
    //     actionArgumentOutput = new float[numActionArguments];
    //     zActionOutput = new float[numActionStates];
    //     actionOutput = new float[numActionStates];
    // }

    // public float[] CreateRandomVector(int size){
    //     float[] randomVector = new float[size];
    //     for (int i = 0; i<size; i++){
    //         randomVector[i] = Random.Range(-parameterDict["Weight_Init_Std"], parameterDict["Weight_Init_Std"]);
    //     }
    //     return randomVector;

    // }

    // public float[,] CreateRandomMatrix(int numRows, int numColumns){
    //     float[,] randomMatrix = new float[numRows, numColumns];
    //     for (int i = 0; i<numRows; i++){
    //         for (int j = 0; j<numColumns; j++){
    //             randomMatrix[i,j] = Random.Range(-parameterDict["Weight_Init_Std"], parameterDict["Weight_Init_Std"]);
    //         }
    //     }
    //     return randomMatrix;
    // }

    // public void InitWeights(){

    //     int visualResolution = (int) traitDict["visual_resolution"]*(int) traitDict["visual_resolution"];
    //     zh_r = CreateRandomMatrix((int) parameterDict["H_Size"], visualResolution);
    //     zh_g = CreateRandomMatrix((int) parameterDict["H_Size"], visualResolution);
    //     zh_b = CreateRandomMatrix((int) parameterDict["H_Size"], visualResolution);
  
    //     zh_bias = CreateRandomVector((int) parameterDict["H_Size"]);
    //     zh_bodyInput = CreateRandomMatrix((int) parameterDict["H_Size"], numBodyStates);
    //     zh_driveInput = CreateRandomMatrix((int) parameterDict["H_Size"], numDriveStates);
    //     zh_actionStateInput = CreateRandomMatrix((int) parameterDict["H_Size"], numActionStates);
    //     zh_actionArgumentInput = CreateRandomMatrix((int) parameterDict["H_Size"], numActionArguments);

    //     zActionOutput_bias = CreateRandomVector(numActionStates);
    //     zActionOutput_h = CreateRandomMatrix(numActionStates, (int) parameterDict["H_Size"]);
    //     zActionArgumentOutput_bias = CreateRandomVector(numActionArguments);
    //     zActionArgumentOutput_h = CreateRandomMatrix(numActionArguments, (int) parameterDict["H_Size"]);

    //     zh_lasth = CreateRandomMatrix((int) parameterDict["H_Size"], (int) parameterDict["H_Size"]);

    // }

    // public void InitCosts(){

    //     costs = new List<NDArray>();
    //     costs.Add(new float[numActionOutputs]);
    //     costs.Add(new float[numActionArguments]);

    //     float[] lastDriveStateValues = new float[numDriveStates];
    //     lastDriveStateValues[0] = parameterDict["hunger_value"];
    //     lastDriveStateValues[1] = parameterDict["thirst_value"];
    //     lastDriveStateValues[2] = parameterDict["fatigue_value"];
    //     lastDriveStateValues[3] = parameterDict["sleepiness_value"];
    //     lastDriveStateValues[4] = parameterDict["health_value"];

    //     float[] driveStateValues = new float[numDriveStates];
    //     driveStateValues[0] = parameterDict["hunger_value"];
    //     driveStateValues[1] = parameterDict["thirst_value"];
    //     driveStateValues[2] = parameterDict["fatigue_value"];
    //     driveStateValues[3] = parameterDict["sleepiness_value"];
    //     driveStateValues[4] = parameterDict["health_value"];
    // }

    // public List<NDArray> Feedforward(){
    //     List<NDArray> outArrays = new List<NDArray>();

    //     //int dotProduct = digits1.Zip(digits2, (d1, d2) => d1 * d2).Sum();

    //     /* zh = zh_bias + 
    //             (zh_r*visualInput[0,:]) + 
    //             (zh_gzh*visualInput[1,:]) + 
    //             (zh_b*visualInput[2,:]) + 
    //             (zh_body*humanBodyState) + 
    //             (zh_drive*humanDriveState) + 
    //             (zh_actionStateInput*actionStateArray)) + 
    //             (zh_lasth * h)
        
    //     */
    //     // h = tanh(zh);
    //     // zaa = zaa_bias + (zaa_h*h);
    //     // zaction = zaction_bias + (zaction_h*h);
    //     // action = sigmoid(zaction)

    //     // find the highest value in action, and set actionChoice to that action

    //     //maxIndex = the index associatied with the highest value in zAction

    //     outArrays.Add(actionOutput);
    //     outArrays.Add(ArgumentOutputOutput);
    // }

    // public CalculateCost(){

    //     float[] driveStateChangeArray = driveStateValues - lastDriveStateValues;
    //     float currentValue = 0;

    //     // change this to a dot product
    //     for (int i=0; i<numDriveStates; i++){
    //         currentValue += driveStateChangeArray[i] * driveStateValues[i];
    //     }
    //     //currentValue = driveStateChangeArray.dot(driveStateValues);

    //     costs[0] = actionOutput * currentValue;
    //     costs[1] = actionArgumentOutput * currentValue;
    // }

    // public List<NDArray> AdjustWeights() {
    //     /*
            
    //     */
        
    // }

    // public override Animal.ActionChoiceStruct ChooseAction(float[ , ] visualInput, 
    //                                                        bool[] bodyStateArray, 
    //                                                        bool[] actionStateArray, 
    //                                                        float[] driveStateArray, 
    //                                                        Dictionary<string, float> traitDict){
        
    //     List<NDArray> costs = CalculateCost();
    //     AdjustWeights();

    //     List<NDArray> networkOutput;
    //     networkOutput = Feedforward();

    //     int maxIndex = 1; // replace this with maxIndex of networkOutput[0]
    //     this.actionChoiceStruct.actionChoiceArray[maxIndex] = true;
    //     this.actionChoiceStruct.actionArgumentArray = networkOutput[1];

    //     lastDriveStateValues = driveStateValues.copy();

    //     string outputString = actionStateLabelList[maxIndex];
    //     Debug.Log(outputString)

    //     return actionChoiceStruct;
        
    // }

}

    