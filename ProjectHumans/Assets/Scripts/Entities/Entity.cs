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

    protected string species;
    public string GetSpecies() { return species;}

    protected string name;
    public string GetName() { return name; }
    public void SetName(String passed) { name = passed; }

    protected int index;
    public int GetIndex() { return index; }
    public void SetIndex(int passed) { index = passed; }

    protected int age;
    public int GetAge(){ return this.age; }
    public void IncreaseAge(int amount){ this.age += amount; }

    protected Genome genome;
    public Genome GetGenome() { return genome; }

    protected Phenotype phenotype;
    public Phenotype GetPhenotype() { return phenotype; }
    
    // Body variables
    protected GameObject gameObject;
    protected Body body;

    public GameObject GetGameObject() { return gameObject; }
    public void SetGameObject(GameObject passed) { this.gameObject = passed; }
    public Body GetBody() { return body; }

    // Sexual reproduction constructor
    public Entity(string objType, int id, Genome motherGenome, Genome fatherGenome, Vector3 spawn) {
        species = objType;
        index = id;
        name = (species + " " + index.ToString());
        displayName = name;

        genome = new Genome(motherGenome, fatherGenome);
        phenotype = new Phenotype(this);
    }

    // Asexual reproduction constructor
    public Entity(string objType, int id, Genome motherGenome, Vector3 spawn) {
        species = objType;
        index = id;
        name = (species + " " + index.ToString());
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