using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class World : MonoBehaviour
{
    // move the num of everything into a config file
    private int numApples = 5;
    private int numWater = 5;
    private int numPenguins = 0;
    public const int numHumans = 2;

    private static Dictionary<string, int> countableObjectCountDict = new Dictionary<string, int>();
    
    /// <value> These dicts keep track of GameObject </value>
    private static Dictionary<string, NonlivingObject> nonlivingObjectDict = new Dictionary<string, NonlivingObject>();
    private static Dictionary<string, Animal> animalDict = new Dictionary<string, Animal>();
    private static Dictionary<string, Plant> plantDict = new Dictionary<string, Plant>();
    
    /// <value> These lists keep track of entities needing an update each epoch</value>
    public static List<Animal> animalList = new List<Animal>();
    public static List<Plant> plantList = new List<Plant>();

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
        CreateHumans();
        CreateApples();
        CreateWater();
    }


    /// <summary>
    /// CreateApples initializes and places numApples appleInstance objectsFs randomly in the world
    /// </summary>
    void CreateApples()
    {
        countableObjectCountDict.Add("Apple", 0);
        for (int i=0; i<numApples; i++){

            // create an instance of the apple class
            NonlivingObject newApple = new Apple(countableObjectCountDict["Apple"]);
            countableObjectCountDict["Apple"]++;
            
            nonlivingObjectDict.Add(newApple.name, newApple);
        }
    }


    /// <summary>
    /// CreateWater initializes and places numApples waterInstance objects randomly in the world
    /// </summary>
    void CreateWater()
    {
        countableObjectCountDict.Add("Water", 0);
        for (int i=0; i<numWater; i++){

            // create an instance of the water class
            NonlivingObject newWater = new Water(countableObjectCountDict["Water"]);
            countableObjectCountDict["Water"]++;

            nonlivingObjectDict.Add(newWater.name, newWater);
        }
    }


    /// <summary>
    /// CreateHumans initializes and places numHumanMales and numHumanFemales HumanMale and HumanFemale objects randomly in the world
    /// </summary>
    void CreateHumans()
    {
        countableObjectCountDict.Add("Human", 0);
        for (int i=0; i<numHumans; i++){
    
            // create the pseudo-random parent genomes
            motherGenome = new Genome();
            motherGenome.CreateGenome("Human");
            fatherGenome = new Genome();
            fatherGenome.CreateGenome("Human");

            // create an instance of the human class
            Animal newHuman = new Human(countableObjectCountDict["Human"], motherGenome, fatherGenome);
            countableObjectCountDict["Human"]++;
            
            animalList.Add(newHuman);
            animalDict.Add(newHuman.name, newHuman);
        }
    }


    void CreatePenguins()
    {
        countableObjectCountDict.Add("Penguin", 0);
        for (int i=0; i<numPenguins; i++){
    
            // create the pseudo-random parent genomes
            motherGenome = new Genome();
            motherGenome.CreateGenome("Penguin");
            fatherGenome = new Genome();
            fatherGenome.CreateGenome("Penguin");

            // create an instance of the human class
            Animal newPenguin = new Penguin(countableObjectCountDict["Penguin"], motherGenome, fatherGenome);
            countableObjectCountDict["Penguin"]++;
            
            animalList.Add(newPenguin);
            animalDict.Add(newPenguin.name, newPenguin);
            
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
}



