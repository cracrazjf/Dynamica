using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using System;
using UnityEngine;

public class Layer
{
    string name;
    List<int> shape;
    int numUnits;
    string layerType = "None";
    Dictionary<string, Connection> inputConnectionDict;
    Dictionary<string, Connection> outputConnectionDict = new Dictionary<string, Connection>();
    Matrix<float> output;
    Matrix<float> cost;
    Dictionary<string, Matrix<float>> thisInputDict;
    Dictionary<string, Matrix<float>> thisRecurrentMemoryDict;
    Dictionary<string, Layer> thisLayerDict;

    
    bool calculatedThisUpdate;
    bool calculatedThisCost;


    public Layer(Dictionary<string, Matrix<float>> passedInputDict, Dictionary<string, Matrix<float>> passedRecurrentMemoryDict, Dictionary<string, Layer> passedLayerDict, Dictionary<string, List<string>> passedLayerInfoDcit)
    {
        this.thisInputDict = passedInputDict;
        this.thisRecurrentMemoryDict = passedRecurrentMemoryDict;
        this.thisLayerDict = passedLayerDict;

        initOutput();
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public List<int> Shape
    {
        get { return shape; }
        set { shape = value; }
    }

    public int NumUnits
    {
        get { return numUnits; }
        set { numUnits = value; }
    }

    public string LayerType
    {
        get { return layerType; }
        set { layerType = value; }
    }

    public Dictionary<string, Connection> InputConnectionDict
    {
        get { return inputConnectionDict; }
        set { inputConnectionDict = value; }
    }

    public Dictionary<string, Connection> OutputConnectionDict
    {
        get { return outputConnectionDict; }
        set { outputConnectionDict = value; }
    }

    public Matrix<float> Output
    {
        get { return output; }
        set { output = value; }
    }

    public Matrix<float> Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public bool CalculatedThisUpdate
    {
        get { return calculatedThisUpdate; }
        set { calculatedThisUpdate = value; }
    }

    void initOutput()
    {
        if(layerType == "output" || layerType == "hidden" || layerType == "recurrent")
        {
            output = Matrix<float>.Build.Dense(shape[0], shape[1]);
        }
        else if (layerType == "bias")
        {
            output = Matrix<float>.Build.Dense(1, 1, 1.0f);
        }
        else
        {
            Debug.Log(name);
            output = thisInputDict[name];
        }
    }

    public void FeedForward()
    {
        if (!calculatedThisUpdate)
        {
            if (layerType == "input")
            {
                output = thisInputDict[name];
            }
            else if (layerType == "recurrent")
            {
                output = thisRecurrentMemoryDict[name];
                //Animal.AddEventTime("Calculated " + this.name + " Feedforward");
            }
            else if (layerType == "bias"){

            }
            else
            // its either a hidden or an output
            {
                foreach (KeyValuePair<string, Connection> inputInfo in inputConnectionDict)
                {
                    //One layer could have multiple connections
                    Connection currentConnection = inputConnectionDict[inputInfo.Key];
                    thisLayerDict[inputInfo.Key].FeedForward();
                    Matrix<float> x = thisLayerDict[inputInfo.Key].output;
                    output += currentConnection.GetNetInputArray(x);
                }
            }
            calculatedThisUpdate = true;
            Animal.AddEventTime("Calculated " + this.name + " Feedforward");

        }
    }
    bool firstRun = true;
    

    // MAKE SURE THIS WORKING CORRECTLY
    public void CalculateCost(){
        Debug.Log("Not First Run");
        if (!calculatedThisCost)
        {
            Debug.Log(calculatedThisUpdate);
            if (layerType == "output" && !name.Contains("zOutput"))
            {
                string inputLayerName = "input" + name.Substring(6);
                Debug.Log(inputLayerName);
                Matrix<float> predictionError = thisInputDict[inputLayerName] - output;
                cost = predictionError;
            }
            else
            {
                Debug.Log("Calculated Cost");
                cost = Matrix<float>.Build.Dense(shape[0], shape[1]);
                foreach (KeyValuePair<string, Connection> outputInfo in outputConnectionDict)
                {
                    Connection currentConnection = OutputConnectionDict[outputInfo.Key];
                    thisLayerDict[outputInfo.Key].CalculateCost();
                    Matrix<float> x = thisLayerDict[outputInfo.Key].output;
                    Matrix<float> y = thisLayerDict[outputInfo.Key].cost;
                    cost += currentConnection.CalculateDeltaWeights(x, y);
                }
            }
            calculatedThisCost = true;
        }
        
        

    /*
        if layerType == "Output"

            layer's cost is defined as an attribute of the layer class
            layer's outputs are initialized in this class constructor
                output, hidden, and recurrent layers to matrices of zeros
                biases to 1
                inputs to their actual values

            cost = error + reinforcement
            prediction_error = y_actual - y_predict

            y_actual is the input, y_predict is the current output values
            for example:
            prediction_error = inputVisionRedLayer - outputVisionRedLayer

        else:
            cost = matrix of zeros
            for each output connection from this layer:
                cost += this layer's output connection's costs * the weights from that connection

        */
    }
  
/*

    example_list = [[], [], [], [], ...]

    for each input:
        x = input
        backpropogation()

    for i in range(num_examples-1):
        x = example_list[i]
        y = example_list[i+1]

        o = feedforward(x)
        o_cost = calc_cost(y, o)
        backpropogation(x, o, h, o_cost, learning_rate)

    def feedforward(self, x):
        h = self.tanh(np.dot(self.h_x, x) + self.h_bias)
        o = self.sigmoid(np.dot(self.o_h, h) + self.o_bias)
        return h, o

    def calc_cost(y, o):
        return y - o

    def backpropogation(self, x, o, h, o_cost, learning_rate):

*/

}