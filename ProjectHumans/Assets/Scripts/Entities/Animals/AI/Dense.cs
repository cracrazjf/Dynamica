using System.Collections;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

public class Dense : Connection
{
    public Dense() : base()
    {
        this.updateType = "tanh_prime";
    }
    public override Matrix<float> GetNetInputArray(Matrix<float> input)
    {
        return this.connectionWeight.Multiply(input);
    }

}


