using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra;

public class Animal : Entity {

    public Camera visualInputCamera;
    public int cheatCommand;
    public float cheatArgs;
    public bool noCheats = true;

    private static AnimalBody animalBody;
    private static DriveSystem driveSystem;
    private static MotorSystem motorSystem;
    public static void SetMotorSystem(MotorSystem motor) { motorSystem = motor; }
    private static SensorySystem sensorySystem;
    private bool finishedUpdate = true;
    protected AI activeAI;
    protected string action;

    public static List<int> timeList = new List<int>();
    public static List<string> timeEventList = new List<string>();

    public Animal(string objectType, int index, Genome motherGenome, Genome fatherGenome, Vector3 spawn) 
    : base (objectType, index, motherGenome, fatherGenome, spawn, true) {

        InitBodyControl(spawn);
        visualInputCamera = animalBody.GetGameObject().GetComponentInChildren<Camera>();
        
        driveSystem = new DriveSystem(this);
        sensorySystem = new SensorySystem(this);

        InitBrain();
    }

    void InitBodyControl(Vector3 spawn) {
        animalBody = World.GetAnimalBody(this, spawn, species);
        motorSystem = World.GetMotor(this, species);
        body = animalBody;
    }
    
    void InitBrain() {
        if (World.humanAIParam == 0) {
            activeAI = new SimpleAI(this, GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype());
        } else { activeAI = new NeuralAI(this, GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype()); }
    }

    public override void UpdateEntity() {
        if (finishedUpdate) {

            finishedUpdate = false;
            GetDriveSystem().UpdateDrives();
            Matrix<float> visualInputMatrix = GetSensorySystem().GetVisualInput();
            
            Vector<float> temp = activeAI.ChooseAction().Column(0);
            GetMotorSystem().TakeAction(temp);
            //Debug.Log(activeAI.ChooseAction(visualInputMatrix)[0,0].GetType());
            action = "In progress!";

            IncreaseAge(1);

            finishedUpdate = true;
        }
    }

    public void ToggleBodyPart(string part, bool toggle) {
        GetBody().GetSkeletonDict()[part].gameObject.SetActive(toggle);
    }
    
    // getters and setters for body, drive system, motor system, sensory system, and action choice class
    public new AnimalBody  GetBody() { return animalBody; }

    public bool GetBodyState(string state) { return (GetBody().GetStateDict()[state] == 1f); }

    public DriveSystem GetDriveSystem() { return driveSystem; }

    public MotorSystem GetMotorSystem() { return motorSystem; }

    public SensorySystem GetSensorySystem() { return sensorySystem; }

    public AI GetAI() { return activeAI; }

    public string GetAction() { return activeAI.GetAction(); }

    public string GetSex() { 
        if(GetPhenotype().GetTrait("sex") == 1.0) {
            return "Male";
        } else { return "Female"; }
    }

    public void SetCommand(float command, float param) {
        //Debug.Log("Passed command B " + command + " with parameter of " + param);
        cheatCommand = (int) command;
        cheatArgs = param;
        noCheats = false;
    }


    public static void ResetEventTimes(){
        timeList.Clear();
        timeEventList.Clear();
    }
    
    public static void AddEventTime(string eventName) {
        int time = DateTime.Now.Millisecond + 1000*DateTime.Now.Second;

        timeList.Add(time);
        timeEventList.Add(eventName);
        //Debug.Log("Adding Event Time " + eventName + " " + timeList.Count + " " + timeEventList.Count);
    }

    public static void PrintEventTimes(){
        int numEvents = timeList.Count;
        
        if (numEvents > 3){
            Debug.Log(numEvents);
            string outputString = "";
            for (int i = 0; i < numEvents-1; i++){
                int timeSpan = timeList[i+1] - timeList[i];
                outputString += timeEventList[i+1] + ": " + timeSpan.ToString() + System.Environment.NewLine;
            }
            Debug.Log(outputString);
        }
    }
}