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
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    
    /// <summary>
    /// AppleTree constructor
    /// </summary>
    public AppleTree(int index, Genome motherGenome, Genome fatherGenome): base("AppleTree", index, motherGenome, fatherGenome) {
        Vector3 startPosition = this.chooseStartPosition();
        Quaternion startRotation = this.chooseStartRotation();
        appleTreePrefab = Resources.Load("TreeRoundPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(appleTreePrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
        this.gameObject.AddComponent<FOVDetection>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();

        // set the size of the tree to .1 the standard scale
        // this.gameObject.transform.localScale *= newVector3(.1,.1,.1);
    }

    public Vector3 chooseStartPosition(){
            // TODO: rewrite this code so that an object appears in a position that is passed in, and random if no position is passed in
            // but it will have to be passed into the object constructor so that it can get passed here
            // for example, an apple's start positionß is determined as a function of the position of the tree that created it
            // move this code into CountableObject.cs
            var startPosition = new Vector3 (Random.Range(World.minPosition,World.maxPosition), 0.03f, Random.Range(World.minPosition,World.maxPosition));
            return startPosition;
        }
    
    public void UpdatePlant(int updateCounter){
        this.IncreaseAge(1);
        int growthRefreshRate = Int32.Parse(this.phenotype.traitDict["growth_refresh_rate"]);
        int fruitRate = Int32.Parse(this.phenotype.traitDict["fruit_rate"]);
        int reproductionAge = Int32.Parse(this.phenotype.traitDict["reproduction_age"]);
        int age = this.GetAge();

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
        float distance = float.Parse(this.phenotype.traitDict["fruit_drop_distance"]);
        Vector3 fruitDisplacement = new Vector3(Random.Range(-distance,distance),0,Random.Range(-distance,distance));                           
        Vector3 fruitLocation = this.gameObject.transform.position + fruitDisplacement;

        // despite the lines above, this currently is going to create the apple in a random location. not next to the tree
        //      we need edit the constructor of all countable objects so that it takes two new arguments,
        //          bool randomLocation and Vector3 startLocation
        //      if randomLocation is true, then generate the item in a random location as we currently do
        //      else, use the value in startLocation
        //      in Python, it would be possible for startLocation to be set to None
        //      if that is possible in C#, then the logic could be if None, pick a random location, else use the specified location

        // it needs access to world to see the index it needs to add for name
        // How do we add these things to the world info lists and dicts?
        // We could have each object contain a pointer back up to world, but is there another solution?
        Apple newApple = new Apple(-1);
        // nonlivingObject newNonlivingObject = new Apple(countableObjectCountDict[objectType]);
        // nonlivingObjectList.Add(newNonlivingObject);
        // nonlivingObjectDict[newNonlivingObject.GetName()] = newNonlivingObject;
        // countableObjectCountDict[objectType]++;

        float fruitSize = float.Parse(this.phenotype.traitDict["fruit_max_size"]) * float.Parse(this.phenotype.traitDict["fruit_size_proportion"]);
        int cyanide = Int32.Parse(this.phenotype.traitDict["cyanide"]);

        if (cyanide == 0){
            // set fruit color mesh to red
        }
        else{
            // set the fruit color mesh to purple
        }
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

