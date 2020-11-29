using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Plant : LivingObject
{
    public Plant(string objectType, int index, Genome motherGenome, Genome fatherGenome) : base (objectType, index, motherGenome, fatherGenome)
    {

    }

}