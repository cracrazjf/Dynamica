using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Plant : LivingObject
{
    public Plant(string objectType, int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome) : 
        base (objectType, index, position, motherGenome, fatherGenome)
    {
    
    }

    public virtual void UpdatePlant(int updateCounter) {
            Debug.Log("No update defined for this plant");
    }

}