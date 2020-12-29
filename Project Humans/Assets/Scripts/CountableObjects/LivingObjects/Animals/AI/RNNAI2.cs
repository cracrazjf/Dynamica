using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;
using System;

public class HumanRNNAI
{
    public ActionChoice actionChoice;
    List<string> layerLabelList = new List<string>();
    List<int> layerSizeList = new List<int>();
    List<List<string>> layerConnectionList = new List<List<string>>();
    Dictionary<string, float> parameterDict = new Dictionary<string, float>();

    // r-vision [1024] 32x32
    // g-vision [1024]
    // b-vision [1024]
    // bodyState [21]
    // driveState [5]

    float[] zh = new float[64]; // net input to the hidden layer
    float[] h = new float[64];  // output of the hidden layer 
    float[] zaa = new float[4];
    float[] zaction = new float[12];

    // these are fully connected layers going to y from x, if its called y_x
    // these are the weights from inputs to hidden
    float[,] zh_r = new float[64,1024];
    float[,] zh_g = new float[64,1024];
    float[,] zh_b = new float[64,1024];
    float[,] zh_body = new float[64, 21];
    float[,] zh_drive = new float[64, 5];
    float[,] zh_bias = new float[64, 1];

    // these are the weights from hidden to outputs
    float[,] zaction_bias = new float [12, 1];
    float[,] zaction_h = new float[12, 64];
    float[,] zaa_bias = new float[4,1];
    float[,] zaa_h = new float[4,64];


    public HumanRNNAI () {
        initNetwork();
    }
    
    public void initNetwork(){
        // init the values of all the weight matrices to small random numbers between .001 and -.001
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 1024; j++) {
                zh_r[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
                zh_g[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
                zh_b[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 21; j++) {
                zh_body[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 5; j++) {
                zh_drive[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 1; j++) {
                zh_bias[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f); //ambiguity error
            }
        }
        for (int i = 0; i < 12; i++) {
            for (int j = 0; j < 1; j++) {
                zaction_bias[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 12; i++) {
            for (int j = 0; j < 64; j++) {
                zaction_h[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 1; j++) {
                zaa_bias[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 64; j++) {
                zaa_h[i,j] = UnityEngine.Random.Range(-0.001f, 0.001f);
            }
        }
    }

    public ActionChoice chooseAction(float[ , ] visualInput, float[] humanBodyState, float[] humanDriveStats, ActionChoice actionchoice){
        //                  [64,1024]*[1024,1]
        //                      [64,1]
        // zh = zh_bias + (zh_r*visualInput[0,:]) + (zh_gzh*visualInput[1,:]) + (zh_b*visualInput[2,:]) + (zh_body*humanBodyState) + (zh_drive*humanDriveState);
        // h = tanh(zh);
        // zaa = zaa_bias + (zaa_h*h);
        // zaction = zaction_bias + (zaction_h*h);
        // find the highest value in zaction and set actionChoice.actionValueDict["rotate"] = 1;
        // the label that corresponds to index x is accessible via actionChoice.actionLabelList[x]
        // it needs to go through all action Argument outputs and copy them into the correct spots in action argument dict
        // for i in range(num_action_arguments)
        //      current_argument = actionChoice.actionArgumentList[i]
        //      actionChoice.actionArgumentDict[currentArgument] = zaa[i]
        //int numCol = 1024;

        NDArray N_visualInput = visualInput;
        NDArray N_humanBodyState = humanBodyState;
        NDArray N_humanDriveStats = humanDriveStats;

        NDArray N_zh = zh;
        NDArray N_h = h;
        NDArray N_zaa = zaa;
        NDArray N_zaction = zaction;

        NDArray N_zh_r = zh_r;
        NDArray N_zh_g = zh_g;
        NDArray N_zh_b = zh_b;
        NDArray N_zh_body = zh_body;
        NDArray N_zh_drive = zh_drive;
        NDArray N_zh_bias = zh_bias;

        NDArray N_zaction_bias = zaction_bias;
        NDArray N_zaction_h = zaction_h;
        NDArray N_zaa_bias = zaa_bias;
        NDArray N_zaa_h = zaa_h;



        //       zh = zh_bias + (zh_r.dot(N_visualInput["0,:"])) + (zh_gzh.dot(N_visualInput["1,:"])) + (zh_b.dot(N_visualInput["2,:"])) + (zh_body.dot(humanBodyState)) + (zh_drive.dot(humanDriveState));
        N_zh = N_zh_bias + (np.multiply(N_zh_r, N_visualInput["0,:"])) + (np.multiply(N_zh_g, N_visualInput["1,:"])) + (np.multiply(N_zh_b, N_visualInput["2,:"])) + (np.multiply(N_zh_body, N_humanBodyState)) + (np.multiply(N_zh_drive, N_humanDriveStats));

        for(int i=0; i < N_zh.size; i++)
        {
            N_h[i] = Math.Tanh(N_zh[i]); //unity tanh activation function
        }


        N_zaa = N_zaa_bias + (np.multiply(N_zaa_h, N_h)); //are these supposed to be dot products or multiplication?
        N_zaction = N_zaction_bias + (np.multiply(N_zaction_h, N_h));


        // find the highest value in zaction and set actionChoice.actionValueDict["rotate"] = 1;
        //find max in zaction
        float max = N_zaction[0];
        for (int count = 0; count < N_zaction.size; count++) {
            if (max < N_zaction[count]) {
                max = N_zaction[count];
            }
        }
        
        //set actionChoice.actionValueDict["rotate"] = 1
        //actionChoice.actionValueDict["rotate"] = 1;
        this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["rotating"]] = true;


        // the label that corresponds to index x is accessible via actionChoice.actionLabelList[x]
        // it needs to go through all action Argument outputs and copy them into the correct spots in action argument dict

        int num_action_arguments = 1;
        string current_argument;

        for (int i = 0; i < num_action_arguments; i++) {
            current_argument = actionChoice.argumentList[i];
            actionChoice.argumentDict[current_argument] = N_zaa[i];
        }
        return actionChoice;
        
    }

}

    