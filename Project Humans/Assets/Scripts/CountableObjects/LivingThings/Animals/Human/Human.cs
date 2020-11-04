using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Missing a namespace;

public class Human : Animal
{
    public HumanTestAI humanTestAI;
    public HumanSimpleAI3 humanSimpleAI3;
    public HumanActionChoice humanActionChoice;
    public HumanGPSAI humanGPSAI;
    public HumanRNNAI humanRNNAI;
    public FOVDetection fOVDetection;
  
    public string activeAI = "humanTestAI";

    public HumanMotorSystem humanMotorSystem;
    public List<float> actionValueList = new List<float>();

    /// <value>Human prefab</value>
    public GameObject humanPrefab;

    public Transform Eye_L;
    public Transform Eye_R;

    public Transform Hand_L;
    public Transform Hand_R;
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    

    public GameObject TestObj;
    
    public float x;
    public float z;


    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(string objectType, Genome motherGenome, Genome fatherGenome): base(objectType, motherGenome, fatherGenome) {

        this.humanMotorSystem = new HumanMotorSystem(this);
        this.humanActionChoice = new HumanActionChoice(this.humanMotorSystem.actionLabelList);
        
        TestObj = GameObject.Find("TestObj");
        
       
        //Instantiate humanPrefab
        Vector3 startPosition = this.chooseStartPosition();

        // use a debug statment
        //Debug.Log(this.phenotype.traitDict["sex"]);
        if (this.phenotype.traitDict["sex"] == "0")
        {
            humanPrefab = Resources.Load("HumanMalePrefab",typeof(GameObject)) as GameObject;
            this.gameObject = GameObject.Instantiate(humanPrefab, startPosition, Quaternion.identity) as GameObject;// instantiate 

        }
        if (this.phenotype.traitDict["sex"] == "1")
        {
            humanPrefab = Resources.Load("HumanFemalePrefab",typeof(GameObject)) as GameObject;
            this.gameObject = GameObject.Instantiate(humanPrefab, startPosition, Quaternion.identity) as GameObject;// instantiate 

        }
        this.gameObject.AddComponent<FOVDetection>();
        animator = this.gameObject.GetComponent<Animator>();

        if (activeAI == "humanGPSAI"){
            humanGPSAI = new HumanGPSAI(this);
        }
        else if (activeAI == "humanRNNAI"){
            humanRNNAI = new HumanRNNAI(this);
        }
        else{
            //humanTestAI = new HumanTestAI(this);
            humanSimpleAI3 = new HumanSimpleAI3(this);
        }
        
        
        fOVDetection = this.gameObject.GetComponent<FOVDetection>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();

        Eye_L = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2);
        Eye_R = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(3);

        Hand_L = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(5);
        Hand_R = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5);


        var direction = (TestObj.transform.position - this.gameObject.transform.position);
        var distance = Vector3.Distance(this.gameObject.transform.position, TestObj.transform.position);
        var magnitude = (this.gameObject.transform.position - TestObj.transform.position).magnitude;
        z  = TestObj.transform.position.z - (1/distance) * Mathf.Abs(direction.z);
        x =  TestObj.transform.position.x - (1/distance) * Mathf.Abs(direction.x);

    }
    
    public void TestUpdate() {
        // this.gameObject.transform.LookAt(new Vector3(x,0,z));
        // this.humanMotorSystem.accellerate(2,new Vector3(x,0,z));
        this.humanMotorSystem.rotate(1);
        
    }

    public void updateHuman(){

        float[ , , ] visualInput = this.nervousSystem.GetVisualInput();
        fOVDetection.inFov(this.gameObject.transform, 45,10);
        this.humanActionChoice = humanSimpleAI3.chooseAction(visualInput);
        this.humanMotorSystem.takeAction(this.humanActionChoice);
        this.driveSystem.UpdateDrives();

        
    }
    
}





    





    






    