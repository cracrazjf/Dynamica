using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class LivingObject : CountableObject
{
    Genome Genome;
    Phenotype Phenotype;

    public LivingObject(string name) : base(name)
    {
        //super(name);
    }
};