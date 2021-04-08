using System.Collections;
using UnityEngine;
using NumSharp;
using System.Collections.Generic;



public class SigmoidActivation : Connection
{
    Dictionary<string, Layer> layerDict;
    public SigmoidActivation(Dictionary<string, Layer> passedLayerDict)
    {
        this.layerDict = passedLayerDict;
        //connectionWeight = new float[layerDict[sender].NumUnits, layerDict[recipient].NumUnits];
    }
}
