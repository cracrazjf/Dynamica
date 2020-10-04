using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Missing a namespace;

public class Human : Animal 
{
    public HumanTestAI humanTestAI;
    public HumanGPSAI humanGPSAI;
    public HumanRNNAI humanRNNAI;
    public string activeAI = "humanTestAI";

    public HumanMotorSystem humanMotorSystem;
    public List<float> actionValueList = new List<float>();

    /// <value>Human prefab</value>
    public GameObject humanPrefab;


    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(string objectType, Genome motherGenome, Genome fatherGenome): base(objectType, motherGenome, fatherGenome) {

        this.humanMotorSystem = new HumanMotorSystem(this);

        //Instantiate humanPrefab
        Vector3 startPosition = this.chooseStartPosition();

        // use a debug statment
        Debug.Log(this.phenotype.traitDict["sex"]);
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

        if (activeAI == "humanGPSAI"){
            humanGPSAI = new HumanGPSAI(this);
        }
        else if (activeAI == "humanRNNAI"){
            humanRNNAI = new HumanRNNAI(this);
        }
        else{
            humanTestAI = new HumanTestAI(this);
        }
    }

    public void updateHuman(){
        if (activeAI == "humanGPSAI"){
            actionValueList = humanGPSAI.chooseAction();
        }
        else if (activeAI == "humanRNNAI"){
            actionValueList = humanRNNAI.chooseAction();
        }
        else{
            actionValueList = humanTestAI.chooseAction();
        }

        this.humanMotorSystem.takeAction(actionValueList);
    }
}





    





    






    