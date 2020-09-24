using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class World : MonoBehaviour
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
            var startPosition = new Vector3 (Random.Range(minPosition,maxPosition), 3, Random.Range(minPosition,maxPosition));
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
