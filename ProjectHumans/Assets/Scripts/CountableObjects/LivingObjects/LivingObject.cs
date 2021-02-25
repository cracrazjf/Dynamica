using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class LivingObject : CountableObject
{
    public Genome genome;
    public Phenotype phenotype;
    

    public LivingObject(string objectType, int index, Genome motherGenome, Genome fatherGenome) 
    : base (objectType, index)
    {
        this.genome = new Genome();
        this.genome.thisLivingObject = this;
        this.genome.InheretGenome(motherGenome, fatherGenome);
        this.phenotype = new Phenotype(this);

        this.SetAge(0);
    }

    public Phenotype GetPhenotype(){
        return phenotype;
    }

    public Genome GetGenome(){
        return genome;
    }

};