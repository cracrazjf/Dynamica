using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;


public class TanhActivation : Connection
{
    public override Matrix<float> GetNetInputArray(Matrix<float> input)
    {
        return Matrix<float>.Tanh(input);
    }
    public override void CalculateDeltaWeights(Matrix<float> output, Matrix<float> cost)
    {
        
    }
}
