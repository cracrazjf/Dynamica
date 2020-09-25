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
    public Human HumanMale = new Human("Male");
    /// <value>Define a HumanFemale in world</value>
    public Human HumanFemale = new Human("Female");
    /// <value>Number of HumanMale objects to initialize</value>
    public int numHumanMales = 2;
    /// <value>Number of HumanFemale objects to initialize</value>
    public int numHumanFemales = 2;
     /// <value>List of all Human objects</value>
    public List<Human> humanList = new List<Human>();

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
        for (int i=0; i<numApples; i++){
            var startPosition = new Vector3 (50+Random.Range(minPosition,maxPosition), 0.21f, 50+Random.Range(minPosition,maxPosition));
            GameObject appleInstance = GameObject.Instantiate(Apple, startPosition, Quaternion.identity) as GameObject;
        }
    }

    /// <summary>
    /// CreateWater initializes and places numApples waterInstance objects randomly in the world
    /// </summary>
    void CreateWater()
    {
        for (int i=0; i<numApples; i++){
            var startPosition = new Vector3 (-50+Random.Range(minPosition,maxPosition), 0.21f, -50+Random.Range(minPosition,maxPosition));
            GameObject waterInstance = GameObject.Instantiate(Water, startPosition, Quaternion.identity) as GameObject;
        }
    }

    /// <summary>
    /// CreateHumans initializes and places numHumanMales and numHumanFemales HumanMale and HumanFemale objects randomly in the world
    /// </summary>
    void CreateHumans()
    {
        for (int i=0; i<numHumanMales; i++){
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 3, Random.Range(minPosition,maxPosition));
            
            Human humanMaleInstance = Instantiate(HumanMale, startPosition, Quaternion.identity) as Human;
            humanList.Add(humanMaleInstance);
        }
        for (int i=0; i<numHumanFemales; i++){
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 3, Random.Range(minPosition,maxPosition));
            
            Human humanFemaleInstance = Instantiate(HumanFemale, startPosition, Quaternion.identity) as Human;
            humanList.Add(humanFemaleInstance);
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        
    }
}

/* public class World : MonoBehaviour
{
    public GameObject HumanInfo;
    public GameObject Ground;
    public static float worldSize = 20.0f;

    public float minPosition = -1 * (World.worldSize) / 2;
    public float maxPosition = World.worldSize / 2;
    
    public GameObject HumanMale;
    public GameObject HumanFemale;
    public int numHumanMales = 2;
    public int numHumanFemales = 2;
    public List<GameObject> humanList = new List<GameObject>();

    public GameObject Apple;
    public int numApples = 100;

    public GameObject Water;
    public int numWater = 100;

    // Start is called before the first frame update
    void Start()
    {
        CreateTerrain();
        CreateHumans();
        CreateApples();
        CreateWater();
        //CreateHumanInfo();
    }
    
    void CreateTerrain()
    {
        CreateGround();
    }

    void CreateGround()
    {
        GameObject groundInstance = GameObject.Instantiate(Ground) as GameObject;
        var groundScript = groundInstance.GetComponent<Ground>();
        groundScript.Init(worldSize);
    }

    void CreateApples()
    {
        for (int i=0; i<numApples; i++){
            var startPosition = new Vector3 (50+Random.Range(minPosition,maxPosition), 0.21f, 50+Random.Range(minPosition,maxPosition));
            GameObject appleInstance = GameObject.Instantiate(Apple, startPosition, Quaternion.identity) as GameObject;
        }
    }

    void CreateWater()
    {
        for (int i=0; i<numWater; i++){
            var startPosition = new Vector3 (-50+Random.Range(minPosition,maxPosition), 0.21f, -50+Random.Range(minPosition,maxPosition));
            GameObject waterInstance = GameObject.Instantiate(Water, startPosition, Quaternion.identity) as GameObject;
        }
    }

    void CreateHumans()
    {
        for (int i=0; i<numHumanMales; i++){
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 0, Random.Range(minPosition,maxPosition));
            GameObject humanMaleInstance = GameObject.Instantiate(HumanMale, startPosition, Quaternion.identity) as GameObject;
            // how do we set humanInstance.sex = "Male";
            humanList.Add(humanMaleInstance);
        }
        for (int i=0; i<numHumanFemales; i++){
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 3, Random.Range(minPosition,maxPosition));
            GameObject humanFemaleInstance = GameObject.Instantiate(HumanFemale, startPosition, Quaternion.identity) as GameObject;
            // how do we set humanInstance.sex = "Female";
            humanList.Add(humanFemaleInstance);
        }
    }
    void CreateHumanInfo()
    {
        GameObject HumanInfoInstance = GameObject.Instantiate(HumanInfo) as GameObject;
        HumanInfoInstance.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
        HumanInfoInstance.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-260, 0, 0);
        
    }

} */
