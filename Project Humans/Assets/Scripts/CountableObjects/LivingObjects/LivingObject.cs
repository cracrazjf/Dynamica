using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class LivingObject : CountableObject
{
    public Genome genome;
    public Phenotype phenotype;
    
    public string test = "test";

    public LivingObject(string objectType, int index, float nutrition, float healthEffect, Genome motherGenome, Genome fatherGenome) 
    : base (objectType, index, nutrition, healthEffect)
    {
        this.genome = new Genome();
        this.genome.thisLivingObject = this;
        this.genome.inheretGenome(motherGenome, fatherGenome);
        this.phenotype = new Phenotype(this);
    }

    public Phenotype GetPhenotype(){
        return phenotype;
    }

    public Genome GetGenome(){
        return genome;
    }
};