using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class LivingObject : CountableObject
{
    public Genome genome;
    public Phenotype phenotype;
    public string test = "test";

    public LivingObject(string objectType, Genome motherGenome, Genome fatherGenome)
    {
        this.genome = new Genome();
        this.genome.thisLivingObject = this;
        this.genome.inheretGenome(motherGenome, fatherGenome);
        this.phenotype = new Phenotype(this);
        
    }
};