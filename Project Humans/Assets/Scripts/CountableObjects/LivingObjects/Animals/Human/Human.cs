﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// test change

// Missing a namespace;

public class Human : Animal
{
    public int updateCounter = 0;

    public HumanSimpleAI2 humanSimpleAI2;
    public HumanRNNAI humanRNNAI;

    public string activeAI = "humanTestAI";

    public List<float> actionValueList = new List<float>();

    /// <value>Human prefab</value>
    public GameObject humanPrefab;
    public Camera visualInputCamera;

    public Transform leftEye;
    public Transform rightEye;

    public Transform leftHand;
    public Transform rightHand;
    public Rigidbody rigidbody;

    public bool doingNothing = true;

    public string bodyState;
    public string actionState;
    public bool sleepingState;
    public bool LHState;
    public bool RHState;

    /// <summary>
    /// Human constructor
    /// </summary>
    public Human(int index, Genome motherGenome, Genome fatherGenome): base("Human", index, 1.0f, 1.0f, motherGenome, fatherGenome) {
        bodyState = "standing";
        actionState = "none";
        sleepingState = false;
        LHState = false;
        RHState = false;

        //Instantiate humanPrefab
        Vector3 startPosition = this.chooseStartPosition();
        Quaternion startRotation = this.chooseStartRotation();

        if (this.phenotype.traitDict["sex"] == "0")
        {   
            humanPrefab = Resources.Load("HumanMalePrefab",typeof(GameObject)) as GameObject;
            this.gameObject = GameObject.Instantiate(humanPrefab, startPosition, startRotation) as GameObject;
            this.gameObject.name = GetName();

            gameObject.SetActive(true);
        } else {
            humanPrefab = Resources.Load("HumanFemalePrefab",typeof(GameObject)) as GameObject;
            this.gameObject = GameObject.Instantiate(humanPrefab, startPosition, startRotation) as GameObject;
            this.gameObject.name = GetName();

            gameObject.SetActive(true);
        }

        this.gameObject.AddComponent<FOVDetection>();
        this.gameObject.AddComponent<HumanMonobehaviour>();
        animator = this.gameObject.GetComponent<Animator>();
        visualInputCamera = this.gameObject.GetComponentInChildren<Camera>();

        SetMotorSystem(new HumanMotorSystem(this));
        SetActionChoice(new ActionChoice(GetMotorSystem().actionLabelList));
        SetNervousSystem(new HumanNervousSystem(this));

        if (activeAI == "humanRNNAI"){
            humanRNNAI = new HumanRNNAI();
        }
        else{
            humanSimpleAI2 = new HumanSimpleAI2(this);
        }
        
        

        leftEye = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2);
        rightEye = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(3);

        leftHand = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(5);
        rightHand = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5);

        SetFOVDetection(this.gameObject.GetComponent<FOVDetection>());
        rigidbody = this.gameObject.GetComponent<Rigidbody>();

        SetActionChoice(new ActionChoice(this.GetMotorSystem().actionLabelList));
    }
    
     public void updateStates() {
        // double check that all possible values of this.animator.GetCurrentAnimatorStateInfo(0).IsName that are not "Laying Loop" or "Sit Loop" are standing
        //Debug.Log(this.actionState);
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Laying Loop")) {
            this.bodyState = "laying";
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Sit Loop")) {
            this.bodyState = "sitting";
        }
        else {
            this.bodyState = "standing";
        }
        
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Rotate")) {
            this.actionState = "rotating";
            if (GetActionChoice().actionValueDict["rotate"] == 0) {
                this.animator.SetBool("rotate", false);
            }
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Sit down")) {
            this.actionState = "sitting down";
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Sit up")) {
            this.actionState = "sitting up";
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Lay down")) {
            this.actionState = "laying down";
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Taking steps")) {
            this.actionState = "taking steps";
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Drink")) {
            this.actionState = "drinking";
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Eat")) {
            if (this.animator.GetFloat("EatL/R") == 0) {
                this.actionState = "eating with left hand";
            }
            if (this.animator.GetFloat("EatL/R") == 1) {
                this.actionState = "eating with right hand";
            }
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("PickUp")) {
            if (this.animator.GetFloat("PickupL/R") == 0) {
                this.actionState = "picking up with left hand";
            }
            if (this.animator.GetFloat("PickupL/R") == 1) {
                this.actionState = "picking up with right hand";
            }
        }
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("SetDown")) {
            if (this.animator.GetFloat("SetdownL/R") == 0) {
                this.actionState = "setting down with left hand";
            }
            if (this.animator.GetFloat("SetdownL/R") == 1) {
                this.actionState = "setting down with right hand";
            }
        }
        else {
            this.actionState = "none";
        }
    }
    public void TestUpdate() {
        // this.gameObject.transform.LookAt(new Vector3(x,0,z));
        // this.humanMotorSystem.accellerate(2,new Vector3(x,0,z));
        GetMotorSystem().rotate(1);
    }

    public override void UpdateAnimal(){

        updateStates();
        float[ , ] visualInput = GetNervousSystem().GetVisualInput();
        float[] bodyState = GetNervousSystem().GetBodyState();
        float[] driveState = GetDriveSystem().GetDriveStatesArray();

        GetFOVDetection().inFov(this.gameObject.transform, 45,10);

        GetActionChoice().initActionChoices(GetMotorSystem().actionLabelList);
        if (activeAI == "humanRNNAI"){
            SetActionChoice(humanRNNAI.chooseAction(visualInput, bodyState, driveState, GetActionChoice()));
            GetMotorSystem().takeAction(GetActionChoice());
        }
        else {
            //humanTestAI = new HumanTestAI(this);
            SetActionChoice(humanSimpleAI2.chooseAction());
            GetMotorSystem().takeAction(GetActionChoice());
        }
        
        GetDriveSystem().UpdateDrives();
        updateCounter++;
        if (updateCounter == 100){
            updateCounter = 0;
        }
    }
}





    





    






    