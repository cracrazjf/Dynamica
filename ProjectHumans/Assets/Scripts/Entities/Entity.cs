using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;

abstract public class Entity {

    private string displayName;
    public string GetDisplayName() { return displayName; }
    public void SetDisplayName(string named) { displayName = named; }

    private String objectType;
    public string GetObjectType() { return objectType;}

    private String name;
    public String GetName() { return name; }
    public void SetName(String passed) { name = passed; }

    private int index;
    public int GetIndex() { return index; }
    public void SetIndex(int passed) { index = passed; }

    private int age;
    public int GetAge(){ return this.age; }
    public void IncreaseAge(int amount){ this.age += amount; }

    private Genome genome;
    public Genome GetGenome() { return genome; }

    private Phenotype phenotype;
    public Phenotype GetPhenotype() { return phenotype; }
    
    // Body variables
    public GameObject gameObject;
    public Animator animator;
    public Vector3 startPosition;
    public Quaternion startRotation;

    // Sexual reproduction constructor
    public Entity(string objectType, int index, Genome motherGenome, Genome fatherGenome) {
        SetObjectType(objectType);
        SetIndex(index);
        name = (objectType + " " + index.ToString());

        startPosition = chooseStartPosition(null);
        startRotation = chooseStartRotation();

        genome = new Genome(this, motherGenome, fatherGenome);
        phenotype = new Phenotype(this);
    }

    // Asexual reproduction constructor
    public Entity(string objectType, int index, Genome motherGenome) {
        SetObjectType(objectType);
        SetIndex(index);
        name = (objectType + " " + index.ToString());

        startPosition = ChooseStartPosition(null);
        startRotation = ChooseStartRotation();

        this.genome = new Genome(this, motherGenome);
        phenotype = new Phenotype(this);
    }

    public Vector3 ChooseStartPosition(Nullable<Vector3> position){
        Vector3 newStartPosition = new Vector3();
        if (position != null) {
            newStartPosition = (Vector3)position;
        } else { 
            newStartPosition = new Vector3 (Random.Range(World.minPosition, World.maxPosition), 0.5f, Random.Range(World.minPosition, World.maxPosition)); 
        }
        return newStartPosition;
    }

    public Quaternion ChooseStartRotation(){
        
        var startRotation = Quaternion.Euler(0.0f, Random.Range(World.minPosition,World.maxPosition), 0.0f);
        return startRotation;
    }
}