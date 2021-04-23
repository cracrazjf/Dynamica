using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;

public class NeuralAI : AI
{
    // this is a dictionary of layer names points to lists of layer info, [layerType, layerShape]
    Dictionary<string, List<string>> networkLayerInfoDict; 
    
    Dictionary<string, Dictionary<string, string>> networkConnectionInfoDict = new Dictionary<string, Dictionary<string, string>>()
    {
        // this is a dict of the Connections and their sender and input types
        { "inputVisionRedLayer", new Dictionary<string, string>(){ } },
        { "inputVisionGreenLayer", new Dictionary<string, string>(){ } },
        { "inputVisionBlueLayer", new Dictionary<string, string>(){ } },
        { "inputDriveLayer", new Dictionary<string, string>(){ } },
        { "inputBodyLayer", new Dictionary<string, string>(){ } },
        { "inputActionLayer", new Dictionary<string, string>(){ } },
        { "inputActionArgumentLayer", new Dictionary<string, string>(){ } },
        { "recurrentHiddenLayer", new Dictionary<string, string>(){ { "zHiddenLayer", "dense"},
                                                               { "biasLayer", "dense"} } },
        { "zHiddenLayer", new Dictionary<string, string>()
            {
                { "inputVisionRedLayer", "dense"},
                { "inputVisionGreenLayer", "dense"},
                { "inputVisionBlueLayer", "dense"},
                { "inputDriveLayer", "dense"},
                { "inputBodyLayer", "dense"},
                { "inputActionLayer", "dense"},
                { "inputActionArgumentLayer", "dense"},
                { "biasLayer", "dense"}
            } },
        { "hiddenLayer", new Dictionary<string, string>(){ { "zHiddenLayer", "tanh"}}},
        { "zOutputVisionRedLayer", new Dictionary<string, string>(){ { "hiddenLayer", "dense" },
                                                                    { "biasLayer", "dense"}} },
        { "zOutputVisionGreenLayer", new Dictionary<string, string>(){ { "hiddenLayer", "dense" },
                                                                      { "biasLayer", "dense"}} },
        { "zOutputVisionBlueLayer", new Dictionary<string, string>(){ { "hiddenLayer", "dense" },
                                                                     { "biasLayer", "dense"}} },
        { "zOutputDriveLayer", new Dictionary<string, string>(){ { "hiddenLayer", "dense" },
                                                                { "biasLayer", "dense"}} },
        { "zOutputBodyLayer", new Dictionary<string, string>(){ { "hiddenLayer", "dense" },
                                                               { "biasLayer", "dense"}} },
        { "zOutputActionLayer", new Dictionary<string, string>(){ { "hiddenLayer", "dense" },
                                                                 { "biasLayer", "dense"}} },
        { "zOutputActionArgumentLayer", new Dictionary<string, string>(){ { "hiddenLayer", "dense" },
                                                                            { "biasLayer", "dense"}} },

        { "outputVisionRedLayer", new Dictionary<string, string>(){ { "zOutputVisionRedLayer", "tanh" }}},
        { "outputVisionGreenLayer", new Dictionary<string, string>(){ { "zOutputVisionGreenLayer", "tanh" }}},
        { "outputVisionBlueLayer", new Dictionary<string, string>(){ { "zOutputVisionBlueLayer", "tanh" }}},
        { "outputDriveLayer", new Dictionary<string, string>(){{ "zOutputDriveLayer", "tanh"}}},
        { "outputBodyLayer", new Dictionary<string, string>(){{ "zOutputBodyLayer", "tanh"}}},
        { "outputActionLayer", new Dictionary<string, string>(){ { "zOutputActionLayer", "tanh" }}},
        { "outputActionArgumentLayer", new Dictionary<string, string>(){{ "zOutputActionArgumentLayer", "tanh" }}}
    };


    Animal thisHuman;
    int[,,] hiddenSize = new int[128, 1, 1];

    // dictionary of the layer objects
    Dictionary<string, Layer> networkLayerDict = new Dictionary<string, Layer>();

    // dictionary of the layer objects
    Dictionary<string, Layer> outputLayerDict = new Dictionary<string, Layer>();

    // dictionary of the layer objects
    Dictionary<string, Matrix<float>> recurrentMemoryDict = new Dictionary<string, Matrix<float>>();
    Dictionary<string, string> recurrentInfoDict = new Dictionary<string, string>();

    // dictionary of the layer objects
    Dictionary<string, Matrix<float>> inputDict = new Dictionary<string, Matrix<float>>();
    
    

    public NeuralAI(Animal animal, Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype phenotype) :
                    base(body, drives, motor, senses, phenotype)
    {
        initNetworkLayerInfoDict();
        initLayers();
        initConnections();
        GetOutputConnectionDict();
        InitInputs();
        Feedforward();
        UpdateWeights();
        
        //foreach (Layer x in networkLayerDict.Values)
        //{
        //    Debug.Log(x.Name);
        //    foreach(KeyValuePair<string, Connection> y in x.OutputConnectionDict)
        //    {
        //        Debug.Log(y);
        //    }
        //}
    }

    void initNetworkLayerInfoDict()
    {
        networkLayerInfoDict = new Dictionary<string, List<string>>()
        {
            // this is a dict of the layers and their types and sizes
            
            {"biasLayer", new List<string>() {"bias", "1,1" } },
            {"inputVisionRedLayer", new List<string>() {"input", visualInput.Row(0).Count.ToString() + ",1" } },
            {"inputVisionGreenLayer", new List<string>() {"input", visualInput.Row(1).Count.ToString() + ",1" } },
            {"inputVisionBlueLayer", new List<string>() {"input", visualInput.Row(2).Count.ToString() + ",1" } },
            {"inputDriveLayer", new List<string>() {"input", driveStates.Count.ToString() + ",1" } },
            {"inputBodyLayer", new List<string>() {"input", bodyStates.Count.ToString() + ",1" } },
            {"inputActionLayer", new List<string>() {"input", actionStates.Count.ToString() + ",1" } },
            {"inputActionArgumentLayer", new List<string>() {"input", actionArguments.Count.ToString() + ",1" } },
            {"recurrentHiddenLayer", new List<string>() { "recurrent", "128,1" } },
            
            {"zHiddenLayer", new List<string>() { "hidden", "128,1" } },
            {"hiddenLayer", new List<string>() { "hidden", "128,1" } },

            {"zOutputVisionRedLayer", new List<string>() {"output", visualInput.Row(0).Count.ToString() + ",1" } },
            {"zOutputVisionGreenLayer", new List<string>() { "output", visualInput.Row(1).Count.ToString() + ",1" } },
            {"zOutputVisionBlueLayer", new List<string>() { "output", visualInput.Row(2).Count.ToString() + ",1" } },
            {"zOutputDriveLayer", new List<string>() { "output", driveStates.Count.ToString() + ",1" } },
            {"zOutputBodyLayer", new List<string>() { "output", bodyStates.Count.ToString() + ",1" } },
            {"zOutputActionLayer", new List<string>() { "output", actionStates.Count.ToString() + ",1" } },
            {"zOutputActionArgumentLayer", new List<string>() { "output", actionArguments.Count.ToString() + ",1" } },
            
            {"outputVisionRedLayer", new List<string>() {"output", visualInput.ColumnCount.ToString() + ",1" } },
            {"outputVisionGreenLayer", new List<string>() { "output", visualInput.ColumnCount.ToString() + ",1" } },
            {"outputVisionBlueLayer", new List<string>() { "output", visualInput.ColumnCount.ToString() + ",1" } },
            {"outputDriveLayer", new List<string>() { "output", driveStates.Count.ToString() + ",1" } },
            {"outputBodyLayer", new List<string>() { "output", bodyStates.Count.ToString() + ",1" } },
            {"outputActionLayer", new List<string>() { "output", actionStates.Count.ToString() + ",1" } },
            {"outputActionArgumentLayer", new List<string>() { "output", actionArguments.Count.ToString() + ",1" } },
        };
    }

    void initLayers()
    {
        foreach (KeyValuePair<string, List<string>> layerInfo in networkLayerInfoDict)
        {
            List<string> shapeLabel = layerInfo.Value[1].Split(',').ToList();
            List<int> shape = new List<int>();
            foreach (string x in shapeLabel)
            {
                if (int.TryParse(x, out int y))
                {
                    shape.Add(y);
                }
                else
                {
                    Debug.Log("String could not be parsed.");
                }
            }
            networkLayerDict.Add(layerInfo.Key, new Layer(inputDict, recurrentMemoryDict, networkLayerDict,networkLayerInfoDict)
            {
                Name = layerInfo.Key,
                Shape = shape,
                NumUnits = shape.Aggregate((x, y) => x * y),
                LayerType = layerInfo.Value[0],
                InputConnectionDict = new Dictionary<string, Connection>(),
                //Cost = Matrix<float>.Build.Dense(shape[0], shape[1])
            });

            if (layerInfo.Value[0] == "recurrent")
            {
                // create a NDArray of all zero's of the appropriate shape
                Matrix<float> memory = Matrix<float>.Build.Dense(shape[0], shape[1]);
                // add that NDArray to the recurrentMemoryDict, with layer name as the key
                recurrentMemoryDict.Add(layerInfo.Key, memory);
                // add an entry to recurrent info dict, that points from the recurrent layer to its input
                recurrentInfoDict.Add(layerInfo.Key, "zHiddenLayer");
                // recurrentHidden, 

            }
            if (layerInfo.Value[0] == "input")
            {
                Matrix<float> currentInput = Matrix<float>.Build.Dense(shape[0], shape[1]);
                inputDict.Add(layerInfo.Key, currentInput);
                
            }
            if (layerInfo.Value[0] == "output")
            {
                outputLayerDict.Add(layerInfo.Key, networkLayerDict[layerInfo.Key]);
            }
            if (layerInfo.Value[0] == "bias")
            {
                networkLayerDict["biasLayer"].Output = Matrix<float>.Build.Dense(shape[0], shape[1], 1.0f);
            }
        }

        networkLayerDict["zHiddenLayer"].Output = Matrix<float>.Build.Dense(128, 1);
        networkLayerDict["hiddenLayer"].Output = Matrix<float>.Build.Dense(128, 1);
        networkLayerDict["zOutputVisionRedLayer"].Output = Matrix<float>.Build.Dense(256, 1);
        networkLayerDict["zOutputVisionGreenLayer"].Output = Matrix<float>.Build.Dense(256, 1);
        networkLayerDict["zOutputVisionBlueLayer"].Output = Matrix<float>.Build.Dense(256, 1);
        networkLayerDict["zOutputDriveLayer"].Output = Matrix<float>.Build.Dense(5, 1);
        networkLayerDict["zOutputBodyLayer"].Output = Matrix<float>.Build.Dense(4, 1);
        networkLayerDict["zOutputActionLayer"].Output = Matrix<float>.Build.Dense(13, 1);
        networkLayerDict["zOutputActionArgumentLayer"].Output = Matrix<float>.Build.Dense(6, 1);
        networkLayerDict["outputVisionRedLayer"].Output = Matrix<float>.Build.Dense(256, 1);
        networkLayerDict["outputVisionGreenLayer"].Output = Matrix<float>.Build.Dense(256, 1);
        networkLayerDict["outputVisionBlueLayer"].Output = Matrix<float>.Build.Dense(256, 1);
        networkLayerDict["outputDriveLayer"].Output = Matrix<float>.Build.Dense(5, 1);
        networkLayerDict["outputBodyLayer"].Output = Matrix<float>.Build.Dense(4, 1);
        networkLayerDict["outputActionLayer"].Output = Matrix<float>.Build.Dense(13, 1);
        networkLayerDict["outputActionArgumentLayer"].Output = Matrix<float>.Build.Dense(6, 1);
    }

    void initConnections()
    {

        float weight_init_range = 0.0001f;
        foreach (KeyValuePair<string, Dictionary<string, string>> structureInfo in networkConnectionInfoDict)
        {
            Dictionary<string, Connection> thisLayerConnectionDict = new Dictionary<string, Connection>();
            // create the layer's empty connection dictionary

            if (structureInfo.Value.Count > 0)
            {

                // create a connection object
                // add an entry to the layer's connection dictionary, with the sender name as the key, and the connection object as its value
                foreach (KeyValuePair<string, string> connectionInfo in structureInfo.Value)
                {
                    if (connectionInfo.Value == "dense")
                    {
                        Connection dense = new Dense { ConnectionType = connectionInfo.Value, Sender = connectionInfo.Key, Recipient = structureInfo.Key };
                        // this should be in the dense constructor
                        dense.ConnectionWeight = Matrix<float>.Build.Random(networkLayerDict[dense.Recipient].NumUnits, networkLayerDict[dense.Sender].NumUnits, new Normal(0, weight_init_range));
                        networkLayerDict[structureInfo.Key].InputConnectionDict.Add(connectionInfo.Key, dense);
                    }
                    else if (connectionInfo.Value == "tanh")
                    {
                        Connection tanh = new TanhActivation { ConnectionType = connectionInfo.Value, Sender = connectionInfo.Key, Recipient = structureInfo.Key };
                        // this shouldn't be here, right?
                        tanh.ConnectionWeight = Matrix<float>.Build.Random(networkLayerDict[tanh.Recipient].NumUnits, networkLayerDict[tanh.Sender].NumUnits, new Normal(0, weight_init_range));
                        networkLayerDict[structureInfo.Key].InputConnectionDict.Add(connectionInfo.Key, tanh);

                    }
                }
            }
        }
    }

    void GetOutputConnectionDict()
    {
        foreach (Layer layer in networkLayerDict.Values)
        {
            foreach (Connection connection in layer.InputConnectionDict.Values)
            {
                networkLayerDict[connection.Sender].OutputConnectionDict.Add(layer.Name, connection);
            }
        }
    }

    int counter = 0;
    public void Feedforward(){
        // testTimes();

        foreach (KeyValuePair<string, Layer> layerInfo in networkLayerDict)
        {
            layerInfo.Value.CalculatedThisUpdate = false;
        }

        foreach (KeyValuePair<string, Layer> outputLayer in outputLayerDict)
        {
            outputLayer.Value.FeedForward();
        }

        //outputLayerDict["outputVisionRedLayer"].FeedForward();
        foreach (string memoryKey in recurrentMemoryDict.Keys.ToList())
        {
            networkLayerDict[recurrentInfoDict[memoryKey]].Output.CopyTo(recurrentMemoryDict[memoryKey]);
        }
    }

    // public void HardCodedFeedforward(){
    //     networkLayerDict["inputVisionRedLayer"].Output = visualInput.Row(0).ToColumnMatrix();
    //     networkLayerDict["inputVisionGreenLayer"].Output = visualInput.Row(1).ToColumnMatrix();
    //     networkLayerDict["inputVisionBlueLayer"].Output = visualInput.Row(2).ToColumnMatrix();
    //     networkLayerDict["inputDriveLayer"].Output = driveStates.ToColumnMatrix();
    //     networkLayerDict["inputBodyLayer"].Output = bodyStates.ToColumnMatrix();
    //     networkLayerDict["inputActionLayer"].Output = actionStates.ToColumnMatrix();
    //     networkLayerDict["inputActionArgumentLayer"].Output = actionArguments.ToColumnMatrix();
    //     Debug.Log(networkLayerDict["inputVisionRedLayer"].Output.RowCount);
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["inputVisionRedLayer"].ConnectionWeight.Multiply(networkLayerDict["inputVisionRedLayer"].Output);
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["inputVisionGreenLayer"].ConnectionWeight.Multiply(networkLayerDict["inputVisionGreenLayer"].Output);
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["inputVisionBlueLayer"].ConnectionWeight.Multiply(networkLayerDict["inputVisionBlueLayer"].Output);
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["inputDriveLayer"].ConnectionWeight.Multiply(networkLayerDict["inputDriveLayer"].Output);
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["inputBodyLayer"].ConnectionWeight.Multiply(networkLayerDict["inputBodyLayer"].Output);
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["inputActionLayer"].ConnectionWeight.Multiply(networkLayerDict["inputActionLayer"].Output);
    //     networkLayerDict["zHiddenLayer"].Output += networkLayerDict["zHiddenLayer"].InputConnectionDict["inputActionArgumentLayer"].ConnectionWeight.Multiply(networkLayerDict["inputActionArgumentLayer"].Output);

        
    //     networkLayerDict["zOutputVisionRedLayer"].Output += networkLayerDict["zOutputVisionRedLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;

    //     networkLayerDict["zOutputVisionRedLayer"].Output += networkLayerDict["zOutputVisionRedLayer"].InputConnectionDict["hiddenLayer"].ConnectionWeight.Multiply(networkLayerDict["hiddenLayer"].Output);
    //     networkLayerDict["outputVisionRedLayer"].Output += Matrix<float>.Tanh(networkLayerDict["zOutputVisionRedLayer"].Output);
        
    //     networkLayerDict["zOutputVisionGreenLayer"].Output += networkLayerDict["zOutputVisionGreenLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;
        
    //     networkLayerDict["zOutputVisionGreenLayer"].Output += networkLayerDict["zOutputVisionGreenLayer"].InputConnectionDict["hiddenLayer"].ConnectionWeight.Multiply(networkLayerDict["hiddenLayer"].Output);
        
    //     networkLayerDict["outputVisionGreenLayer"].Output += Matrix<float>.Tanh(networkLayerDict["zOutputVisionGreenLayer"].Output);

    //     networkLayerDict["zOutputVisionBlueLayer"].Output += networkLayerDict["zOutputVisionBlueLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;
        
    //     networkLayerDict["zOutputVisionBlueLayer"].Output += networkLayerDict["zOutputVisionBlueLayer"].InputConnectionDict["hiddenLayer"].ConnectionWeight.Multiply(networkLayerDict["hiddenLayer"].Output);
       
    //     networkLayerDict["outputVisionBlueLayer"].Output += Matrix<float>.Tanh(networkLayerDict["outputVisionBlueLayer"].Output);

    //     networkLayerDict["zOutputDriveLayer"].Output += networkLayerDict["zOutputDriveLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;
        
    //     networkLayerDict["zOutputDriveLayer"].Output += networkLayerDict["zOutputDriveLayer"].InputConnectionDict["hiddenLayer"].ConnectionWeight.Multiply(networkLayerDict["hiddenLayer"].Output);
       
    //     networkLayerDict["outputDriveLayer"].Output += Matrix<float>.Tanh(networkLayerDict["zOutputDriveLayer"].Output);

    //     networkLayerDict["zOutputBodyLayer"].Output += networkLayerDict["zOutputBodyLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;
        
    //     networkLayerDict["zOutputBodyLayer"].Output += networkLayerDict["zOutputBodyLayer"].InputConnectionDict["hiddenLayer"].ConnectionWeight.Multiply(networkLayerDict["hiddenLayer"].Output);
        
    //     networkLayerDict["outputBodyLayer"].Output += Matrix<float>.Tanh(networkLayerDict["zOutputBodyLayer"].Output);
    //     networkLayerDict["zOutputActionLayer"].Output += networkLayerDict["zOutputActionLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;
        
    //     networkLayerDict["zOutputActionLayer"].Output += networkLayerDict["zOutputActionLayer"].InputConnectionDict["hiddenLayer"].ConnectionWeight.Multiply(networkLayerDict["hiddenLayer"].Output);
        
    //     networkLayerDict["outputActionLayer"].Output += Matrix<float>.Tanh(networkLayerDict["zOutputActionLayer"].Output);

    //     networkLayerDict["zOutputActionArgumentLayer"].Output += networkLayerDict["zOutputActionArgumentLayer"].InputConnectionDict["biasLayer"].ConnectionWeight;

    //     networkLayerDict["zOutputActionArgumentLayer"].Output += networkLayerDict["zOutputActionArgumentLayer"].InputConnectionDict["hiddenLayer"].ConnectionWeight.Multiply(networkLayerDict["hiddenLayer"].Output);

    //     networkLayerDict["outputActionArgumentLayer"].Output += Matrix<float>.Tanh(networkLayerDict["zOutputActionArgumentLayer"].Output);

    //     foreach (string memoryKey in recurrentMemoryDict.Keys.ToList())
    //     {
    //         networkLayerDict[recurrentInfoDict[memoryKey]].Output.CopyTo(recurrentMemoryDict[memoryKey]);
    //     }
    // }
    // create a copy of previous input layer

    void UpdateWeights(){
        // make sure we initialize all the non-bias layers to zero, and bias to 1

        // we may want to rename CalculatedThisUpdate and break it into two, CalculatedFeedforwardThisUpdate, and CalculatedWeightUpdateThisUpdate
        foreach (KeyValuePair<string, Layer> layerInfo in networkLayerDict)
        {
            layerInfo.Value.CalculatedThisUpdate = false;
        }
        foreach (KeyValuePair<string, Layer> outputLayer in outputLayerDict)
        {
            if(!outputLayer.Value.Name.Contains("zOutput"))
            {
                outputLayer.Value.CalculateCost();
            }
                
        }


        //     foreach all the connections
        //         connection.calculateDeltaWeights

        foreach (KeyValuePair<string, Layer> layerInfo in networkLayerDict)
        {
            foreach(KeyValuePair<string, Connection> connection in layerInfo.Value.OutputConnectionDict)
            {
                connection.Value.UpdateWeights();
            }
        }
    }

    void InitInputs(){
        // ideally this would loop that iterates over the inputDict data structure
        networkLayerDict["inputVisionRedLayer"].Output = visualInput.Row(0).ToColumnMatrix();
        networkLayerDict["inputVisionGreenLayer"].Output = visualInput.Row(1).ToColumnMatrix();
        networkLayerDict["inputVisionBlueLayer"].Output = visualInput.Row(2).ToColumnMatrix();
        networkLayerDict["inputDriveLayer"].Output = driveStates.ToColumnMatrix();
        networkLayerDict["inputBodyLayer"].Output = bodyStates.ToColumnMatrix();
        networkLayerDict["inputActionLayer"].Output = actionStates.ToColumnMatrix();
        networkLayerDict["inputActionArgumentLayer"].Output = actionArguments.ToColumnMatrix();
    }

    float distanceCounter = 0.0f;
    float rotationCounter = 0.0f;
    public override Matrix<float> ChooseAction()
    {
        

        counter += 1;
        if (counter % 100 == 0)
        {
            // the real choose action
            InitInputs();
            UpdateWeights();

            Feedforward();
            Debug.Log(networkLayerDict["outputActionLayer"].Output);
        }


        // hard coded walk in a square
        if (distanceCounter > 10.0f)
        {
            rotationCounter += 90;
            if(rotationCounter == 360)
            {
                rotationCounter = 0.0f;
            }
            this.gameobject.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, rotationCounter, 0);
            distanceCounter = 0.0f;
        }
        else
        {
            this.gameobject.transform.Translate(this.gameobject.transform.GetChild(0).forward * Time.deltaTime);
            distanceCounter += Time.deltaTime;
        }



        return networkLayerDict["outputActionLayer"].Output;
    }
}