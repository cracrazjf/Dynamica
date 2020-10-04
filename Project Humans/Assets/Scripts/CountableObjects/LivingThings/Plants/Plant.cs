using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Plant : LivingObject
{
    public Plant(string objectType, Genome motherGenome, Genome fatherGenome) : base (objectType, motherGenome, fatherGenome)
        {
            //super(name);
        }
}