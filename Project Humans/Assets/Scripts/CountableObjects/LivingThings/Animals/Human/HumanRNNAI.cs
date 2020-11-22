using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanRNNAI
{
    public ActionChoice actionChoice;
    List<string> layerLabelList = new List<string>();
    List<int> layerSizeList = new List<int>();
    List<List<string>> layerConnectionList = new List<List<string>>();
    Dictionary<string, float> parameterDict = new Dictionary<string, float>();

    float[,] zh_r = new float[64,1024];
    float[,] zh_g = new float[64,1024];
    float[,] zh_b = new float[64,1024];
    float[,] zh_body = new float[64, 21];
    float[,] zh_drive = new float[64, 5];
    float[,] zh_bias = new float[64, 1];

    float[,]  zaction_bias = new float [12, 1];
    float[,] zaction_h = new float[12, 64];
    float[,] zaa_bias = new float[4,1];
    float[,] zaa_h = new float[4,64];

    float[] zh = new float[64];
    float[] h = new float[64];
    float[] zaa = new float[4];
    float[] aa = new float[4];
    float[] zaction = new float[12];
    float[] action = new float[12];

    public HumanRNNAI () {
        initNetwork();
    }

    public void initNetwork(){
        // init the values of all the weight matrices to small random numbers between .001 and -.001
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 1024; j++) {
                zh_r[i,j] = Random.Range(-0.001f, 0.001f);
                zh_g[i,j] = Random.Range(-0.001f, 0.001f);
                zh_b[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 21; j++) {
                zh_body[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 5; j++) {
                zh_drive[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 1; j++) {
                zh_bias[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 12; i++) {
            for (int j = 0; j < 1; j++) {
                zaction_bias[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 12; i++) {
            for (int j = 0; j < 64; j++) {
                zaction_h[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 1; j++) {
                zaa_bias[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 64; j++) {
                zaa_h[i,j] = Random.Range(-0.001f, 0.001f);
            }
        }
    }

    public ActionChoice chooseAction(float[ , ] visualInput, float[] humanBodyState, float[] humanDriveStats, ActionChoice actionchoice){
        
        // zh = zh_bias + (zh_r*visualInput[0,:]) + (zh_gzh*visualInput[1,:]) + (zh_b*visualInput[2,:]) + (zh_body*humanBodyState) + (zh_drive*humanDriveState);
        // h = sigmoid(zh);
        // zaa = zaa_bias + (zaa_h*h);
        // aa = sigmoid(zaa);
        // zaction = zaction_bias + (zaction_h*h);
        // action = sigmoid(zaction)

        // find the highest value in action, and set actionChoice to that action

        return actionChoice;
        
    }

}

    