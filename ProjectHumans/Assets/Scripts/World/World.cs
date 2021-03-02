using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Configuration;
using System.Linq;
using System.IO;


public class World : MonoBehaviour {
    public bool paused = false;

    /// This dict keeps track of the total number of each kind of object that has been created
    public static Dictionary<string, int> countableObjectCountDict = new Dictionary<string, int>();
    public Dictionary<string, int> startingCountsDict = new Dictionary<string, int>();
    public List<string> animalNames = new List<string>();
    public List<string> plantNames = new List<string>();

    public static Dictionary<string, ObjectInfo> objectInfoDict = new Dictionary<string, ObjectInfo>();
    public static Dictionary<string, float> worldConfigDict = new Dictionary<string, float>();

    /// This dict keeps track of data in world.config
    public Dictionary<string, Dictionary<string, List<string>>> genomeInfoDict = new Dictionary<string, Dictionary<string, List<string>>>();
    public Dictionary<string, Dictionary<string, List<string>>> constantInfoDict = new Dictionary<string, Dictionary<string, List<string>>>();

    /// These dicts keep track of GameObjects
    public static Dictionary<string, Animal> animalDict = new Dictionary<string, Animal>();
    public static Dictionary<string, Plant> plantDict = new Dictionary<string, Plant>();
    public static Dictionary<string, NonlivingObject> nonlivingObjectDict = new Dictionary<string, NonlivingObject>();
    
    /// These lists keep track of entities needing an update each epoch
    public static List<Animal> animalList = new List<Animal>();
    public static List<Plant> plantList = new List<Plant>();
    public static List<NonlivingObject> nonlivingObjectList = new List<NonlivingObject>();

    /// These genome variables are used to instantiate every living thing that is created
    public Genome motherGenome;
    public Genome fatherGenome;

    /// <value>Setting initial world properties</value>
    public static float worldSize;
    public static float maxPosition;
    public static float minPosition;

    int updateCounter;


    void Start() {
        updateCounter = 0;
        
        LoadWorldConfig();
        CreateObjectInfoInstances();

        worldSize = worldConfigDict["World_Size"];
        maxPosition = worldSize / 2;
        minPosition = -worldSize / 2;

        CreateEntities();
        CreateAnimals();
        CreatePlants();
        CreateNonLivingObjects();
    }

    void CreateEntities() {
        foreach(KeyValuePair<string, int> entry in startingCountsDict) {
            speciesType = entry.Key;
            countableObjectCountDict[speciesType] = 0;
            numEntities = entry.Value;

            for (int i = 0; i < numEntities; i++) {
                if (animalNames.Exists(speciesType)) {
                    AddAnimal(speciesType);
                } else if(plantNames.Exists(speciesType)) {
                    AddPlant(speciesType);
                } else { AddObject(speciesType); }
            }
        }
    }

    void AddAnimal(string speciesType) {
        motherGenome = new Genome();
        motherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);
        fatherGenome = new Genome();
        fatherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

        int val = (countableObjectCountDict[speciesType]);
        Class<?> classy = Class.forName(speciesType);
        Constructor<?> cons = classy.getConstructor(String.class);
        Animal newAnimal = cons.newInstance(val, motherGenome, fatherGenome);
        animalDict[newAnimal.GetName()] = newAnimal;
        countableObjectCountDict[speciesType]++;
    }

    void AddPlant(string speciesType) {
        motherGenome = new Genome();
        motherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);
        fatherGenome = new Genome();
        fatherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

        int val = (countableObjectCountDict[speciesType]);
        Class<?> classy = Class.forName(speciesType);
        Constructor<?> cons = classy.getConstructor(String.class);
        plant newPlant = cons.newInstance(val, motherGenome, fatherGenome);
        plantDict[newPlant.GetName()] = newPlant;
        countableObjectCountDict[speciesType]++;
    }

    void AddObject(string type) {
        int val = (countableObjectCountDict[type]);
        ObjectInfo toSend = objectInfoDict[objectType];

        Class<?> classy = Class.forName(type);
        Constructor<?> cons = classy.getConstructor(String.class);
        NonlivingObject newObj = cons.newInstance(val, toSend);
        nonlivingObjectDict[newObj.GetName()] = newObjt;
        countableObjectCountDict[type]++;
    }

    // void CreateAnimals() {

    //     string speciesType;
    //     int numEntities;
    //     Animal newAnimal;

    //     foreach(KeyValuePair<string, int> entry in startingAnimalCountsDict)
    //     {
    //         speciesType = entry.Key;
    //         countableObjectCountDict[speciesType] = 0;
    //         numEntities = entry.Value;

    //         for (int i = 0; i < numEntities; i++){
    //             // create the pseudo-random parent genomes. this doesnt actually make the mother female and father male...
    //             motherGenome = new Genome();
    //             motherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);
    //             fatherGenome = new Genome();
    //             fatherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

    //             if (speciesType == "Human"){
    //                 newAnimal = new Human(countableObjectCountDict[speciesType], motherGenome, fatherGenome);
    //                 animalList.Add(newAnimal);
    //                 animalDict[newAnimal.GetName()] = newAnimal;
    //                 countableObjectCountDict[speciesType]++;
    //             } else if (speciesType == "Llama"){
    //                 newAnimal = new Llama(countableObjectCountDict[speciesType], motherGenome, fatherGenome);
    //                 animalList.Add(newAnimal);
    //                 animalDict[newAnimal.GetName()] = newAnimal;
    //                 countableObjectCountDict[speciesType]++;
    //             }
    //         }
    //     }
    // }

    // void CreatePlants() {
    //     string speciesType;
    //     int numEntities;
    //     Plant newPlant;

    //     foreach(KeyValuePair<string, int> entry in startingPlantCountsDict)
    //     {
    //         speciesType = entry.Key;
    //         // super important; can't ++ a key that does not yet exist! -jc
    //         countableObjectCountDict[speciesType] = 0;
    //         numEntities = entry.Value;

    //         for (int i = 0; i < numEntities; i++){
    //             // create the pseudo-random parent genomes
    //             motherGenome = new Genome();
    //             motherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);
    //             fatherGenome = new Genome();
    //             fatherGenome.CreateGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

    //             // should be able to move lines 77-79 and 83-85 out of these if's, but it creates an error I dont understand
    //             if (speciesType == "Apple_Tree"){
    //                 newPlant = new AppleTree(countableObjectCountDict[speciesType], motherGenome, fatherGenome, this);
    //                 plantList.Add(newPlant);
    //                 plantDict[newPlant.GetName()] = newPlant;
    //                 countableObjectCountDict[speciesType]++;
    //             }
    //         }
    //     }
    // }

    // void CreateNonLivingObjects() {
    //     NonlivingObject newNonlivingObject;
    //     string objectType;
    //     int numEntities;
        
    //     foreach(KeyValuePair<string, int> entry in startingNonLivingObjectCountsDict){
    //         objectType = entry.Key;
    //         numEntities = entry.Value;

    //         for (int i=0; i < numEntities; i++){
    //             if (objectType == "Water"){
    //                 newNonlivingObject = new Water(countableObjectCountDict[objectType], nonlivingObjectInfoDict[objectType]);
    //                 nonlivingObjectList.Add(newNonlivingObject);

    //                 nonlivingObjectDict[newNonlivingObject.GetName()] = newNonlivingObject;
    //                 countableObjectCountDict[objectType]++;
    //             }
    //         }
    //     }
    // }

    public static Animal GetAnimal(string name) {
        return animalDict[name];
    }

    public static NonlivingObject GetObject(string name) {
        return nonlivingObjectDict[name];
    }

    public static Plant GetPlant(string name) {
        return plantDict[name];
    }

    public void UpdateAnimals() {
        for(int i= 0; i < animalList.Count; i++) {
            animalList[i].UpdateAnimal();
        }
    }
    
    public void UpdatePlants() {
        for(int i= 0; i < plantList.Count; i++) {
            plantList[i].UpdatePlant(updateCounter); // I take issue with this but other priorities exist
        }
    }

    public void UpdateNonlivings() {
        for (int i = 0; i < nonlivingObjectList.Count; i++) {
            nonlivingObjectList[i].NonlivingObjectUpdate();
        }
    }

    void Update() {
        paused = MainUI.GetPause();

        if(!paused) {
            UpdateAnimals(); 
            UpdatePlants();
            UpdateNonLivings();
            updateCounter++;
        }
    }

    void LoadWorldConfig(){
        DirectoryInfo d = new DirectoryInfo(@"Assets/Scripts/config/");
        FileInfo[] Files = d.GetFiles("*.config"); //Getting Text files
        string line;
        string[] lineInfo;
        string objectType;
        string lineType;

        string propertyName;

        using (var reader = new StreamReader(@"Assets/Scripts/config/world.config")) {
            while ((line = reader.ReadLine()) != null) {
                lineInfo = line.Split(new[] { "," }, StringSplitOptions.None);

                if (lineInfo[0] == "Constant") {
                    worldConfigDict.Add(lineInfo[1], float.Parse(lineInfo[2]));
                    
                } else {
                    if  (lineInfo[0] == "Animal") {animalNames.Add(lineInfo[1]);} 
                    if  (lineInfo[0] == "Plant") {plantNames.Add(lineInfo[1]);} 
                    startingCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                    countableObjectCountDict.Add(lineInfo[1], 0);
                }
            }
        }
    }

    void CreateObjectInfoInstances() {
        foreach(KeyValuePair<string, int> entry in startingCountsDict) {
            if(animalNames.Exists(entry.Key) || (plantNames.Exists(entry.Key))) {
                ObjectInfo newObjectInfo = new ObjectInfo(entry.Key, entry.Value, true);
                objectInfoDict.Add(entry.Key, newObjectInfo);
            }
            ObjectInfo newObjectInfo = new ObjectInfo(entry.Key, entry.Value, false);
            objectInfoDict.Add(entry.Key, newObjectInfo);
        }
    }
}