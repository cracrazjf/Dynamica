using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Configuration;
using System.Linq;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using Random=UnityEngine.Random;

public class Population {
    // this is the initial number that will be spawned
    public string name;
    protected int numGroups;
    protected float meanMembers;
    protected float standardMembers;
    protected float spawnDensity;
    protected int updateRate;

    public int GetUpdateRate() { return updateRate; }

    public Genome baseGenome;
    public string entityType;
    public bool isFirst = false;

    protected List<Entity> entityList = new List<Entity>();
    protected Dictionary<string, Entity> entityDict = new Dictionary<string, Entity>();

    public  List<Entity> GetEntityList() { return entityList; }
    public List<string> GetEntityNames() { return new List<string>(entityDict.Keys); }

    public Population(string passedSpeciesName, int groups, float mean, float std, float density, int refresh) {
        name = passedSpeciesName;
        baseGenome = new Genome();
        numGroups = groups;
        meanMembers = mean;
        standardMembers = std;
        spawnDensity = density;
        updateRate = refresh;

        ImportPopConfig();
    }

    public Population(string passedSpeciesName, int refresh = 1) {
        name = passedSpeciesName;
        baseGenome = new Genome();
        updateRate = refresh;
        
        ImportPopConfig();
    }

    public void UpdatePopulation() {
        World.LogComment("Starting updating population: " + name);
        foreach(Entity individual in entityList) {
            individual.UpdateEntity();
        }
        World.LogComment("Population update completed.");
    }

    public void ImportPopConfig() {
        string line;
        string[] lineInfo;
        string filename = name + ".config";

        using (var reader = new StreamReader(@"Assets/Scripts/config/" + filename)) {
            while ((line = reader.ReadLine()) != null) {
                lineInfo = line.Split(new[] { "=" }, StringSplitOptions.None);
                string[] leftArray = lineInfo[0].Split(new[] { "." }, StringSplitOptions.None);
                string[] rightArray = lineInfo[1].Split(new[] { "," }, StringSplitOptions.None);

                if (leftArray[0] == "gene") { baseGenome.AddGeneToGenome(leftArray[1], rightArray); 
                } else if (leftArray[0] == "constant") {
                    baseGenome.AddConstantToGenome(leftArray[1], rightArray);
                } else if (leftArray[0] == "quality") {
                    baseGenome.AddQualToGenome(leftArray[1], rightArray);
                } else if (leftArray[0] == "object_type") {
                    entityType = rightArray[0];
                    Debug.Log("Saving object type for " + name + " as " + entityType);
                } 
            }
        } 
    }

    public void SaveEntity(Entity passed) {
        entityList.Add(passed);
        entityDict.Add(passed.GetName(), passed);
    }
}