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

    public static Dictionary<string, Population> populationDict = new Dictionary<string, Population>();
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
        foreach(KeyValuePair<string, Population> entry in populationDict) {
            Population activePop = entry.Value;
            string speciesType = entry.Key;
            entityCountDict[speciesType] = 0;

            Debug.Log(populationDict);

            if (activePop.contrastPop == "none") {
                int numEntities = activePop.startingN;
                for (int i = 0; i < numEntities; i++) {
                    Debug.Log(speciesType);
                    AddEntity(activePop, null);
                }
            } else if (!activePop.isFirst) {
                Debug.Log("Printing " + activePop.contrastPop);
                SpawnGroves(populationDict[activePop.contrastPop], activePop, .2f, .5f);
            }
        }
        Debug.Log("All entities spawned");
        updateCompleted = true;
    }

    // pass a single object, spawn random


    // make general, spawn contrastively for 2
    public void SpawnGroves(Population first, Population second, float chanceEmpty, float chanceFirst) {
        List<Vector3>[] grid = InitGrid();
        System.Random rand = new System.Random();
        
        for ( int i = 0; i <  grid.Length; i++ ) {

            bool hasTrees = true;
            double option = rand.NextDouble();
            if (option < chanceEmpty) {
                hasTrees = false;
            }
            
            Population toPass = second;
            option = rand.NextDouble();
            if (option < chanceFirst) {
                toPass = first;
            }

            if (hasTrees) {
                for (int n = 0; n < grid[i].Count; n++) {
                    Vector3 location = grid[i][n];
                    AddEntity(toPass, location);
                }
            }
        }
    }

    public static void DestroyComponent(Component passed) {
        Destroy(passed);
    }

    public static void AddEntity(string species, Nullable<Vector3> passedSpawn) {
        Population toPass = populationDict[species];
        AddEntity(toPass, passedSpawn);
    }

    public static void AddEntity(Population population, Nullable<Vector3> passedSpawn) {
        Vector3 spawn;
        Genome motherGenome = new Genome();
        motherGenome.InheritGenome(population.genome, true);

        // Debug.Log("Spawning a " + speciesType );
        if (!passedSpawn.HasValue) { 
            spawn = CreateRandomPosition();
        } else { spawn = (Vector3) passedSpawn; }

        int val = (entityCountDict[population.name]);
        Entity newEnt = null;
        string speciesType = population.name;

        if (population.isItem) {
            Item newObj = new Item(speciesType, val, motherGenome, spawn);
            itemDict[newObj.GetName()] = newObj;
            newEnt = newObj;

        } else {
            Genome fatherGenome = new Genome();
            fatherGenome.InheritGenome(population.genome, true);
            
            if (population.isPlant) {
                Plant newPlant = new Plant(speciesType, val, motherGenome, fatherGenome, spawn);
                plantDict[newPlant.GetName()] = newPlant;
                newEnt = newPlant;
            } else {
                Animal newAnimal = new Animal(speciesType, val, motherGenome, fatherGenome, spawn);
                animalDict[newAnimal.GetName()] = newAnimal;
                newEnt = newAnimal;
            }
        } 
        entityList.Add(newEnt);
        entityDict[newEnt.GetName()] = newEnt;
        entityCountDict[speciesType]++;
        population.SaveEntity(newEnt);
    }

    public static void RemoveEntity(string name) {
        for (int i = 0; i < entityList.Count; i++) {
            if (entityList[i].GetName() == name) {
                Destroy(entityList[i].GetGameObject());
                entityList.RemoveAt(i);
            }
        }
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
            CreatePopulations();

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
                    startingCountsDict.Add(lineInfo[1], Int32.Parse(lineInfo[2]));
                    entityCountDict.Add(lineInfo[1], 0);
                }
            }
        }
    }

    void CreatePopulations() {
        foreach(KeyValuePair<string, int> entry in startingCountsDict) {
            Population newPop = new Population(entry.Key, entry.Value);
            Debug.Log(entry.Key);
            populationDict[entry.Key] = newPop;
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

                    grid[index].Add(addPos);
                    //AddEntity("PinkCube", addPos);
                }
                index++;
            }
        }
        return grid;
    }
}