using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Configuration;
using System.Linq;


public class World : MonoBehaviour
{
    /// <value> This dict keeps track of the total number of each kind of object that has been created</value>
    private static Dictionary<string, int> countableObjectCountDict = new Dictionary<string, int>();
    
    /// <value> This dict keeps track of data in world.config</value>
    private Dictionary<string, string> worldConfigDict = new Dictionary<string, string>();
    private Dictionary<string, int> startingAnimalCountsDict = new Dictionary<string, int>();
    private Dictionary<string, int> startingPlantCountsDict = new Dictionary<string, int>();
    private Dictionary<string, int> startingNonLivingObjectCounts = new Dictionary<string, int>();

    /// <value> These dicts keep track of GameObjects </value>
    private static Dictionary<string, Animal> animalDict = new Dictionary<string, Animal>();
    private static Dictionary<string, Plant> plantDict = new Dictionary<string, Plant>();
    private static Dictionary<string, NonlivingObject> nonlivingObjectDict = new Dictionary<string, NonlivingObject>();
    
    /// <value> These lists keep track of entities needing an update each epoch</value>
    public static List<Animal> animalList = new List<Animal>();
    public static List<Plant> plantList = new List<Plant>();
    public static List<NonlivingObject> nonlivingObjectList = new List<NonlivingObject>();

    /// <value> These genome variables are used to instantiate every living thing that is created</value>
    public Genome motherGenome;
    public Genome fatherGenome;

    /// <value>Setting initial world properties</value>
    public static float worldSize = 20.0f;
    public static float maxPosition = World.worldSize / 2;
    public static float minPosition = -World.worldSize / 2;


    /// <summary>
    /// Start is called before the first frame update and initializes all scene objects
    /// </summary>
    void Start()
    { 
        loadWorldConfig();
        CreateAnimals();
        CreateNonLivingObjects();
        // CreateApples();
        // CreateWater();
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
                    newAnimal = new Human(countableObjectCountDict[speciesType], motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
                    countableObjectCountDict[speciesType]++;
                }
                else if (speciesType == "Penguin"){
                    newAnimal = new Penguin(countableObjectCountDict[speciesType], motherGenome, fatherGenome);
                    animalList.Add(newAnimal);
                    animalDict[newAnimal.GetName()] = newAnimal;
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

        foreach(KeyValuePair<string, int> entry in startingNonLivingObjectCounts){
            objectType = entry.Key;
            n = entry.Value;
            countableObjectCountDict.Add(objectType, 0);

            for (int i=0; i<n; i++){
                if (objectType == "Apple"){
                    newNonlivingObject = new Apple(countableObjectCountDict[objectType]);
                    nonlivingObjectList.Add(newNonlivingObject);
                    nonlivingObjectDict[newNonlivingObject.GetName()] = newNonlivingObject;
                    countableObjectCountDict[objectType]++;
                }
                else if (objectType == "Water"){
                    newNonlivingObject = new Water(countableObjectCountDict[objectType]);
                    nonlivingObjectList.Add(newNonlivingObject);
                    nonlivingObjectDict[newNonlivingObject.GetName()] = newNonlivingObject;
                    countableObjectCountDict[objectType]++;
                }
            }
        }

    }


    // /// <summary>
    // /// CreateApples initializes and places numApples appleInstance objectsFs randomly in the world
    // /// </summary>
    // void CreateApples()
    // {
    //     int n = startingNonLivingObjectCounts["Apple"];
    //     countableObjectCountDict.Add("Apple", 0);
    //     for (int i=0; i<n; i++){

    //         // create an instance of the apple class
    //         NonlivingObject newApple = new Apple(countableObjectCountDict["Apple"]);
    //         countableObjectCountDict["Apple"]++;
            
    //         nonlivingObjectDict.Add(newApple.GetName(), newApple);
    //     }
    // }


    // /// <summary>
    // /// CreateWater initializes and places numApples waterInstance objects randomly in the world
    // /// </summary>
    // void CreateWater()
    // {
    //     int n = startingNonLivingObjectCounts["Water"];
    //     countableObjectCountDict.Add("Water", 0);
    //     for (int i=0; i<n; i++){

    //         // create an instance of the water class
    //         NonlivingObject newWater = new Water(countableObjectCountDict["Water"]);
    //         countableObjectCountDict["Water"]++;

    //         nonlivingObjectDict.Add(newWater.GetName(), newWater);
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
        for(int i= 0; i< animalList.Count; i++) {
            
            //humanList[i].TestUpdate();
            animalList[i].UpdateAnimal();
        }
    }
    
    int updateCounter = 0;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        UpdateAnimals();
    }

    /// <summary>
    /// loadWorldConfig loads the information from Assets/config/world.config into the appropriate config dict or starting count dict
    /// </summary>
    void loadWorldConfig(){
        // int counter = 0;  
        string line;
        System.IO.StreamReader file;
        
        string filename = @"Assets/config/world.config";
        file = new System.IO.StreamReader(filename);  
        
        while((line = file.ReadLine()) != null)  
        {  
            string[] lineInfo = line.Split(new[] { "," }, StringSplitOptions.None);
            if (lineInfo[0] == "Animal_Count"){
                startingAnimalCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
            }
            else if (lineInfo[0] == "Plant_Count"){
                startingPlantCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
            }
            else if (lineInfo[0] == "Nonliving_Object_Count"){
                startingNonLivingObjectCounts.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
            }
            else{
                worldConfigDict.Add(lineInfo[1], lineInfo[2]);
            }
        }  
        file.Close();
    }


}