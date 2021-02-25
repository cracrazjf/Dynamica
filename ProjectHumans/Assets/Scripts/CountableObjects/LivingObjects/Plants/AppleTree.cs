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
    public string displayName;

    public bool doingNothing = true;
    
    /// <summary>
    /// AppleTree constructor
    /// </summary>
    public AppleTree(int index, Genome motherGenome, Genome fatherGenome, World theWorld): 
            base("AppleTree", index, motherGenome, fatherGenome) {

        this.theWorld = theWorld;
        
        appleTreePrefab = Resources.Load("Prefabs/TreeRoundPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(appleTreePrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        rigidbody = this.gameObject.GetComponent<Rigidbody>();

        // set the size of the tree to .1 the standard scale
        // this.gameObject.transform.localScale *= newVector3(.1,.1,.1);
    }
    
    public override void UpdatePlant(int updateCounter){
        this.IncreaseAge(1);
        float growthRefreshRate = this.phenotype.GetTraitDict()["growth_refresh_rate"];
        float fruitRate = this.phenotype.GetTraitDict()["fruit_rate"];
        float reproductionAge = this.phenotype.GetTraitDict()["reproduction_age"];
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

        // figure out where the apple should appear
        float distance = this.phenotype.GetTraitDict()["fruit_drop_distance"];
        Vector3 fruitDisplacement = new Vector3(Random.Range(-distance,distance),0,Random.Range(-distance,distance));                           
        Vector3 fruitLocation = this.gameObject.transform.position + fruitDisplacement;

        int indexNumber = World.countableObjectCountDict["Apple"];
        Apple newApple = new Apple(indexNumber, World.nonlivingObjectInfoDict["Apple"]);
        newApple.startPosition = fruitLocation;

        float size = this.phenotype.GetTraitDict()["fruit_max_size"] * this.phenotype.GetTraitDict()["fruit_size_proportion"];
        float poison = this.phenotype.GetTraitDict()["poison"];
    
        newApple.AddConstant("size", size);
        newApple.AddConstant("poison", poison);

        World.nonlivingObjectList.Add(newApple);
        World.nonlivingObjectDict[newApple.GetName()] = newApple;
        World.countableObjectCountDict["Apple"]++;
    }
}

