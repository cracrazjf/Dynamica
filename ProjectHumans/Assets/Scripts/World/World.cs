using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Configuration;
using System.Linq;
using System.IO;


public class World : MonoBehaviour
{
    /// <value> This value pauses or continues the Update function</value>
    public bool paused = false;

    /// <value> This dict keeps track of the total number of each kind of object that has been created</value>
    public static Dictionary<string, int> countableObjectCountDict = new Dictionary<string, int>();


    /// <value> This dict keeps track of the Living Info objects</value>
    public static Dictionary<string, LivingObjectInfo> livingObjectInfoDict = new Dictionary<string, LivingObjectInfo>();
    public static Dictionary<string, NonlivingObjectInfo> nonlivingObjectInfoDict = new Dictionary<string, NonlivingObjectInfo>();
    
    /// <value> This dict keeps track of data in world.config</value>
    public static Dictionary<string, float> worldConfigDict = new Dictionary<string, float>();
    public Dictionary<string, int> startingAnimalCountsDict = new Dictionary<string, int>();
    public Dictionary<string, int> startingPlantCountsDict = new Dictionary<string, int>();
    public Dictionary<string, int> startingNonLivingObjectCountsDict = new Dictionary<string, int>();

    /// <value> This dict keeps track of data in world.config</value>
    public Dictionary<string, Dictionary<string, List<string>>> genomeInfoDict = new Dictionary<string, Dictionary<string, List<string>>>();
    public Dictionary<string, Dictionary<string, List<string>>> constantInfoDict = new Dictionary<string, Dictionary<string, List<string>>>();

    /// <value> These dicts keep track of GameObjects </value>
    public static Dictionary<string, Animal> animalDict = new Dictionary<string, Animal>();
    public static Dictionary<string, Plant> plantDict = new Dictionary<string, Plant>();
    public static Dictionary<string, NonlivingObject> nonlivingObjectDict = new Dictionary<string, NonlivingObject>();
    
    /// <value> These lists keep track of entities needing an update each epoch</value>
    public static List<Animal> animalList = new List<Animal>();
    public static List<Plant> plantList = new List<Plant>();
    public static List<NonlivingObject> nonlivingObjectList = new List<NonlivingObject>();

    /// <value> These genome variables are used to instantiate every living thing that is created</value>
    public Genome motherGenome;
    public Genome fatherGenome;

    /// <value>Setting initial world properties</value>
    public static float worldSize;
    public static float maxPosition;
    public static float minPosition;

    int updateCounter;

    /// <summary>
    /// Start is called before the first frame update and initializes all scene objects
    /// </summary>
    void Start()
    { 
        updateCounter = 0;
        
        LoadWorldConfig();
        CreateObjectInfoInstances();

        worldSize = worldConfigDict["World_Size"];
        maxPosition = worldSize / 2;
        minPosition = -worldSize / 2;

        CreateAnimals();
        CreatePlants();
        CreateNonLivingObjects();
    }

    /// <summary>
    /// CreateAnimals initializes and places all the animals
    /// </summary>
    void CreateAnimals(){

        string speciesType;
        int n;
        Animal newAnimal;

        foreach(KeyValuePair<string, int> entry in startingAnimalCountsDict)
        {
            speciesType = entry.Key;
            // super important; can't ++ a key that does not yet exist! -jc
            countableObjectCountDict[speciesType] = 0;
            n = entry.Value;

            for (int i=0; i<n; i++){
                // create the pseudo-random parent genomes. this doesnt actually make the mother female and father male...
                motherGenome = new Genome();
                motherGenome.CreateGenomeFromSpeciesTemplate(livingObjectInfoDict[speciesType]);
                fatherGenome = new Genome();
                fatherGenome.CreateGenomeFromSpeciesTemplate(livingObjectInfoDict[speciesType]);

                if (speciesType == "Human"){
                    newAnimal = new Human(countableObjectCountDict[speciesType], null, motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
                    countableObjectCountDict[speciesType]++;
                }
                else if (speciesType == "Penguin"){
                    newAnimal = new Penguin(countableObjectCountDict[speciesType], null, motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
                    countableObjectCountDict[speciesType]++;
                }
                else if (speciesType == "Wolf"){
                    newAnimal = new Wolf(countableObjectCountDict[speciesType], null, motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
                    countableObjectCountDict[speciesType]++;
                }
                else if (speciesType == "Elephant"){
                    newAnimal = new Elephant(countableObjectCountDict[speciesType], null, motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
                    countableObjectCountDict[speciesType]++;
                }                
                else if (speciesType == "Deer"){
                    newAnimal = new Deer(countableObjectCountDict[speciesType], null, motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
                    countableObjectCountDict[speciesType]++;
                }
                else if (speciesType == "Llama"){
                    newAnimal = new Llama(countableObjectCountDict[speciesType], null, motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
                    countableObjectCountDict[speciesType]++;
                }
            }
        }
    }

    /// <summary>
    /// CreatePlants initializes and places all the nonliving objects in the world
    /// </summary>  
    void CreatePlants()
    {
        string speciesType;
        int n;
        Plant newPlant;

        foreach(KeyValuePair<string, int> entry in startingPlantCountsDict)
        {
            speciesType = entry.Key;
            // super important; can't ++ a key that does not yet exist! -jc
            countableObjectCountDict[speciesType] = 0;
            n = entry.Value;

            for (int i=0; i<n; i++){
                // create the pseudo-random parent genomes
                motherGenome = new Genome();
                motherGenome.CreateGenomeFromSpeciesTemplate(livingObjectInfoDict[speciesType]);
                fatherGenome = new Genome();
                fatherGenome.CreateGenomeFromSpeciesTemplate(livingObjectInfoDict[speciesType]);

                // should be able to move lines 77-79 and 83-85 out of these if's, but it creates an error I dont understand
                if (speciesType == "Apple_Tree"){
                    newPlant = new AppleTree(countableObjectCountDict[speciesType], null, motherGenome, fatherGenome, this);
                    plantList.Add(newPlant);
                    plantDict[newPlant.GetName()] = newPlant;
                    countableObjectCountDict[speciesType]++;
                }
            }
        }
    }

    /// <summary>
    /// CreateNonLivingObjects initializes and places all the nonliving objects in the world
    /// </summary>  
    void CreateNonLivingObjects()
    {
        NonlivingObject newNonlivingObject;
        string objectType;
        int n;
        Nullable<Vector3> position = null;
        
        foreach(KeyValuePair<string, int> entry in startingNonLivingObjectCountsDict){
            objectType = entry.Key;
            n = entry.Value;

            for (int i=0; i<n; i++){
                if (objectType == "Water"){
                    newNonlivingObject = new Water(countableObjectCountDict[objectType], null, nonlivingObjectInfoDict[objectType]);
                    nonlivingObjectList.Add(newNonlivingObject);
                    nonlivingObjectDict[newNonlivingObject.GetName()] = newNonlivingObject;
                    countableObjectCountDict[objectType]++;
                }
            }
        }
    }

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
        for(int i= 0; i< animalList.Count; i++) {
            animalList[i].UpdateAnimal();
        }
    }
    
    public void UpdatePlants() {
        for(int i= 0; i< plantList.Count; i++) {
            plantList[i].UpdatePlant(updateCounter);
        }
    }

    public void LateUpdateNonlivingObjects() {
        for (int i = 0; i<nonlivingObjectList.Count; i++) {
            nonlivingObjectList[i].NonlivingObjectLateUpdate();
        }
    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        paused = MainUI.GetPause();

        if(!paused) {
            UpdateAnimals(); //breaks
            UpdatePlants();
            updateCounter++;
        }
    }

    void LateUpdate() {
        LateUpdateNonlivingObjects();
    }

    /// <summary>
    /// loadWorldConfig loads the information from Assets/config/world.config into the appropriate config dict or starting count dict
    /// </summary>
    void LoadWorldConfig(){
        DirectoryInfo d = new DirectoryInfo(@"Assets/Scripts/config/");
        FileInfo[] Files = d.GetFiles("*.config"); //Getting Text files
        string line;
        string[] lineInfo;
        string objectType;
        string lineType;

        string propertyName;

        using (var reader = new StreamReader(@"Assets/Scripts/config/world.config"))
        {
            while ((line = reader.ReadLine()) != null)
            {
                lineInfo = line.Split(new[] { "," }, StringSplitOptions.None);
                if (lineInfo[0] == "Animal"){
                    startingAnimalCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                    countableObjectCountDict.Add(lineInfo[1], 0);
                }
                else if (lineInfo[0] == "Plant"){
                    startingPlantCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                    countableObjectCountDict.Add(lineInfo[1], 0);
                }
                else if (lineInfo[0] == "Nonliving_Object"){
                    startingNonLivingObjectCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                    countableObjectCountDict.Add(lineInfo[1], 0);
                }
                else if (lineInfo[0] == "Constant"){
                    worldConfigDict.Add(lineInfo[1], float.Parse(lineInfo[2]));
                }
            }
        }
    
    }

    void CreateObjectInfoInstances(){
        foreach(KeyValuePair<string, int> entry in startingAnimalCountsDict)
        {
            LivingObjectInfo newLivingObjectInfo = new LivingObjectInfo(entry.Key, entry.Value);
            livingObjectInfoDict.Add(entry.Key, newLivingObjectInfo);
        }
        foreach(KeyValuePair<string, int> entry in startingPlantCountsDict)
        {
            LivingObjectInfo newLivingObjectInfo = new LivingObjectInfo(entry.Key, entry.Value);
            livingObjectInfoDict.Add(entry.Key, newLivingObjectInfo);
        }
        foreach(KeyValuePair<string, int> entry in startingNonLivingObjectCountsDict)
        {
            NonlivingObjectInfo newNonlivingObjectInfo = new NonlivingObjectInfo(entry.Key, entry.Value);
            nonlivingObjectInfoDict.Add(entry.Key, newNonlivingObjectInfo);
        }
        
    }

}