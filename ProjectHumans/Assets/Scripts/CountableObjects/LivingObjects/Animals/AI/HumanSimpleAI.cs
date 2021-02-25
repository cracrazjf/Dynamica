using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanSimpleAI : AI
{
    Human thisHuman;
    Transform humanTransform;
    
    Dictionary<string, bool> bodyStateDict;
    Dictionary<string, float> driveStateDict;
    Dictionary<string, bool> actionStateDict;
    Dictionary<string, float> actionArgumentDict;
    Dictionary<string, float> traitDict;
    Dictionary<string, Action> goalDict;

    List<string> decidedActions;
    List<GameObject> inSight = new List<GameObject>();
    string currentGoal = "None";
    protected bool goalOngoing = false;

    float rotatedAngle = 0;

    public HumanSimpleAI(Human human, Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype traits) : base(body, drives, motor, senses, traits) {
        this.thisHuman = human;
        bodyStateDict = body.GetStateDict();
        driveStateDict = drives.GetStateDict();
        actionStateDict = motor.GetStateDict();
        actionArgumentDict = motor.GetArgDict();
        traitDict = traits.GetTraitDict();

        InitGoalDict();
    }

    public override string ChooseAction(float[ , ] visualInput, Dictionary<string, float> passedTraitDict)
    {
        this.traitDict = passedTraitDict;

        this.humanTransform = thisHuman.gameObject.transform;
        InFov(thisHuman.gameObject.transform, 45, 10);
        Debug.DrawRay(humanTransform.position, humanTransform.forward * 10, Color.red);
        
        if (currentGoal == "None") { 
            // Debug.Log("Choosing a goal ");
            decidedActions = new List<string>();
            ChooseGoal(); 
        } 
        if (currentGoal != "None") {
            Debug.Log("Current goal is: "+ currentGoal);
            goalDict[currentGoal].DynamicInvoke();
            return decidedActions[0];
        }
        return "standing up";
    }

    public void ChooseGoal() {
        string toSet = "None";
        foreach (string drive in driveStateDict.Keys) {
            string threshold = drive + "_threshold";
            if (drive == "health"){
                if (driveStateDict[drive] < traitDict[threshold]) { toSet = "Increase health"; }
            } else if (driveStateDict[drive] > traitDict[threshold]) {
                toSet = "Decrease " + drive;
            } 
        }
        currentGoal = toSet;
    }

    public void InitGoalDict() {
        goalDict = new Dictionary<string, Action>();

        goalDict.Add("Decrease thirst", DecreaseThirst);
        goalDict.Add("Decrease hunger", DecreaseHunger);
        goalDict.Add("Decrease sleepiness", DecreaseSleepiness);
        goalDict.Add("Decrease fatigue", DecreaseFatigue);
        goalDict.Add("Increase health", IncreaseHealth);
    }
  
    public void DecreaseThirst() {
        Debug.Log("Called DecreaseThirst");
        if (bodyStateDict["standing"]) {
            if (IsVisible("Water")) {
                rotatedAngle = 0;
                GameObject target = CalculateNearestObject(GetTargetObjectList("Water"));
                if (IsReachable(target)) {
                    this.thisHuman.GetMotorSystem().EndAction("taking steps");
                    decidedActions.Add("consuming");
                    currentGoal = "None";
                } else if (IsFacing(target.transform.position)) {
                    this.thisHuman.GetMotorSystem().EndAction("rotating");
                    decidedActions.Add("taking steps");
                    this.thisHuman.GetMotorSystem().SetArgs("step rate", 0.01f);
                } else {
                    FacePosition(target.transform.position);
                }
            } else {
                SearchForObjects();
            }
        }
    }

    public void DecreaseHunger() {
        Debug.Log("Called DecreaseHunger");
        if (bodyStateDict["standing"]) {
            if (this.thisHuman.GetBody().labelLH == "Food") {
                decidedActions.Add("consuming");
                this.thisHuman.GetMotorSystem().SetArgs("active hand", 0);
                currentGoal = "None";
            } else if (this.thisHuman.GetBody().labelRH == "Food") {
                decidedActions.Add("consuming");
                this.thisHuman.GetMotorSystem().SetArgs("active hand", 1);
                currentGoal = "None";
            } else if (IsVisible("Food")) {
                rotatedAngle = 0;
                GameObject target = CalculateNearestObject(GetTargetObjectList("Food"));
                if (IsReachable(target)) {
                    Vector3 targetPos = target.transform.position;
                    thisHuman.GetMotorSystem().SetArgs("hand target x", targetPos.x);
                    thisHuman.GetMotorSystem().SetArgs("hand target y", targetPos.y);
                    thisHuman.GetMotorSystem().SetArgs("hand target x", targetPos.z);
                    this.thisHuman.GetMotorSystem().EndAction("taking steps");

                    if (bodyStateDict["holding with left hand"] && bodyStateDict["holding with right hand"]) {
                        //Both hands full
                        decidedActions.Add("setting down");
                    } else if (!bodyStateDict["holding with left hand"]) {
                        //Grab with LH
                        decidedActions.Add("picking up");
                        this.thisHuman.GetMotorSystem().SetArgs("active hand", 0);
                    } else if (!bodyStateDict["holding with right hand"]) {
                        //Grab with RH
                        decidedActions.Add("picking up");
                        this.thisHuman.GetMotorSystem().SetArgs("active hand", 1);
                    }

                } else if (IsFacing(target.transform.position)) {
                    this.thisHuman.GetMotorSystem().EndAction("rotating");
                    decidedActions.Add("taking steps");
                    this.thisHuman.GetMotorSystem().SetArgs("step rate", 0.01f);
                } else { 
                    FacePosition(target.transform.position); 
                }
            } else {
                SearchForObjects();
            }
        } else if (bodyStateDict["sitting"]){
            decidedActions.Add("standing up");
        } else if (bodyStateDict["laying"]) {
            decidedActions.Add("sitting up");
        } else if (bodyStateDict["sleeping"]) {
            decidedActions.Add("waking up");
        }
    }

    public void DecreaseSleepiness() {
        Debug.Log("Called DecreaseSleepiness");
        if (bodyStateDict["laying"]) {
            decidedActions.Add("falling asleep");
        } else if (bodyStateDict["sitting"]) {
            decidedActions.Add("laying down");
        } else {
            decidedActions.Add("sitting down");
        }
    }

    public void DecreaseFatigue() {
        Debug.Log("Called DecreaseFatigue");
        if (bodyStateDict["standing"]) {
            decidedActions.Add("sitting down");
        } else if (bodyStateDict["sitting"]) {
            decidedActions.Add("laying down");
        }
    }

    public void IncreaseHealth() {
        Debug.Log("Called IncreaseHealth");
        if (bodyStateDict["standing"]) {
            decidedActions.Add("sitting down");
        } else if (bodyStateDict["sitting"]) {
            decidedActions.Add("laying down");
        }
    }

    public bool IsVisible(string targetTag) {
        if (inSight.Count > 0) {
            foreach (GameObject x in inSight) {
                if (x.tag == targetTag) {
                    return true;
                }
            }
        }
        return false;   
    }

    public List<GameObject> GetTargetObjectList(string targetTag) {
        List<GameObject> targetList = new List<GameObject>();
        foreach (GameObject x in inSight) {
            if (x.tag == targetTag) {
                targetList.Add(x);
            }
        }
        return targetList;
    }

    public GameObject CalculateNearestObject(List<GameObject> targetList) {
        if (targetList.Count == 1) {
            return targetList[0];

        } else {
            GameObject nearestObject = null;
            float largeNumber = Mathf.Infinity;
            for (int i =0; i < targetList.Count; i++) {
                float distance = Vector3.Distance(humanTransform.position, targetList[i].transform.position);

                if (distance < largeNumber) {
                    largeNumber = distance;
                    nearestObject = targetList[i];
                }
            }
            return nearestObject;
        }  
    }

    public bool IsReachable(GameObject target){
        float distance = Vector3.Distance(humanTransform.position, target.transform.position);
        if (distance < 1) {
            return true; 
        }
        return false;
    }

    public float CalculateRotationAngle(Vector3 passedPosition) {
        Vector3 targetDirection = passedPosition - humanTransform.position;
        float angle = Vector3.Angle(targetDirection, humanTransform.forward);

        return angle;
    }

    public int CalculateRelativePosition(Vector3 passedPosition) {
        Vector3 relativePosition = humanTransform.InverseTransformPoint(passedPosition);
        if (relativePosition.x < 0) {
            return -1;
        } else if (relativePosition.x > 0) { 
            return 1; 
            }
        return 0;
    }

    public bool IsFacing(Vector3 passedPosition) {
        float angle = CalculateRotationAngle(passedPosition);
        if (angle <= 0.5f) { return true; }
        return false;
    }

    public void FacePosition(Vector3 passedPosition) {
        decidedActions.Add("rotating");
        if (CalculateRelativePosition(passedPosition) == -1) {
            this.thisHuman.GetMotorSystem().SetArgs("rotation velocity", -0.05f);
        } else {
           this.thisHuman.GetMotorSystem().SetArgs("rotation velocity", 0.05f);
        }
    }

    public void SearchForObjects() {
        if (rotatedAngle < 360) {
            decidedActions.Add("rotating");
            this.thisHuman.GetMotorSystem().SetArgs("rotation velocity", 1.0f);
            rotatedAngle += 2.0f;
        }
        Vector3 randomPosition = CreateRandomPosition(3.0f);
        FacePosition(randomPosition);
        if(IsFacing(CalculateNearestObject(GetTargetObjectList("Food")).transform.position)) {
            thisHuman.GetMotorSystem().EndAction("rotating");
            decidedActions.Add("taking steps");
            this.thisHuman.GetMotorSystem().SetArgs("step rate", 0.01f);
            while ((thisHuman.gameObject.transform.position - randomPosition).magnitude < 1) {
                randomPosition = CreateRandomPosition(3.0f);
            } 
            this.thisHuman.GetMotorSystem().EndAction("taking steps");
            FacePosition(randomPosition);
        }
    }

    public Vector3 CreateRandomPosition(float Range) {
        Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(thisHuman.gameObject.transform.position.x - Range,
                                thisHuman.gameObject.transform.position.x + Range), 0,
                                UnityEngine.Random.Range(thisHuman.gameObject.transform.position.z - Range,
                                thisHuman.gameObject.transform.position.z + Range));
        return randomPosition;
    }
    
    public void InFov(Transform checkingObject, float maxAngle, float maxRadius) {
        Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        inSight.Clear();

        for (int i = 0; i < count + 1; i++) {
            if (overlaps[i] != null) {
                Vector3 directionBetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                directionBetween.y *= 0;

                float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxAngle) {
                    Ray ray = new Ray(new Vector3(checkingObject.position.x, 0.1f, checkingObject.position.z), overlaps[i].transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius)) {
                        if (hit.transform == overlaps[i].transform) {
                            if (!inSight.Contains(overlaps[i].gameObject) && overlaps[i].tag != "ground") {
                                inSight.Add(overlaps[i].gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
