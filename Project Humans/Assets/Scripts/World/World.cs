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
    /// <value> This dict keeps track of the total number of each kind of object that has been created</value>
    public static Dictionary<string, int> countableObjectCountDict = new Dictionary<string, int>();
    
    /// <value> This dict keeps track of data in world.config</value>
    public Dictionary<string, string> worldConfigDict = new Dictionary<string, string>();
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
    public static float worldSize = 20.0f; // this needs to be in world.config
    public static float maxPosition = World.worldSize / 2;
    public static float minPosition = -World.worldSize / 2;

    int updateCounter;

    /// <summary>
    /// Start is called before the first frame update and initializes all scene objects
    /// </summary>
    void Start()
    { 
        updateCounter = 0;
        loadWorldConfig();
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
            n = entry.Value;
            countableObjectCountDict.Add(speciesType, 0);

            for (int i=0; i<n; i++){
                // create the pseudo-random parent genomes
                motherGenome = new Genome();
                motherGenome.CreateGenome(speciesType);
                fatherGenome = new Genome();
                fatherGenome.CreateGenome(speciesType);

                // should be able to move lines 77-79 and 83-85 out of these if's, but it creates an error I dont understand
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
            n = entry.Value;
            countableObjectCountDict.Add(speciesType, 0);

            for (int i=0; i<n; i++){
                // create the pseudo-random parent genomes
                motherGenome = new Genome();
                motherGenome.CreateGenome(speciesType);
                fatherGenome = new Genome();
                fatherGenome.CreateGenome(speciesType);

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
            countableObjectCountDict.Add(objectType, 0);

            for (int i=0; i<n; i++){
                if (objectType == "Water"){
                    newNonlivingObject = new Water(countableObjectCountDict[objectType], null, constantInfoDict["water"]);
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

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        UpdateAnimals();
        UpdatePlants();
        updateCounter++;
    }

    /// <summary>
    /// loadWorldConfig loads the information from Assets/config/world.config into the appropriate config dict or starting count dict
    /// </summary>
    void loadWorldConfig(){
        DirectoryInfo d = new DirectoryInfo(@"Assets/config/");
        FileInfo[] Files = d.GetFiles("*.config"); //Getting Text files
        string line;
        string[] lineInfo;
        string objectType;
        string lineType;

        string propertyName;

        foreach(FileInfo file in Files){
            if (file.Name == "world.config"){
                using (StreamReader sr = file.OpenText()){
                    
                    while ((line = sr.ReadLine()) != null){
                        lineInfo = line.Split(new[] { "," }, StringSplitOptions.None);
                        if (lineInfo[0] == "Animal_Count"){
                            startingAnimalCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                        }
                        else if (lineInfo[0] == "Plant_Count"){
                            startingPlantCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                        }
                        else if (lineInfo[0] == "Nonliving_Object_Count"){
                            startingNonLivingObjectCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                        }
                        else{
                            worldConfigDict.Add(lineInfo[1], lineInfo[2]);
                        }
                    }
                }  
            }
            else{

                string[] fileNameInfo = file.Name.Split(new[] { "." }, StringSplitOptions.None);
                objectType = fileNameInfo[0];

                constantInfoDict.Add(fileNameInfo[0], new Dictionary<string, List<string>>());
                genomeInfoDict.Add(fileNameInfo[0], new Dictionary<string, List<string>>());

                using (StreamReader sr = file.OpenText()){
                    while ((line = sr.ReadLine()) != null){
                        // genome.sex=binary,1,mutable,.3,.25,1
                        lineInfo = line.Split(new[] { "=" }, StringSplitOptions.None);
                        string[] leftArray = lineInfo[0].Split(new[] { "." }, StringSplitOptions.None);
                        string[] rightArray = lineInfo[1].Split(new[] { "," }, StringSplitOptions.None);
                        lineType = leftArray[0];
                        propertyName = leftArray[1];

                        if (lineInfo[0] == "genome"){
                            genomeInfoDict[objectType].Add(leftArray[1], rightArray.ToList());
                        }
                        else if(lineInfo[0] == "constant"){
                            constantInfoDict[objectType].Add(leftArray[1], rightArray.ToList());
                        }
                    }
                }
            }
        }
    }     
}