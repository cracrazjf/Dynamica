using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RNNAI : AI
{
    List<string> layerLabelList = new List<string>();
    List<int> layerSizeList = new List<int>();
    List<List<string>> layerConnectionList = new List<List<string>>();
    Dictionary<string, float> parameterDict;

    float[,] zh_r;
    float[,] zh_g;
    float[,] zh_b;

    float[] zh_bias;
    float[,] zh_bodyInput;
    float[,] zh_driveInput;
    float[,] zh_actionStateInpute;
    float[,] zh_actionArgumentInput;
    
    float[] zActionOutput_bias;
    float[,] zActionOutput_h;

    float[] zActionArgumentOutput_bias;
    float[,] zActionArgumentOutput_h;

    float[] zh;
    float[] h;
    float[] zActionOutput;
    float[] actionOutput;
    float[] zActionArgumentOutput;
    float[] ActionArgumentOutput;

    public RNNAI (Dictionary<string, int> bodyStateIndexDict,
                       Dictionary<string, int> driveStateIndexDict,
                       Dictionary<string, int> actionStateIndexDict, 
                       Dictionary<string, int> actionArgumentIndexDict,
                       Dictionary<string, float> traitDict) : base(bodyStateIndexDict, driveStateIndexDict, actionStateIndexDict, actionArgumentIndexDict, traitDict) {

        InitParameters();
        InitLayers();
        InitWeights();
    }

    public void InitParameters(){
        parameterDict = new Dictionary<string, float>();
        parameterDict.Add("H_Size") = 64;
        parameterDict.Add("Weight_Init_Std") = 0.01;
    }

    public void InitLayers(){
        zh = new float[parameterDict["H1_Size"]];
        h = new float[parameterDict["H1_Size"]];
        zActionArgumentOutput = new float[numActionArguments];
        actionArgumentOutput = new float[numActionArguments];
        zActionOutput = new float[numActionStates];
        actionOutput = new float[numActionStates];
    }

    public CreateRandomVector(int size){
        randomVector = new float[size];
        for (int i = 0; i<size; i++){
            randomVector[i] = Random.Range(-parameterDict["weight_init_std"], parameterDict["weight_init_std"]);
        }
        return randomVector;

    }

    public CreateRandomMatrix(int numRows, int numColumns){
        randomMatrix = new float[numRows, numColumns];
        for (int i = 0; i<numRows; i++){
            for (int j = 0; j<numColumns; j++){
                randomVector[i,j] = Random.Range(-parameterDict["weight_init_std"], parameterDict["weight_init_std"]);
            }
        }
        return randomMatrix;
    }

    public void InitWeights(){

        int visualResolution = traitDict["visual_resolution"]*traitDict["visual_resolution"]);
        zh_r = CreateRandomMatrix(parameterDict["H1_Size"], visualResolution);
        zh_g = CreateRandomMatrix(parameterDict["H1_Size"], visualResolution);
        zh_b = CreateRandomMatrix(parameterDict["H1_Size"], visualResolution);
  
        zh_bias = CreateRandomVector(parameterDict["H1_Size"]);
        zh_bodyInput = CreateRandomMatrix(parameterDict["H1_Size"], numBodyStates);
        zh_driveInput = CreateRandomMatrix(parameterDict["H1_Size"], numDriveStates);
        zh_actionStateInput = CreateRandomMatrix(parameterDict["H1_Size"], numActionStates);
        zh_actionArgumentInput = CreateRandomMatrix(parameterDict["H1_Size"], numActionArguments);

        zActionOutput_bias = CreateRandomVector(numActionStates);
        zActionOutput_h = CreateRandomMatrix(numActionStates, parameterDict["H1_Size"]);
        zActionArgumentOutput_bias = CreateRandomVector(numActionArguments);
        zActionArgumentOutput_h = CreateRandomMatrix(numActionArguments, parameterDict["H1_Size"]);

    }

    public override Animal.ActionChoiceStruct ChooseAction(float[ , ] visualInput, 
                                                           bool[] bodyStateArray, 
                                                           bool[] actionStateArray, 
                                                           float[] driveStateArray, 
                                                           Dictionary<string, float> traitDict){

        int dotProduct = digits1.Zip(digits2, (d1, d2) => d1 * d2).Sum();

        // zh = zh_bias + (zh_r*visualInput[0,:]) + (zh_gzh*visualInput[1,:]) + (zh_b*visualInput[2,:]) + (zh_body*humanBodyState) + (zh_drive*humanDriveState);
        // h = sigmoid(zh);
        // zaa = zaa_bias + (zaa_h*h);
        // aa = sigmoid(zaa);
        // zaction = zaction_bias + (zaction_h*h);
        // action = sigmoid(zaction)

        // find the highest value in action, and set actionChoice to that action
        

        return actionChoiceStruct;
        
    }

}

    