
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Missing a namespace;

public class Human : Animal 
{
    /// <value>holding reference to classes</value>
    public Human human;
    public HumanHCAI humanhcai;
    public NervousSystem nervousSystem;
    public HumanMotorSystem humanMotorSystem;
    public FOVDetection fovdetection;   

    /// <value>AI type</value>
    public string activeAI;

    /// <value>Human prefab</value>
    public GameObject humanPrefab;

    /// <value>goals and tasks</value>
    public string currentGoal; 
    public string currentTask;
    public int numActions;


   

    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(Genome motherGenome, Genome fatherGenome): base(motherGenome, fatherGenome) {

        // constructors for all necessary functionale
        
        // I have to add this since the constructor in drive system asked me to pass in a parameter;
        this.humanhcai = new HumanHCAI(this); //I have to add this since the constructor in humanHcai asked me to pass in a parameter;
        this.nervousSystem = new NervousSystem();
        this.fovdetection = new FOVDetection();
        this.humanMotorSystem = new HumanMotorSystem();
        //humanPrefab = Resources.Load()
        var startPosition = new Vector3 (Random.Range(World.minPosition,World.maxPosition), 0.03f, Random.Range(World.minPosition,World.maxPosition));
        this.gameObject = GameObject.Instantiate(humanPrefab, startPosition, Quaternion.identity) as GameObject;// instantiate 

    }

    public void TakeAction()
    {
        for (int i = 0; i < numActions; i++)
        {
            if (this.humanhcai.actionValueList[i] != 0)
            {
                //this.humanMotorSystem.ActionFunctionList[i]();
            }
        }
    }

    public new void Start()
    {
        //figure out whether phenotype is a new object or declared in constructor etc... ask Jon
    }

    /// <summary>
    /// Update is called once per frame and calls ChooseAction to direct Human object
    /// </summary>
    public new void Update()
    {
        //yield return new WaitForSeconds(1); 

        //this.nervousSystem.GetInput();  // this gets the 32x32x3 pixel map of the agent's camera
        //this.humanhcai.ChooseAction();
        //neuralnetworkai.ChooseAction();
        //this.humanMotorSystem.TakeAction();
        //this.driveSystem.UpdateDrives();
        //Debug.Log("worked");

    }
    //load correct prefab when the game start and be able to change it during runtime.
    // public void loadHumanPrefab() {
    //     int sex = this.Phenotype.traitValueDict['sex'];
   
    //     if(sex == 0) 
    //         {
    //             humanPrefab = Resources.Load("humanMaleAdult", typeof(GameObject)) as GameObject;
    //         } 
    //     } else if (sex == 1) {

    //             humanPrefab = Resources.Load("humanFemaleAdult", typeof(GameObject)) as GameObject;
    
    //         } 
    //     }
    // }
}





    