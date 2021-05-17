using System.Collections;
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
    protected float learning_rate = 0.3f;
    protected Matrix<float> deltaWeight;
    protected Matrix<float> senderCost;

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
    public Matrix<float> CalculateNetCost(Matrix<float> output, Matrix<float> cost)
    {
        
        if (connectionType == "dense")
        {
            if (updateType == "tanh_prime")
            {
                Matrix<float> resizedConnectionWeight = connectionWeight.Resize(connectionWeight.ColumnCount, connectionWeight.RowCount);
                deltaWeight = cost.PointwiseMultiply(TanhPrime(output));
                senderCost = resizedConnectionWeight * deltaWeight;
            }
            else if (updateType == "sigmoid_prime")
            {

            }
            else
            {
                // is the case where there were no transformations, in which case its 
                senderCost = cost;
            }
        }
        else
        {
            // is the case where there were no transformations, in which case its 
            senderCost = cost;
        }
        return senderCost;
    }

    //public void CalculateDeltaWeight(Matrix<float> output){
    //    // these variables should be defined above initialized to all zeros, and set here
    //    if (connectionType == "dense")
    //    {
    //        //deltaWeight = Matrix<float>.Build.Dense()
    //        if (updateType == "tanh_prime")
    //        {
    //            deltaWeight = deltaWeight * output.Resize(output.ColumnCount, output.RowCount);
    //        }
    //        else if (updateType == "sigmoid_prime")
    //        {

    //        }
    //        else
    //        {
    //            // is the case where there were no transformations, in which case its 
    //            //deltaWeight = cost;
    //        }
    //    }
    //    else
    //    {
    //        // is the case where there were no transformations, in which case its 
    //        //deltaWeight = cost;
    //    }
    //}

    public void UpdateWeights(Matrix<float> output) {
        if(connectionType == "dense")
        {
            Matrix<float> resizedOutput = output.Resize(output.ColumnCount, output.RowCount);
            //Debug.Log(sender + " " + deltaWeight);
            connectionWeight += deltaWeight * resizedOutput * learning_rate;
        }
    }

    Matrix<float> TanhPrime(Matrix<float> output){
        Matrix<float> tanh_prime_of_output = (Matrix<float>.Tanh(output).PointwisePower(2).SubtractFrom(1));
        return tanh_prime_of_output;        
    }

    /*
        o_delta = o_cost * self.sigmoid_prime(o)  [5]

        h_cost = np.dot(o_delta, self.o_h)
        h_delta = h_cost * self.tanh_prime(h)

        self.o_bias += o_delta * learning_rate

                        5,1                 1,128
        self.o_h += (np.dot(o_delta.reshape(len(o_delta), 1), h.reshape(1, len(h))) * learning_rate)

        self.h_bias += h_delta * learning_rate
        self.h_x += (np.dot(h_delta.reshape(len(h_delta), 1), x.reshape(1, len(x))) * learning_rate)
    */
}
