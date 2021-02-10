using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : LivingObject
{
    public string displayName;
    private Body body;

    public Plant(string objectType, int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome) : 
        base (objectType, index, position, motherGenome, fatherGenome)
    {
        this.displayName = GetObjectType();  
    }

    public string GetDisplayName() {
        return displayName;
    }

    public void SetDisplayName(string named){
        this.displayName = named;
    }

    public virtual void UpdatePlant(int updateCounter) {
            Debug.Log("No update defined for this plant");
    }

    public void Grow(){
        // tree starts at scale of .1 in height (y) and width (x,z)
        // it has a max_height and max_width stored in this.phenotype.traitDict["max_height"] and this.phenotype.traitDict["max_width"]
        // every time grow is called, it should grow by its growth rate parameter (a float between 0 and 1)
        //      towards its max height and width. So for example, if its scale vector3 starts at [.1, .1, .1]
        //      if its max_height is 5 and its max_width is 4, and its this.phenotype.traitDict["growth_rate"] = .01
        //      then its new height should be y += .01*(5-.1) = .149
        //      its new width should be x,z += .01*(4-.1) = .139
    }

}