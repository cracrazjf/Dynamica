using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class World : MonoBehaviour
{

    /// <value>Init starting numbers for objects</value>
    private int numApples = 5;
    private int numWater = 5;
    private int numPenguins = 2;
    public const int numHumans = 1;

    /// <value>Creating object lists</value>
    public static bool populationChanged = false;

    public static List<Animal> humanList = new List<Animal>();
    private static Dictionary<string, Animal> allAnimalDict = new Dictionary<string, Animal>();
    private static Dictionary<string, NonlivingObject> allObjectDict = new Dictionary<string, NonlivingObject>();
    //private static Dictionary<string, int> speciesCount = new Dictionary<string, int>();

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
        for (int i=0; i<numApples; i++){

            string name = "Apple " + i.ToString();

            // create an instance of the human class
            NonlivingObject newApple = new Apple(name);
            allObjectDict.Add(name, newApple);
        }
    }

    /// <summary>
    /// CreateWater initializes and places numApples waterInstance objects randomly in the world
    /// </summary>
    void CreateWater()
    {
        for (int i=0; i<numWater; i++){

            string name = "Water " + i.ToString();

            // create an instance of the human class
            NonlivingObject newWater = new Water(name);
            allObjectDict.Add(name, newWater);
        }
    }

    public Genome motherGenome;
    public Genome fatherGenome;

    /// <summary>
    /// CreateHumans initializes and places numHumanMales and numHumanFemales HumanMale and HumanFemale objects randomly in the world
    /// </summary>
    void CreateHumans()
    {
        for (int i=0; i<numHumans; i++){
    
            // create the pseudo-random parent genomes
            motherGenome = new Genome();
            motherGenome.CreateGenome("human");
            fatherGenome = new Genome();
            fatherGenome.CreateGenome("human");

            string name = "Human " + i.ToString();

            // create an instance of the human class
            Animal newHuman = new Human(name, motherGenome, fatherGenome);
            humanList.Add(newHuman);
            allAnimalDict.Add(name, newHuman);
        }
    }

    void CreatePenguins()
    {
        for (int i=0; i<numPenguins; i++){
    
            // create the pseudo-random parent genomes
            motherGenome = new Genome();
            motherGenome.CreateGenome("penguin");
            fatherGenome = new Genome();
            fatherGenome.CreateGenome("penguin");

            string name = "Penguin " + i.ToString();

            // create an instance of the human class
            Animal newPenguin = new Penguin(name, motherGenome, fatherGenome);
            allAnimalDict.Add(name, newPenguin);
            
        }
    } 

    public static Animal GetAnimal(string name) {
        return allAnimalDict[name];
    }

    public static NonlivingObject GetObject(string name) {
        return allObjectDict[name];
    }


    public void UpdateHumans() {
       
        for(int i= 0; i< humanList.Count; i++) {
            //humanList[i].TestUpdate();
            humanList[i].UpdateAnimal();
        }
    }
    
    int updateCounter = 0;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        UpdateHumans();
    }
}



