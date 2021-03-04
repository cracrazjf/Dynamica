

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using NumSharp;


// public class RNNAI : AI
// {
//     List<string> layerLabelList = new List<string>();
//     List<int> layerSizeList = new List<int>();
//     List<List<string>> layerConnectionList = new List<List<string>>();
//     Dictionary<string, float> parameterDict;

//     float[,] zh_r;
//     float[,] zh_g;
//     float[,] zh_b;

//     float[] zh_bias;
//     //float[,] zh_bias;
//     float[,] zh_bodyInput;
//     float[,] zh_driveInput;
//     float[,] zh_actionStateInput;
//     float[,] zh_actionArgumentInput;
    
//     float [,] zh_lasth;

//     float[] zActionOutput_bias;
//     float[,] zActionOutput_h;

//     float[] zActionArgumentOutput_bias;
//     float[,] zActionArgumentOutput_h;

//     float[] zh;
//     float[] h;
//     float[] zActionOutput;//before tanh
//     float[] actionOutput;//after tanh
//     float[] zActionArgumentOutput;
//     float[] actionArgumentOutput;

//     List<NDArray> costs;
//     float[] driveStateValues;
//     float[] lastDriveStateValues;
    //added these and modified lastdrivestatevalues.
    //float[] zaa = new float[4];
    //float[] zaction = new float[12];
    //float[,] zh_body = new float[64, 21];
    //float[,] zh_drive = new float[64, 5];
    //float[,] zaction_bias = new float[12, 1];
    //float[,] zaction_h = new float[12, 64];
    //float[,] zaa_bias = new float[4, 1];
    //float[,] zaa_h = new float[4, 64];
    
    // bodyStateIndexDict[""]

     
    // public RNNAI (Dictionary<string, int> bodyStateIndexDict,
    //                    Dictionary<string, int> driveStateIndexDict,
    //                    Dictionary<string, int> actionStateIndexDict, 
    //                    Dictionary<string, int> actionArgumentIndexDict,
    //                    Dictionary<string, float> traitDict) : base(bodyStateIndexDict, driveStateIndexDict, actionStateIndexDict, actionArgumentIndexDict, traitDict) {

    //     InitParameters();
    //     InitLayers();
    //     InitWeights();
    //     InitCosts(); //added this in 
    // }

    // public void InitParameters(){
    //     parameterDict = new Dictionary<string, float>();
    //     parameterDict.Add("H_Size",64);
    //     parameterDict.Add("Weight_Init_Std", 0.01f);
    //     //should these be initialized here and to what value?
    //     parameterDict.Add("hunger_value", 1);//-1 to 1, hunger should be multiplied by -1 when calculating costs
    //     parameterDict.Add("thirst_value", 1);
    //     parameterDict.Add("fatigue_value", 1);
    //     parameterDict.Add("sleepiness_value", 1);
    //     parameterDict.Add("health_value", 1);
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
  
    //     zh_bias = CreateRandomVector((int) parameterDict["H_Size"]);//would it change the logic if we made the vectors matrices?
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

    //     //costs.Add(new float[numActionOutputs]);
    //     costs.Add(new float[actionOutput.Length]);
    //     costs.Add(new float[numActionArguments]);

    //     lastDriveStateValues = new float[numDriveStates]; //this was redeclared should it be a global variable?
    //     lastDriveStateValues[0] = parameterDict["hunger_value"];
    //     lastDriveStateValues[1] = parameterDict["thirst_value"];
    //     lastDriveStateValues[2] = parameterDict["fatigue_value"];
    //     lastDriveStateValues[3] = parameterDict["sleepiness_value"];
    //     lastDriveStateValues[4] = parameterDict["health_value"];

    //     driveStateValues = new float[numDriveStates];
    //     driveStateValues[0] = parameterDict["hunger_value"];
    //     driveStateValues[1] = parameterDict["thirst_value"];
    //     driveStateValues[2] = parameterDict["fatigue_value"];
    //     driveStateValues[3] = parameterDict["sleepiness_value"];
    //     driveStateValues[4] = parameterDict["health_value"];
    // }

    // public List<NDArray> Feedforward(float[,] visualInput, bool[] bodyStateArray, bool[] actionStateArray, 
    //     float[] driveStateArray, Dictionary<string, float> traitDict)
    // {
    //     List<NDArray> outArrays = new List<NDArray>();

    //     NDArray N_visualInput = visualInput;
    //     NDArray N_humanBodyStateTemp = bodyStateArray;
    //     NDArray N_humanBodyState = N_humanBodyStateTemp.astype(np.float64);
    //     NDArray N_humanDriveStats = driveStateArray;
    //     //I will clean this up after it is functional
    //     //NDArray N_zh = zh;
    //     NDArray N_h = h;
    //     NDArray N_zaa = zActionArgumentOutput;
    //     NDArray N_zaction = zActionOutput;

    //     NDArray N_zh_r = zh_r;
    //     NDArray N_zh_g = zh_g;
    //     NDArray N_zh_b = zh_b;
    //     NDArray N_zh_body = zh_bodyInput;
    //     NDArray N_zh_drive = zh_driveInput;
    //     NDArray N_zh_bias = zh_bias;

    //     NDArray N_zaction_bias = zActionOutput_bias;
    //     NDArray N_zaction_h = zActionOutput_h;
    //     NDArray N_zaa_bias = zActionArgumentOutput_bias;
    //     NDArray N_zaa_h = zActionArgumentOutput_h;

    //     NDArray N_zh_actionStateInput = zh_actionStateInput;
    //     NDArray N_actionStateArrayTemp = actionStateArray;
    //     NDArray N_actionStateArray = N_actionStateArrayTemp.astype(np.float64);
    //     NDArray N_zh_lasth = zh_lasth;

    //     //NDArray N_actionOutput = actionOutput;
    //     //NDArray N_ArgumentOutput = actionArgumentOutput; //is this the right argumentOutput?

    //     //int dotProduct = digits1.Zip(digits2, (d1, d2) => d1 * d2).Sum(); what is this?

    //     zh = zh_bias + 
    //             (zh_r*visualInput[0,:]) + 
    //             (zh_gzh*visualInput[1,:]) + 
    //             (zh_b*visualInput[2,:]) + 
    //             (zh_body*humanBodyState) + 
    //             (zh_drive*humanDriveState) + 
    //             (zh_actionStateInput*actionStateArray)) + 
    //             (zh_lasth * h)
        
        
    //     //shape error

    //     var temp1 = (np.dot(N_zh_r, np.transpose(N_visualInput["0,:"])));//should be dot products
    //     var temp2 = (np.dot(N_zh_g, np.transpose(N_visualInput["1,:"])));
    //     var temp3 = (np.dot(N_zh_b, np.transpose(N_visualInput["2,:"])));
    //     var temp4 = (np.dot(N_zh_body, np.transpose(N_humanBodyState)));
    //     var temp5 = (np.dot(N_zh_drive, np.transpose(N_humanDriveStats)));
    //     var temp6 = (np.dot(N_zh_actionStateInput, np.transpose(N_actionStateArray)));
    //     var temp7 = (np.dot(N_zh_lasth, np.transpose(N_h)));
    //     //var sum = np.add(temp1, temp2);

        
    //     //var sum2 = temp4 + temp5 + temp6 + temp7 + sum;
    //     var N_zh = np.transpose(temp1 + temp2 + temp3 + temp4 + temp5 + temp6 + temp7 + np.transpose(N_zh_bias));

    //     // CRITICAL: MAKE COPY OF N_h before we change it below
    //     N_zh_lasth = N_h.copy();

    //     // h = tanh(zh);
    //     //N_h.ToMuliDimArray<float>();
    //     for (int i = 0; i < N_zh.size; i++)
    //     {
    //         N_h[i] = (float) System.Math.Tanh((double)N_zh[i]); //unity tanh activation function shape error at first, need to keep track of the shape thorugh the computations
    //     }
    //     // zaa = zaa_bias + (zaa_h*h);
    //     // zaction = zaction_bias + (zaction_h*h);
    //     N_zaa = N_zaa_bias + np.transpose((np.dot(N_zaa_h, np.transpose(N_h)))); //what shape are these supposed to be before during and after?
    //     N_zaction = N_zaction_bias + (np.dot(N_zaction_h, np.transpose(N_h)));//what shape are these supposed to be before during and after?

    //     //add this 
    //     NDArray N_action = new float[12];
    //     NDArray N_aa = new float[8];

    //     for (int i = 0; i < N_zaction.size; i++)
    //     {
    //         N_action[i] = (float) System.Math.Tanh((double)N_zaction[i]); //unity tanh activation function shape error at first, need to keep track of the shape thorugh the computations
    //     }

    //     for (int i = 0; i < N_zaa.size; i++)
    //     {
    //         N_action[i] = (float) System.Math.Tanh((double)N_zaa[i]); //unity tanh activation function shape error at first, need to keep track of the shape thorugh the computations
    //     }


    //     outArrays.Add(N_action);
    //     outArrays.Add(N_aa);
    //     return outArrays;
    //     //end feed forward function here

    //     // find the highest value in action, and set actionChoice to that action
        
    //     float max = (float) N_zaction[0]; //shape error//what shape are these supposed to be before during and after?
    //     for (int count = 0; count < N_zaction.size; count++)
    //     {
    //         if (max < (float) N_zaction[count])
    //         {
    //             max = (float) N_zaction[count];
    //         }
    //     }
        
        //maxIndex = the index associatied with the highest value in zAction


    // }

    // public List<NDArray> CalculateCost(){
    //     NDArray N_driveStateValues = driveStateValues;
    //     NDArray N_lastDriveStateValues = lastDriveStateValues;
    //     float[] driveStateChangeArray = (float[])(N_driveStateValues - N_lastDriveStateValues);
    //     float currentValue = 0;
    //     NDArray N_driveStateChangeArray = driveStateChangeArray;
    //     NDArray N_actionOutput = actionOutput;
    //     NDArray N_actionArgumentOutput = actionArgumentOutput;
    //     // change this to a dot product
    //     //for (int i=0; i<numDriveStates; i++){
    //     //    currentValue += driveStateChangeArray[i] * driveStateValues[i];
    //     //}
    //     currentValue = N_driveStateChangeArray.dot(driveStateValues);

    //     costs[0] = N_actionOutput * currentValue;
    //     costs[1] = N_actionArgumentOutput * currentValue;
    //     return costs;
    // }

    // public void AdjustWeights() {
    
    //    parameters: costs[action_error][action_argument_error] or the costs [vector the size of each output], last_input, learning rate, networkoutput
        
    //         """ multiply the error by the derivative of y_predict with respect to the sigmoid function in other words, to
    //     make y what we want, how much would we have to change x, taking into account it is a sigmoid function (since
    //     it's not linear, changes to x may not straightforwardly affect y? in other words, we wanted y1 to be 00, but it
    //     was 0.48, so what weights coming from layer h would have made y1=00 given the current values of h, taking into
    //     consideration that y is being calculated as the sigmoid of the dot product of its weighted inputs"""
    //     y_delta = y_error * self.sigmoid_prime(y_predict)
        
        

    //     cost0_delta = costs[0] * self.tanh_prime(networkOutput[0])
    //     cost1_delta = costs[1] * self.tanh_prime(networkOutput[1])



    //     self.action_bias += costs[0]_delta * self.learning_rate
    //     self.za_h += (np.dot(cost[0]delta.reshape(len(y_delta), 1), h.reshape(1, len(h))) * self.learning_rate)

    //     actionargument_bias += costs[1]_delta * self.learning_rate
    //     self.zaa_h += (np.dot(cost1delta.reshape(len(y_delta), 1), h.reshape(1, len(h))) * self.learning_rate)

    //     """ propogate the delta, how much each y should be changed, back to the hidden layer, multiplying the value of 
    //     y_delta for each y by the weights coming into it, figuring out how much each hidden unit is "to blame" for y 
    //     being incorrect. Then figure out how much to change each of the weights coming into each hidden unit from x in 
    //     the same way, but multiplying the cost by the derivative of the tangent function. In other words, we wanted h1 
    //     to be a, it was a+00.05, so what weight change would make the output of h1 closer to a for this x, given that h1 
    //     is using the tangent function """

        
    //     h_cost = np.dot(cost0delta, h) + np.dot(cost1delta, h)
    //     h_delta = h_cost * self.tanh_prime(h)
        
    //     self.h_bias += h_delta * self.learning_rate
        
    //     N_zh_r += (np.dot(delta, N_visualInput["0,:"] ) * self.learning_rate
    //     N_zh_g +=
    //     N_zh_b +=
    //     N_zh_body +=
    //     N_zh_drive +=
    //     N_zh_bias +=

    //     """change the weights by the amounts determined above, which would set the weights to the "exact best" value
    //     for this item, multiplied by the learning rate, which says "only move them this proportion in the exact
    //     right direction", considering that the exact best weights for this item might not be the exact best weights
    //     for all items, and we dont want our weights ping-ponging all over the place."""
        

        



    //     self.h_bias += h_delta * self.learning_rate
    //     self.h_x += (np.dot(h_delta.reshape(len(h_delta), 1), x.reshape(1, len(x))) * self.learning_rate)
       
    //     //List<NDArray> test = new List<NDArray>();
    //     //return test;
    // }

//    public override AI.ActionChoiceStruct ChooseAction(float[ , ] visualInput, Dictionary<string, float> traitDict){
//         //driveStateValues = driveStateArray; //needed to initialize driveStateValues, dont know if this is correct
        
//         List<NDArray> costs = CalculateCost();
//         //AdjustWeights();

//         List<NDArray> networkOutput;
//         networkOutput = Feedforward(visualInput, bodyStateArray,actionStateArray,driveStateArray,traitDict);
//         networkOutput[actionOutput, actionArgumentOutput], find the highest in actionOutput
//         float max = networkOutput[0][0]; // replace this with maxIndex of networkOutput[0]
//         int maxIndex = networkOutput[0].argmax();
//         * * for (int i = 0; i < 12; i++)
//          {
//              if (networkOutput[0][i] > networkOutput[0][maxIndex])
//              {
//                  maxIndex = i;
//              }
//          } */
//         int maxIndex = np.argmax(networkOutput[0]); 

//         float[] networkOutputArr1 = (float[])networkOutput[1];
//         this.actionChoiceStruct.actionChoiceArray[maxIndex] = true;
//         this.actionChoiceStruct.actionArgumentArray = networkOutputArr1;

//         NDArray N_driveStateValues = driveStateValues;
//         lastDriveStateValues = (float[])N_driveStateValues.copy();

//         string outputString = actionStateLabelList[maxIndex];
//         Debug.Log(outputString);

//         return actionChoiceStruct;
        
//     }

// }

