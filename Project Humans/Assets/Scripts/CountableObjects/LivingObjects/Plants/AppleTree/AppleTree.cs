using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Configuration;
using System.Linq;
using Random=UnityEngine.Random;

public class AppleTree : Plant
{
    /// <value>AppleTree prefab</value>
    public GameObject appleTreePrefab;
    public GameObject applePrefab;
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    
    /// <summary>
    /// AppleTree constructor
    /// </summary>
    public AppleTree(int index, Genome motherGenome, Genome fatherGenome): base("AppleTree", index, motherGenome, fatherGenome) {
        // these should be moved up the hierarchy, countable object
        Vector3 startPosition = this.chooseStartPosition(null);
        Quaternion startRotation = this.chooseStartRotation();
        appleTreePrefab = Resources.Load("TreeRoundPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(appleTreePrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        rigidbody = this.gameObject.GetComponent<Rigidbody>();

        // set the size of the tree to .1 the standard scale
        // this.gameObject.transform.localScale *= newVector3(.1,.1,.1);
    }
    
    public override void UpdatePlant(int updateCounter){
        this.IncreaseAge(1);
        int growthRefreshRate = Int32.Parse(this.phenotype.traitDict["growth_refresh_rate"]);
        int fruitRate = Int32.Parse(this.phenotype.traitDict["fruit_rate"]);
        int reproductionAge = Int32.Parse(this.phenotype.traitDict["reproduction_age"]);
        int age = this.GetAge();
        Debug.Log("Update Apple Tree");

        if (age % growthRefreshRate == 0){
            Grow();
        }

        if (age >= reproductionAge){
            if (age % fruitRate == 0){
                
                CreateApple();
            }
        }
    }

    public void CreateApple(){
        Debug.Log("Create an Apple");
        float distance = float.Parse(this.phenotype.traitDict["fruit_drop_distance"]);
        Vector3 fruitDisplacement = new Vector3(Random.Range(-distance,distance),0,Random.Range(-distance,distance));                           
        Vector3 fruitLocation = this.gameObject.transform.position + fruitDisplacement;

        Dictionary<string, string> propertyDict = new Dictionary<string, string>();
        float fruitSize = float.Parse(this.phenotype.traitDict["fruit_max_size"]) * float.Parse(this.phenotype.traitDict["fruit_size_proportion"]);
        propertyDict.Add("Size", fruitSize.ToString());
        
        int poison = Int32.Parse(this.phenotype.traitDict["poison"]);

        if (poison == 1){
            propertyDict.Add("poison", "1");
            propertyDict.Add("color", "purple");
        }
        else{
            propertyDict.Add("poison", "0");
            propertyDict.Add("color", "red");
        }

        int indexNumber = -1; // this needs to be the value of world.countableObjectCountDict['Apple']
       
        Apple newApple = new Apple(indexNumber, fruitLocation, propertyDict);
        // nonlivingObject newNonlivingObject = new Apple(countableObjectCountDict[objectType]);
        // nonlivingObjectList.Add(newNonlivingObject);
        // nonlivingObjectDict[newNonlivingObject.GetName()] = newNonlivingObject;
        // countableObjectCountDict[objectType]++;
    }

    public void Grow(){
        // this code should go in Plant.cs, not Apple Tree. 

        // tree starts at scale of .1 in height (y) and width (x,z)
        // it has a max_height and max_width stored in this.phenotype.traitDict["max_height"] and this.phenotype.traitDict["max_width"]
        // every time grow is called, it should grow by its growth rate parameter (a float between 0 and 1)
        //      towards its max height and width. So for example, if its scale vector3 starts at [.1, .1, .1]
        //      if its max_height is 5 and its max_width is 4, and its this.phenotype.traitDict["growth_rate"] = .01
        //      then its new height should be y += .01*(5-.1) = .149
        //      its new width should be x,z += .01*(4-.1) = .139
    }
}

