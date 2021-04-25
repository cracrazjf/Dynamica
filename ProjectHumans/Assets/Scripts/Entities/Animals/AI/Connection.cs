﻿using System.Collections;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;


public class Connection
{
    protected string sender = "None";
    protected string recipient = "None";
    protected string connectionType = "None";
    protected string updateType = "None"; // tanh_prime
    protected Matrix<float> connectionWeight;
    protected float learning_rate = 0.6f;
    protected Matrix<float> deltaWeight;

    public Connection()
    {
    }

    public string Sender
    {
        get { return sender; }
        set { sender = value; }
    }
    public string Recipient
    {
        get { return recipient; }
        set { recipient = value; }
    }
    public string ConnectionType
    {
        get { return connectionType; }
        set { connectionType = value; }
    }
    public Matrix<float> ConnectionWeight
    {
        get { return connectionWeight; }
        set { connectionWeight = value; }
    }
    public Matrix<float> DeltaWeight
    {
        get { return deltaWeight; }
        set { deltaWeight = value; }
    }
    public virtual Matrix<float> GetNetInputArray(Matrix<float> x)
    {
        return connectionWeight;
    }

    public Matrix<float> CalculateDeltaWeights(Matrix<float> output, Matrix<float> cost)
    {
        // these variables should be defined above initialized to all zeros, and set here
        if (connectionType == "dense")
        {
            //deltaWeight = Matrix<float>.Build.Dense()
            if (updateType == "tanh_prime")
            {
                Matrix<float> resizedWeight = connectionWeight.Resize(connectionWeight.ColumnCount, connectionWeight.RowCount);
                deltaWeight = resizedWeight.Multiply(TanhPrime(output, cost));
            }
            else if (updateType == "sigmoid_prime")
            {

            }
            else
            {
                // is the case where there were no transformations, in which case its 
                deltaWeight = cost;
            }
        }
        else
        {
            // is the case where there were no transformations, in which case its 
            deltaWeight = cost;
        }
        //Debug.Log(sender + " " + deltaWeight);
        return deltaWeight;

    }

    public void UpdateWeights()
    {
        if (connectionType == "dense")
        {

        }
    }

    Matrix<float> TanhPrime(Matrix<float> output, Matrix<float> cost)
    {
        Matrix<float> tanh_prime_of_output = (Matrix<float>.Tanh(output).PointwisePower(2).SubtractFrom(1));
        deltaWeight = cost.PointwiseMultiply(tanh_prime_of_output);
        return tanh_prime_of_output;
    }
}
