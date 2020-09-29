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
    public int numApples = 100;
    public int numWater = 100;
    public int numHumans = 5;

    /// <value>Creating object lists</value>
    public List<Human> humanList = new List<Human>();

    /// <value>Setting initial world properties</value>
    public static float worldSize = 20.0f;
    public float maxPosition = World.worldSize / 2;
    public float minPosition = -World.worldSize / 2;

    /// <summary>
    /// Start is called before the first frame update and initializes all scene objects
    /// </summary>
    void Start()
    {
        CreateTerrain();
        CreateHumans();
        CreateApples();
        CreateWater();
    }

    /// <summary>
    /// CreateTerrain is redundant and serves to call CreateGround
    /// </summary>
    void CreateTerrain()
    {
        CreateGround();
    }

    /// <summary>
    /// CreateGround initializes the Ground object
    /// </summary>
    void CreateGround()
    {
        GameObject groundInstance = GameObject.Instantiate(Ground) as GameObject;
        var groundScript = groundInstance.GetComponent<Ground>();
        groundScript.Init(worldSize);
    }

    /// <summary>
    /// CreateApples initializes and places numApples appleInstance objectsFs randomly in the world
    /// </summary>
    void CreateApples()
    {
        for (int i=0; i<numApples; i++){
            var startPosition = new Vector3 (50 + Random.Range(minPosition,maxPosition), 0.21f, 50+Random.Range(minPosition,maxPosition));
            GameObject appleInstance = GameObject.Instantiate(Apple, startPosition, Quaternion.identity) as GameObject;
        }
    }

    // / <summary>
    // / CreateWater initializes and places numApples waterInstance objects randomly in the world
    // / </summary>
    void CreateWater()
    {
        for (int i=0; i<numApples; i++){
            var startPosition = new Vector3 (-50+Random.Range(minPosition,maxPosition), 0.21f, -50+Random.Range(minPosition,maxPosition));
            GameObject waterInstance = GameObject.Instantiate(Water, startPosition, Quaternion.identity) as GameObject;
        }
    }

    // / <summary>
    // / CreateHumans initializes and places numHumanMales and numHumanFemales HumanMale and HumanFemale objects randomly in the world
    // / </summary>
    void CreateHumans()
    {
        for (int i=0; i<numHumans; i++){
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 0.03f, Random.Range(minPosition,maxPosition));
            
            // normal reproduction way: new_human = human(mother_genome, father_genome)
            // god creation way: new_human = human(read genome from file)
            // create an empty genome object for mother & father, but specifying the species

            // create the pseudo-random parent genomes
            Genome motherGenome = new Genome();
            motherGenome.createGenome("human");
            Genome fatherGenome = new Genome();
            fatherGenome.createGenome("human");

            // create an instance of the human class
            Human newHuman = new Human(motherGenome, fatherGenome);
            humanList.Add(newHuman);
            GameObject humanInstance = Instantiate(HumanMale, startPosition, Quaternion.identity) as GameObject;
        
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }
}

