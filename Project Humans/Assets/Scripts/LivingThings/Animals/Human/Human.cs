using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Animals
{
    public string activeAI;
    public HumanHCAI humanhcai;
    public NervousSystem nervousSystem;

    // access scripts
    public DriveSystem driveSystem;
    public HumanMotorSystem humanMotorSystem;
    public FOVDetection fovdetection;   // this would be in HumanHCAIpublic NervousSystem nervousSystem;

    //START JULIA CODE

    // fixed traits
    /// <value>Baseline movement speed of Humans</value>
    public float movementSpeed = 1.0f;
    /// <value>Initial sex of Humans</value>
    public string sex = "None";

    // goals and tasks
    /// <value>Maintained as the current objective of Human object</value>
    public string currentGoal;
    /// <value>Maintained as the current action of the Human object</value>
    public string currentTask;

    // END DEC OF JULIA PARAMS

    /// <summary>
    /// Human constructor - Julia made
    /// </summary>
    public Human(string name, string passed_sex) {
        super(name);

        this.sex = passed_sex;

        // constructors for all necessary functionale
        this.driveSystem = new DriveSystem();
        this.hhumanhcai = new HHumanHCAI();
        this.nervousSystem = new NervousSystem();
        this.fovdetection = new FOVDetection();
        this.humanMotorSystem = new HumanMotorSystem();

        activeAI = "HumanHCAI";
    }

    public void TakeAction()
    {
        for (int i = 0; i < numActions; i++)
        {
            if (this.humanhcai.actionValueList[i] != 0)
            {
                this.humanMotorSystem.ActionFunctionList[i]();
            }
        }
    }

    void Start()
    {
        //figure out whether phenotype is a new object or declared in constructor etc... ask Jon
    }

    /// <summary>
    /// Update is called once per frame and calls ChooseAction to direct Human object
    /// </summary>
    void Update()
    {
        yield return new WaitForSeconds(1); 

        this.nervousSystem.GetInput();  // this gets the 32x32x3 pixel map of the agent's camera
        this.humanhcai.ChooseAction();
        //neuralnetworkai.ChooseAction();
        this.humanMotorSystem.TakeAction();
        this.driveSystem.UpdateStates();

    }
}



    