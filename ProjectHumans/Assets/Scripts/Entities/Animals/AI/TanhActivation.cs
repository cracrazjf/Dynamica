using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;


public class TanhActivation : Connection
{
    public TanhActivation() : base()
    {
        this.updateType = "None";
    }
    public override Matrix<float> GetNetInputArray(Matrix<float> input)
    {
        return Matrix<float>.Tanh(input);
    }
}
