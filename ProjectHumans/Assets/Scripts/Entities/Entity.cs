using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Random=UnityEngine.Random;

abstract public class Entity {

    public string displayName;
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
    protected GameObject gameObject;

    // Sexual reproduction constructor
    public Entity(string objType, int id, Genome motherGenome, Genome fatherGenome, Transform spawn) {
        objectType = objType;
        index = id;
        name = (objectType + " " + index.ToString());
        displayName = name;

        genome = new Genome(motherGenome, fatherGenome);
        phenotype = new Phenotype(this);

        body = World.InitBody(objectType);
    }

    // Asexual reproduction constructor
    public Entity(string objType, int id, Genome motherGenome, Transform spawn) {
        objectType = objType;
        index = id;
        name = (objectType + " " + index.ToString());
        displayName = name;

        genome = new Genome(motherGenome);
        phenotype = new Phenotype(this);
    }

    public virtual void UpdateEntity() {
        Debug.Log("No update defined for this entity");
    }

    public virtual void InitBody() {
        Debug.Log("This entity is not of this realm");
    }
}