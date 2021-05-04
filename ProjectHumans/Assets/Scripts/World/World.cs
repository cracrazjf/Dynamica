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


public class World : MonoBehaviour {
    public bool paused = false;
    public static int densityParam = 0;

    /// This dict keeps track of the total number of each kind of object that has been created
    public static Dictionary<string, int> entityCountDict = new Dictionary<string, int>();
    public Dictionary<string, int> startingCountsDict = new Dictionary<string, int>();
    public static List<string> animalNames = new List<string>();
    public static List<string> plantNames = new List<string>();
    public static List<string> itemNames = new List<string>();

    public static Dictionary<string, ObjectInfo> objectInfoDict = new Dictionary<string, ObjectInfo>();
    public static Dictionary<string, float> worldConfigDict = new Dictionary<string, float>();

    /// This dict keeps track of data in world.config
    public Dictionary<string, Dictionary<string, List<string>>> genomeInfoDict = new Dictionary<string, Dictionary<string, List<string>>>();
    public Dictionary<string, Dictionary<string, List<string>>> constantInfoDict = new Dictionary<string, Dictionary<string, List<string>>>();

    /// These dicts keep track of GameObjects
    public static Dictionary<string, Animal> animalDict = new Dictionary<string, Animal>();
    public static Dictionary<string, Plant> plantDict = new Dictionary<string, Plant>();
    public static Dictionary<string, Item> itemDict = new Dictionary<string, Item>();
    
    /// These lists keep track of entities needing an update each epoch
    public static List<Entity> entityList = new List<Entity>();
    public static Dictionary<string, Entity> entityDict = new Dictionary<string, Entity>();



    /// <value>Setting initial world properties</value>
    public static float worldSize;
    public static float maxPosition;
    public static float minPosition;

    public static bool initWorld = false;

    public Dictionary<string, AI> allAIDict;
    public Dictionary<string, Body> allBodyDict;
    public Dictionary<string, MotorSystem> allMotorDict;

    private bool updateCompleted = false;
    int updateCounter;

    void Start() {}

    public static void SetGo(bool start) {
        Debug.Log("Made it to world!");
        initWorld = start;
    }

    void CreateEntities() {
        foreach(KeyValuePair<string, int> entry in startingCountsDict) {
            string speciesType = entry.Key;
            entityCountDict[speciesType] = 0;

            if (speciesType == "TreeRound" || speciesType == "TreeBumpy") {
                break;

            } else {
                int numEntities = entry.Value;
                for (int i = 0; i < numEntities; i++) {
                    AddEntity(speciesType, null);
                }
            }
        }
        SpawnGroves();
        Debug.Log("All entities spawned");
        updateCompleted = true;
    }

    public void SpawnGroves() {
        List<Vector3>[] grid = InitGrid();
        System.Random rand = new System.Random();
        for ( int i = 0; i <  grid.Length; i++ ) {
            double option = rand.NextDouble();
            bool hasTrees = true;
            string speciesType = "";

            if (option < .2) {
                speciesType = "TreeBumpy";
            } else if (option < .5) {
                speciesType = "TreeRound";
            } else { hasTrees = false; }

            if (hasTrees) {
                for (int n = 0; n < grid[i].Count; n++) {
                    Vector3 location = grid[i][n];
                    AddEntity(speciesType, location);
                }
            }
        }
    }

    public static void DestroyComponent(Component passed) {
        Destroy(passed);
    }

    public static void AddEntity(string speciesType, Nullable<Vector3> passedSpawn) {
        Vector3 spawn;
        Genome motherGenome = new Genome();
        motherGenome.InitGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

        // Debug.Log("Spawning a " + speciesType );
        if (!passedSpawn.HasValue) { 
            spawn = CreateRandomPosition();
        } else { spawn = (Vector3) passedSpawn; }

        int val = (entityCountDict[speciesType]);
        if (itemNames.Contains(speciesType)) {
            InitItem(val, speciesType, motherGenome, spawn);
        } else {
            Genome fatherGenome = new Genome();
            fatherGenome.InitGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

            if (plantNames.Contains(speciesType)) {
            InitPlant(val, speciesType, motherGenome, fatherGenome, spawn);
            } else { InitAnimal(val, speciesType, motherGenome, fatherGenome, spawn); }
        } 
    }

    public static void RemoveEntity(string name) {
        for (int i = 0; i < entityList.Count; i++) {
            if (entityList[i].GetName() == name) {
                Destroy(entityList[i].GetGameObject());
                entityList.RemoveAt(i);
            }
        }
    }

    public static void InitAnimal(int val, string speciesType, Genome mother, Genome father, Vector3 spawn ) {
        // Debug.Log("making an animal in world");
        Animal newAnimal = new Animal(speciesType, val, mother, father, spawn);
        animalDict[newAnimal.GetName()] = newAnimal;
        entityList.Add(newAnimal);
        entityDict[newAnimal.GetName()] = newAnimal;
        entityCountDict[speciesType]++;
    }

    public static void InitPlant(int val, string speciesType, Genome mother, Genome father, Vector3 spawn) {
        // Debug.Log("making an plant in world");
        Plant newPlant = new Plant(speciesType, val, mother, father, spawn);
        plantDict[newPlant.GetName()] = newPlant;
        entityList.Add(newPlant);
        entityDict[newPlant.GetName()] = newPlant;
        entityCountDict[speciesType]++;
    }

    public static void InitItem(int val, string speciesType, Genome mother, Vector3 spawn) {
        // Debug.Log("making an item in world");
        Item newObj = new Item(speciesType, val, mother, spawn);
        itemDict[newObj.GetName()] = newObj;
        entityList.Add(newObj);
        entityDict[newObj.GetName()] = newObj;
        entityCountDict[speciesType]++;
    }

    public static Animal GetAnimal(string name) { return animalDict[name]; }
    
    public static Item GetItem(string name) { return itemDict[name]; }

    public static Plant GetPlant(string name) { return plantDict[name]; }

    public static Entity GetEntity(string name) { 
        return entityDict[name];
    }

    public void UpdateEntities() {
        int numEntities = entityList.Count;
        for (int i = 0; i < numEntities; i++) {
            entityList[i].UpdateEntity();
        }
        //Debug.Log("All entities updated");
        updateCompleted = true;
    }

    void Update() {
        if (initWorld) {
            initWorld = false;
            Debug.Log("Should only send once");
            updateCounter = 0;
        
            LoadWorldConfig();
            CreateObjectInfoInstances();

            worldSize = worldConfigDict["World_Size"];
            maxPosition = worldSize / 2;
            minPosition = -worldSize / 2;

            CreateEntities();
            MainUI.ToggleUpdate();
        }

        //Debug.Log("Started an update");
        paused = MainUI.GetPause();

        if(!paused) {
            if(updateCompleted) {
                updateCompleted = false;
                UpdateEntities();
                updateCounter++;
            }
        }
    }

    public static void SetDensity(int passed) {
        densityParam = passed;
        Debug.Log("Recieved density as: " + passed);
    }

    void LoadWorldConfig(){

        string line;
        string[] lineInfo;
        
        string name = "world" + densityParam + ".config";
        Debug.Log("Trying to open " + name);
        using (var reader = new StreamReader(@"Assets/Scripts/config/" + name)) {
            while ((line = reader.ReadLine()) != null) {
                lineInfo = line.Split(new[] { "," }, StringSplitOptions.None);

                if (lineInfo[0] == "Constant") {
                    Debug.Log(lineInfo[1]);
                    worldConfigDict.Add(lineInfo[1], float.Parse(lineInfo[2]));
                    
                } else {
                    if  (lineInfo[0] == "Animal") {animalNames.Add(lineInfo[1]);} 
                    if  (lineInfo[0] == "Plant") {plantNames.Add(lineInfo[1]);} 
                    if  (lineInfo[0] == "Item") {itemNames.Add(lineInfo[1]);} 
                    startingCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                    entityCountDict.Add(lineInfo[1], 0);
                }
            }
        }
    }

    void CreateObjectInfoInstances() {
        foreach(KeyValuePair<string, int> entry in startingCountsDict) {
            ObjectInfo newObjectInfo = new ObjectInfo(entry.Key, entry.Value);
            objectInfoDict.Add(entry.Key, newObjectInfo);
        }
    }

    public static Vector3 CreateRandomPosition() {
        float xRan = Random.Range(World.minPosition, World.maxPosition);
        float zRan = Random.Range(World.minPosition, World.maxPosition);
        Vector3 newStartPosition = new Vector3 (xRan, 0.5f, zRan); 

        return newStartPosition;
    }

    public static Quaternion CreateRandomRotation(){
        var startRotation = Quaternion.Euler(0.0f, Random.Range(World.minPosition,World.maxPosition), 0.0f);
        return startRotation;
    }

    public static void PrintStates(Dictionary<string, object> thisStateDict) {
        Debug.Log("START");
        foreach(KeyValuePair<string, object> entry in thisStateDict) {
            Debug.Log(entry.Key + " " + entry.Value);
        }
    }

    public List<Vector3>[] InitGrid() {
        // Okay so we're going to divide the map into an 8x8 grid
        // Each of those 64 zones will have an array of 10 random locations within the zone

        int gridDiv = 8;
        float bump = maxPosition / (gridDiv / 2);
        int numLocs = 10;
        List<Vector3>[] grid = new List<Vector3>[gridDiv * gridDiv]; 

        for (int i = 0; i < grid.Length; i++) {
            grid[i] = new List<Vector3>();
        }

        int index = 0;
        for (int xN = 0; xN < gridDiv - 1; xN++) {
            float minX = minPosition + (xN * bump);
            float maxX = minPosition + ((xN + 1) * bump);

            // Technically in unity this is the Z coord, but a 2D plane was being imagined for development
            for (int yN = 0; yN < gridDiv - 1; yN++) {
                float minY = minPosition + (yN * bump);
                float maxY = minPosition + ((yN + 1) * bump);

                for (int j = 0; j < numLocs; j++) {

                    float xRan = Random.Range(minX, maxX);
                    float zRan = Random.Range(minY, maxY);
                    Vector3 addPos = new Vector3 (xRan, 0, zRan);

                    Debug.Log(index);
                    grid[index].Add(addPos);
                }
                index++;
            }
        }
        return grid;
    }
}