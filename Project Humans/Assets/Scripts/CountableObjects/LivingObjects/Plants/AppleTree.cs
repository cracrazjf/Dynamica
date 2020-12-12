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
    public World theWorld;

    public bool doingNothing = true;
    
    /// <summary>
    /// AppleTree constructor
    /// </summary>
    public AppleTree(int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome, World theWorld): 
            base("AppleTree", index, position, motherGenome, fatherGenome) {

        this.theWorld = theWorld;
        
        appleTreePrefab = Resources.Load("TreeRoundPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(appleTreePrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        rigidbody = this.gameObject.GetComponent<Rigidbody>();

        // set the size of the tree to .1 the standard scale
        // this.gameObject.transform.localScale *= newVector3(.1,.1,.1);
    }
    
    public override void UpdatePlant(int updateCounter){
        this.IncreaseAge(1);
        float growthRefreshRate = this.phenotype.traitDict["growth_refresh_rate"];
        float fruitRate = this.phenotype.traitDict["fruit_rate"];
        float reproductionAge = this.phenotype.traitDict["reproduction_age"];
        int age = this.GetAge();
        //Debug.Log("Update Apple Tree");

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

        // figure out where the apple should appear
        float distance = this.phenotype.traitDict["fruit_drop_distance"];
        Vector3 fruitDisplacement = new Vector3(Random.Range(-distance,distance),0,Random.Range(-distance,distance));                           
        Vector3 fruitLocation = this.gameObject.transform.position + fruitDisplacement;

        List<string> sizeInfo;
        List<string> poisonInfo;
        List<string> colorInfo;

        Dictionary<string, List<string>> propertyDict = new Dictionary<string, List<string>>(theWorld.constantInfoDict["apple"]);

        float fruitSize = this.phenotype.traitDict["fruit_max_size"] * this.phenotype.traitDict["fruit_size_proportion"];
        sizeInfo = new List<string>{fruitSize.ToString(), "1"};
        propertyDict.Add("Size", sizeInfo);
        
        float poison = this.phenotype.traitDict["poison"];
        if (poison == 1){
            poisonInfo = new List<string>{"1", "1"};
            propertyDict.Add("poison", poisonInfo);
            colorInfo = new List<string>{"purple", "1"};
            propertyDict.Add("color", colorInfo);
        }

        else{
            poisonInfo = new List<string>{"0", "1"};
            propertyDict.Add("poison", poisonInfo);
            colorInfo = new List<string>{"red", "1"};
            propertyDict.Add("color", colorInfo);
        }

        int indexNumber = World.countableObjectCountDict["Apple"];
        Apple newApple = new Apple(indexNumber, fruitLocation, propertyDict);
        World.nonlivingObjectList.Add(newApple);
        World.nonlivingObjectDict[newApple.GetName()] = newApple;
        World.countableObjectCountDict["Apple"]++;
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

