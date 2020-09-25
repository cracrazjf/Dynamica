using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class LivingObject : CountableObject
{
    Genome Genome;
    Phenotype Phenotype;

    LivingObject(String name) {
        super(name);
    }
};