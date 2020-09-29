using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class LivingObject : CountableObject
{
    Genome Genome;
    Phenotype Phenotype;

    public LivingObject( Genome motherGenome, Genome fatherGenome)
    {
        //super(name);
    }
};