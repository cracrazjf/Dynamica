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

    // w = [[1,1,1,1],[2,2,2,2],[3,3,3,3]]   dot((1,3),(3,4)) = (1,4)
    // x = [10,20,30]
    // y = w.Multiply(x) = [140, 140, 140, 140]
    // y1 = 10 + 40 + 90 
    // y2 = 10 + 40 + 90
    // y3 = 10 + 40 + 90
    // y4 = 10 + 40 + 90

}


