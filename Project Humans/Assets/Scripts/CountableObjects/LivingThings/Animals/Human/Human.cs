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
    public HumanNervousSystem humanNervousSystem;
    public List<float> actionValueList = new List<float>();

    /// <value>Human prefab</value>
    public GameObject humanPrefab;
    private string named;

    public Transform leftEye;
    public Transform rightEye;

    public Transform leftHand;
    public Transform rightHand;
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    
    public GameObject TestObj;

    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(string objectType, Genome motherGenome, Genome fatherGenome): base(objectType, motherGenome, fatherGenome) {

        this.humanMotorSystem = new HumanMotorSystem(this);
        this.humanActionChoice = new HumanActionChoice(this.humanMotorSystem.actionLabelList);
        this.humanNervousSystem = new HumanNervousSystem(this);
        
        TestObj = GameObject.Find("TestObj");
        
        //Instantiate humanPrefab
        Vector3 startPosition = this.chooseStartPosition();
        Quaternion startRotation = this.chooseStartRotation();

        if (this.phenotype.traitDict["sex"] == "0")
        {
            humanPrefab = Resources.Load("HumanMalePrefab",typeof(GameObject)) as GameObject;
            this.gameObject = GameObject.Instantiate(humanPrefab, startPosition, startRotation) as GameObject;// instantiate
            this.gameObject.name = named; 

            gameObject.SetActive(true);
        }
        if (this.phenotype.traitDict["sex"] == "1")
        {
            humanPrefab = Resources.Load("HumanFemalePrefab",typeof(GameObject)) as GameObject;
            this.gameObject = GameObject.Instantiate(humanPrefab, startPosition, startRotation) as GameObject;// instantiate 
            this.gameObject.name = named;

            gameObject.SetActive(true);
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

        leftEye = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2);
        rightEye = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(3);

        leftHand = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(5);
        rightHand = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5);

    }
    
    public void TestUpdate() {
        // this.gameObject.transform.LookAt(new Vector3(x,0,z));
        // this.humanMotorSystem.accellerate(2,new Vector3(x,0,z));
        this.humanMotorSystem.rotate(1);
    }

    public void UpdateAnimal(){

        float[ , , ] visualInput = this.humanNervousSystem.GetVisualInput();
        fOVDetection.inFov(this.gameObject.transform, 45,10);

        this.humanActionChoice = humanSimpleAI3.chooseAction(visualInput);
        this.humanMotorSystem.takeAction(this.humanActionChoice);
        this.driveSystem.UpdateDrives();
    }
}





    





    






    