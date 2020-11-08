using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class World : MonoBehaviour
{
    /// <value>Create unity game objects</value>
    public GameObject Water;
    public GameObject Apple;
    public GameObject Ground;
    public GameObject HumanFemale;
    public GameObject HumanMale;

    /// <value>Init starting numbers for objects</value>
    private int numApples = 5;
    private int numWater = 5;
    public const int numHumans = 2;

    /// <value>Creating object lists</value>
    public static bool populationChanged = false;
    private static List<Animal> humanList = new List<Animal>();

    /// <value>Setting initial world properties</value>
    public static float worldSize = 20.0f;
    public static float maxPosition = World.worldSize / 2;
    public static float minPosition = -World.worldSize / 2;

    /// <summary>
    /// Start is called before the first frame update and initializes all scene objects
    /// </summary>
    void Start()
    { 
        //CreateGround();
        CreateHumans();
        CreateApples();
        CreateWater();
    }

/*     /// <summary>
    /// CreateGround initializes the Ground object
    /// </summary>
    void CreateGround()
    {
        GameObject groundInstance = GameObject.Instantiate(Ground) as GameObject;
        var groundScript = groundInstance.GetComponent<Ground>();
        groundScript.Init(worldSize);
    } */

    /// <summary>
    /// CreateApples initializes and places numApples appleInstance objectsFs randomly in the world
    /// </summary>
    void CreateApples()
    {
        for (int i=0; i<numApples; i++){
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 0, Random.Range(minPosition,maxPosition));
            GameObject appleInstance = GameObject.Instantiate(Apple, startPosition, Quaternion.identity) as GameObject;
        }
    }

    /// <summary>
    /// CreateWater initializes and places numApples waterInstance objects randomly in the world
    /// </summary>
    void CreateWater()
    {
        for (int i=0; i<numApples; i++){
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 0f, Random.Range(minPosition,maxPosition));
            GameObject waterInstance = GameObject.Instantiate(Water, startPosition, Quaternion.identity) as GameObject;
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
            
            // normal reproduction way: new_human = human(mother_genome, father_genome)
            // god creation way: new_human = human(read genome from file)
            // create an empty genome object for mother & father, but specifying the species

            // create the pseudo-random parent genomes
            motherGenome = new Genome();
            motherGenome.CreateGenome("human");
            fatherGenome = new Genome();
            fatherGenome.CreateGenome("human");

            string name = "Human " + i.ToString();

            // create an instance of the human class
            Animal newHuman = new Human(name, motherGenome, fatherGenome);
            humanList.Add(newHuman);
            
        }
    }

    public static Animal GetHuman(int i) {
        return humanList[i];
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



