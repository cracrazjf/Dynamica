using UnityEngine;
using System;

public class MathParabola 
{
    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        //Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, 0 ,mid.z);
    }
}