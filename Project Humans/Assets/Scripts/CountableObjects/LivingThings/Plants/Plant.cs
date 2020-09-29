using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Plant : LivingObject
{
    public Plant(Genome motherGenome, Genome fatherGenome) : base (motherGenome,fatherGenome)
        {
            //super(name);
        }
}