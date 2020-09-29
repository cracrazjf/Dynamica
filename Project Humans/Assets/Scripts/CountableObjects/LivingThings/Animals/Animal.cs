using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    abstract public class Animal : LivingObject
    {
        public Animal(Genome motherGenome, Genome fatherGenome) : base (motherGenome,fatherGenome)
        {
            
        }
    }
