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
    private static SensorySystem sensorySystem;
    private bool finishedUpdate = true;
    protected AI activeAI;
    protected string action;

    public static List<int> timeList = new List<int>();
    public static List<string> timeEventList = new List<string>();

    public Animal(string objectType, int index, Genome motherGenome, Genome fatherGenome, Vector3 spawn) 
    : base (objectType, index, motherGenome, fatherGenome, spawn) {
            
        animalBody = new PrimateBody(this, spawn);
        body = animalBody;
        motorSystem = new PrimateMotorSystem(this);
            
        visualInputCamera = animalBody.GetGameObject().GetComponentInChildren<Camera>();
        
        driveSystem = new DriveSystem(this);
        sensorySystem = new SensorySystem(this);

    //activeAI = new SimpleAI(this, GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype());
    //activeAI = new TestAI(this, GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype());
    activeAI = new NeuralAI(this, GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype());
    }
    

    public override void UpdateEntity() {
        //Debug.Log("Updating an animal");
        if (finishedUpdate) {
            // get the system time here
            ResetEventTimes();
            AddEventTime("Start Update Entity");

            finishedUpdate = false;
            this.GetDriveSystem().UpdateDrives();
            Matrix<float> visualInputMatrix = GetSensorySystem().GetVisualInput();
            AddEventTime("Get Sensory System");
            AddEventTime("Finished Choose Action");
            // get the system time again here, subtract
            Vector<float> temp = activeAI.ChooseAction().Column(0);
            this.GetMotorSystem().TakeAction(temp);
            //Debug.Log(activeAI.ChooseAction(visualInputMatrix)[0,0].GetType());
            action = "In progress!";

            IncreaseAge(1);
            //Debug.Log("Got through a loop");
            finishedUpdate = true;

            //PrintEventTimes();
        }
    }

    public void ToggleBodyPart(string part, bool toggle) {
        this.GetBody().GetSkeletonDict()[part].gameObject.SetActive(toggle);
    }
    
    // getters and setters for body, drive system, motor system, sensory system, and action choice class
    public new AnimalBody  GetBody() { return animalBody; }

    public bool GetBodyState(string state) { return (this.GetBody().GetStateDict()[state] == 1f); }

    public DriveSystem GetDriveSystem() { return driveSystem; }

    public MotorSystem GetMotorSystem() { return motorSystem; }

    public SensorySystem GetSensorySystem() { return sensorySystem; }

    public string GetAction() { return this.activeAI.GetAction(); }

    public string GetSex() { 
        if(this.GetPhenotype().GetTrait("sex") == 1.0) {
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