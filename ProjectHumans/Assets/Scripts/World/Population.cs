using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;


public class Population {
    // this is the initial number that will be spawned
    public string name;
    public float spawnChance = 0.0f;
    public Genome genome;
    public bool isItem = false;
    public bool isAnimal = false;
    public bool isPlant = false;
    public bool isFirst = false;

    public string contrastPop = "";
    protected List<Entity> entityList = new List<Entity>();

    public Population(string passedSpeciesName, float passedSpawnChance) {
        name = passedSpeciesName;
        spawnChance = passedSpawnChance;
        string toCast = "";

        this.genome = new Genome();
        ImportPopConfig();
    
        contrastPop = genome.GetQual("contrast_species");
        if (contrastPop != "none") {
            toCast = genome.GetQual("first_contrast");
        }
        if (toCast == "true") { isFirst = true; }
    }

    public void ImportPopConfig(){
        string line;
        System.IO.StreamReader file;
        
        string filename = @"Assets/Scripts/config/"+ name.ToLower() + ".config";
        file = new System.IO.StreamReader(filename);  

        while((line = file.ReadLine()) != null)  
        {  
            string[] lineInfo = line.Split(new[] { "=" }, StringSplitOptions.None);
            string[] leftArray = lineInfo[0].Split(new[] { "." }, StringSplitOptions.None);
            string[] rightArray = lineInfo[1].Split(new[] { "," }, StringSplitOptions.None);

            
            if (leftArray[0] == "gene") { genome.AddGeneToGenome(leftArray[1], rightArray); 
            } else if (leftArray[0] == "constant") {
                genome.AddConstantToGenome(leftArray[1], rightArray);
            } else if (leftArray[0] == "quality") {
                genome.AddQualToGenome(leftArray[1], rightArray);
            } else if (leftArray[0] == "object_type") {
                if (rightArray[0] == "animal") {
                    isAnimal = true;
                } else if (rightArray[0] == "plant") {
                    isPlant = true;
                } else { isItem = true; }
            }
        }  
        file.Close();
    }

    public void SaveEntity(Entity passed) {
        entityList.Add(passed);
    }

    public Dictionary<string, float> AverageDrives() {
        Dictionary<string, float> averages = new Dictionary<string, float>();

        foreach (Entity ent in entityList) {
            Animal animal = (Animal) ent;
            DriveSystem currentDrives = animal.GetDriveSystem();
            if (averages.Count == 0) {
                averages = currentDrives.GetStateDict();
            } else {
                foreach (KeyValuePair<string, float> entry in currentDrives.GetStateDict()) {
                    averages[entry.Key] += currentDrives.GetState(entry.Key);
                }
            }
        }

        foreach (KeyValuePair<string, float> entry in averages) {
            averages[entry.Key] = (entry.Value / entityList.Count);
        }

        return averages;
    }
}