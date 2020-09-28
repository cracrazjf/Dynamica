using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class World : MonoBehaviour
{
    /// <value>Gets the value of PI.</value>
    public GameObject Ground;
    /// <value>Initial value for size of world</value>
    public static float worldSize = 20.0f;

    /// <value>Used for bounding minimum camera position</value>
    public float minPosition = -1 * (World.worldSize) / 2;
    /// <value>Used for bounding maximum camera position</value>
    public float maxPosition = World.worldSize / 2;

    /// <value>Define a HumanMale in world</value>

    public GameObject HumanMale; //= new Human("Name","Male");// it was new Human("Male") but in human constructor, we need both name and sex as parameters;
    /// <value>Define a HumanFemale in world</value>
    public GameObject HumanFemale; //= new Human("Name","Femal");
    /// <value>Number of HumanMale objects to initialize</value>
    public int numHumanMales = 2;
    /// <value>Number of HumanFemale objects to initialize</value>
    public int numHumanFemales = 2;
    /// <value>List of all Human objects</value>
    public List<GameObject> humanList = new List<GameObject>();

    /// <value>Define an Apple in world</value>
    public GameObject Apple;
    /// <value>Number of Apple objects to initialize</value>
    public int numApples = 100;

    /// <value>Define a waterInstance in world</value>
    public GameObject Water;
    /// <value>Number of Water objects to initialize</value>
    public int numWater = 100;



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
        for (int i = 0; i < numApples; i++)
        {
            var startPosition = new Vector3(50 + Random.Range(minPosition, maxPosition), 0.21f, 50 + Random.Range(minPosition, maxPosition));
            GameObject appleInstance = GameObject.Instantiate(Apple, startPosition, Quaternion.identity) as GameObject;
        }
    }

    // / <summary>
    // / CreateWater initializes and places numApples waterInstance objects randomly in the world
    // / </summary>
    void CreateWater()
    {
        for (int i = 0; i < numApples; i++)
        {
            var startPosition = new Vector3(-50 + Random.Range(minPosition, maxPosition), 0.21f, -50 + Random.Range(minPosition, maxPosition));
            GameObject waterInstance = GameObject.Instantiate(Water, startPosition, Quaternion.identity) as GameObject;
        }
    }

    // / <summary>
    // / CreateHumans initializes and places numHumanMales and numHumanFemales HumanMale and HumanFemale objects randomly in the world
    // / </summary>
    void CreateHumans()
    {
        for (int i = 0; i < numHumanMales; i++)
        {
            var startPosition = new Vector3(Random.Range(minPosition, maxPosition), 0.03f, Random.Range(minPosition, maxPosition));

            // normal reproduction way: new_human = human(mother_genome, father_genome)
            // god creation way: new_human = human(read genome from file)
            // create an empty genome object for mother & father, but specifying the species

            // create the pseudo-random parent genomes
            motherGenome = new Genome;
            motherGenome.createGenome("human")
            fatherGenome = new Genome;
            fatherGenome.createGenome("human")

            // create an instance of the human class
            newHuman = new Human(motherGenome, fatherGenome)

            // does this line need to be in human
            GameObject humanInstance = Instantiate(Human, startPosition, Quaternion.identity) as GameObject;
            humanList.Add(humanMaleInstance);
            humanList[i].AddComponent<Human>();
            humanList[i].GetComponent<Human>().sex = "male";
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

    }
}