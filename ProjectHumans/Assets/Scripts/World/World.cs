using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Reflection;


public class World : MonoBehaviour {
    public bool paused = false;

    /// This dict keeps track of the total number of each kind of object that has been created
    public static Dictionary<string, int> EntityCountDict = new Dictionary<string, int>();
    public Dictionary<string, int> startingCountsDict = new Dictionary<string, int>();
    public Dictionary<string, GameObject> prefabsDict = new Dictionary<string, GameObject>();
    public List<string> animalNames = new List<string>();
    public List<string> plantNames = new List<string>();
    public List<string> itemNames = new List<string>();

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
    public static List<Entity> entityList = new List<Item>();

    /// These genome variables are used to instantiate every living thing that is created
    public Genome motherGenome;
    public Genome fatherGenome;

    /// <value>Setting initial world properties</value>
    public static float worldSize;
    public static float maxPosition;
    public static float minPosition;

    public Dictionary<string, AI> allAIDict;
    public Dictionary<string, Body> allBodyDict;
    public Dictionary<string, MotorSystem> allMotorDict;

    int updateCounter;

    void Start() {
        updateCounter = 0;
        
        LoadWorldConfig();
        CreateObjectInfoInstances();

        worldSize = worldConfigDict["World_Size"];
        maxPosition = worldSize / 2;
        minPosition = -worldSize / 2;

        CreateEntities();
    }

    void CreateEntities() {
        foreach(KeyValuePair<string, int> entry in startingCountsDict) {
            string speciesType = entry.Key;
            entityCountDict[speciesType] = 0;
            int numEntities = entry.Value;

            for (int i = 0; i < numEntities; i++) {
                AddEntity(speciesType, null);
            }
        }
    }

    public void AddEntity(string speciesType, Transform spawn) {
        motherGenome = new Genome();
        motherGenome.InitGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

        if (spawn == null) { spawn = new Transform(CreateRandomPosition(), CreateRandomRotation()); }

        int val = (entityCountDict[speciesType]);
        if (itemList.Contain(speciesType)) {
            InitItem(val, speciesType, motherGenome, spawn);
        } else {
            fatherGenome = new Genome();
            fatherGenome.InitGenomeFromSpeciesTemplate(objectInfoDict[speciesType]);

            if (plantList.Contain(speciesType)) {
            InitPlant(val, speciesType, motherGenome, fatherGenome, spawn);
            } else { InitAnimal(val, speciesType, motherGenome, fatherGenome, spawn); }
        } 
    }

    public void InitAnimal(int val, string speciesType, Genome mother, Genome father, Transform spawn ) {
        Entity newAnimal = new Animal(speciesType, val, mother, father, spawn);
        animalDict[newAnimal.GetName()] = newAnimal;
        entityList.Add(newAnimal);
        entityCountDict[speciesType]++;
    }

    public void InitPlant(int val, string speciesType, Genome mother, Genome father, Transform spawn) {
        Entity newPlant = new Plant(speciesType, val, mother, father, spawn);
        plantDict[newPlant.GetName()] = newPlant;
        entityList.Add(newPlant);
        entityCountDict[speciesType]++;
    }

    public void InitItem(int val, string speciesType, Genome mother, Transform spawn) {
        Entity newObj = new Item(speciesType, val, mother, spawn);
        itemDict[newObj.GetName()] = newObj;
        entityList.Add(newObj);
        entityCountDict[speciesType]++;
    }

    public Body InitBody(Entity passed, Transform spawn) {
        Body toReturn;
        string species = passed.GetObjectType();
        
        if(animalNames.Contains(species)) {
            toReturn = new PrimateBody((Animal) passed, spawn);
            toReturn.InitGameObject(prefabsDict[species]);
            return toReturn;
        } else {
            toReturn = new Body(passed, spawn);
            toReturn.InitGameObject(prefabsDict[species]);
            return toReturn;
        }
    }

    public MotorSystem InitAnimalMotor(Animal passed) {
        return new PrimateMotorSystem(passed);
    }

    void LoadPrefabs() {
        prefabsDict.Add("Human", Resources.Load("Prefabs/HumanPrefab", typeof(GameObject)) as GameObject);
        prefabsDict.Add("Llama", Resources.Load("Prefabs/LlamaPrefab", typeof(GameObject)) as GameObject);
        prefabsDict.Add("Apple", Resources.Load("Prefabs/ApplePrefab", typeof(GameObject)) as GameObject);
        prefabsDict.Add("Human", Resources.Load("Prefabs/WaterPrefab", typeof(GameObject)) as GameObject);
        prefabsDict.Add("Tree", Resources.Load("Prefabs/TreeRoundPrefab", typeof(GameObject)) as GameObject);
    }

    public static Animal GetAnimal(string name) { return animalDict[name]; }
    public static Item GetItem(string name) { return itemDict[name]; }
    public static Plant GetPlant(string name) { return plantDict[name]; }

    public void UpdateEntities() {
        foreach(Entity entity in entityList) {
            entity.UpdateEntity();
        }
    }

    void Update() {
        paused = MainUI.GetPause();

        if(!paused) {
            UpdateEntities();
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

    public Vector3 CreateRandomPosition() {
        float xRan = Random.Range(World.minPosition, World.maxPosition);
        float zRan = Random.Range(World.minPosition, World.maxPosition);
        Vector3 newStartPosition = new Vector3 (xRan, 0.5f, zRan); 

        return newStartPosition;
    }

    public Quaternion CreateRandomRotation(){
        var startRotation = Quaternion.Euler(0.0f, Random.Range(World.minPosition,World.maxPosition), 0.0f);
        return startRotation;
    }
}