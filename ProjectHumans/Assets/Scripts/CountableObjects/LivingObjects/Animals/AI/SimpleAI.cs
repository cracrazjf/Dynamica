using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimpleAI : AI {
    Animal thisAnimal;
    Transform animalTransform;

    bool[] actionStates;
    List<string> actionStateLabelList;

    float[] actionArguments;
    List<string> actionArgumentLabelList;

    Dictionary<string, bool> bodyStateDict;
    Dictionary<string, float> driveStateDict;
    Dictionary<string, float> traitDict;
    Dictionary<string, Action> goalDict;

    int[] decidedActions;
    List<GameObject> inSight;
    string currentGoal = "None";

    public SimpleAI(Animal animal, Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype traits) : base(body, drives, motor, senses, traits) {
        thisAnimal = animal;
        bodyStateDict = body.GetStateDict();

        actionStates = motor.GetStates();
        actionStateLabelList = motor.GetStateLabels();

        actionArguments = motor.GetArgs();
        actionStateLabelList = motor.GetArgLabels();

        driveStateDict = drives.GetStateDict();
        traitDict = traits.GetTraitDict();

        InitGoalDict();
    }

    public override int[] ChooseAction(float[ , ] visualInput) {
        decidedActions = new int[actionStates.Length];

        animalTransform = thisAnimal.gameObject.transform;
        UpdateFOV(animalTransform, 45, 10);

        Debug.DrawRay(animalTransform.position, animalTransform.forward * 10, Color.yellow);
        
        ChooseGoal();
        Debug.Log("Current goal is: " + currentGoal);
        goalDict[currentGoal].DynamicInvoke();
        
        return decidedActions;
    }

    public void ChooseGoal() {
        string toSet = "None";
        foreach (string drive in driveStateDict.Keys) {
            string threshold = drive + "_threshold";
            if (drive == "health"){
                // if (driveStateDict[drive] < traitDict[threshold]) { toSet = "Increase health"; }
            } else if (driveStateDict[drive] > traitDict[threshold]) {
                toSet = "Decrease " + drive;
            } 
        }
        currentGoal = toSet;
    }

    public void InitGoalDict() {
        goalDict = new Dictionary<string, Action>();
        goalDict.Add("Decrease thirst", Consume);
        goalDict.Add("Decrease hunger", Consume);
        goalDict.Add("Decrease sleepiness", DecreaseSleepiness);
        goalDict.Add("Decrease fatigue", DecreaseFatigue);
        goalDict.Add("Increase health", IncreaseHealth);
    }

    public void Consume() {
        // Check for both hands (or positions of potential holding)
        List<GameObject> heldItems = thisAnimal.GetBody().GetHoldings();
        for (int i = 0; i < heldItems.Count; i++) {
            if (checkEdible(heldItems[i])) {
                decidedActions[9] = 1;
                this.thisAnimal.GetMotorSystem().SetArgs("held position", i);
            }
        }

        if (decidedActions[9] != 1) {
            AcquireObject("Object");
        } else { SearchForObjects("Object"); }
    }

    // Seeks out an object of the passed tag
    public void AcquireObject(string tag) {
        GameObject target = CalculateNearestObject(GetSightedTargets(tag));
        if (target != null) {
            if (IsReachable(target)) {
                SetTargetArgs(target.transform.position);
                decidedActions[7] = 1;
            } else { MoveToPos(target.transform.position); }
        } else { SearchForObjects("Object"); }
    }

    // Handles these parameters 
    public void SetTargetArgs(Vector3 targetPos) {
            thisAnimal.GetMotorSystem().SetArgs("hand target x", targetPos.x);
            thisAnimal.GetMotorSystem().SetArgs("hand target y", targetPos.y);
            thisAnimal.GetMotorSystem().SetArgs("hand target x", targetPos.z);
    }

    // Sets laying down to true;
    public void DecreaseSleepiness() {
        decidedActions[3] = 1;
    }

    // Sets resting to true
    public void DecreaseFatigue() {
        decidedActions[12] = 1;
    }

    // Sets resting to true
    public void IncreaseHealth() {
        decidedActions[12] = 1;
    }

    // Determines whether the item in question should be eaten
    public bool checkEdible(GameObject questioned) {
        return true;
    }

    // Makes human stand if not already
    public void EnsureStanding() {
        if (bodyStateDict["standing"]) {}
        else { decidedActions[4] = 1; }
    }

    // Returns all objects inFOV of the passed tag
    public List<GameObject> GetSightedTargets(string targetTag) {
        List<GameObject> targetList = new List<GameObject>();
        foreach (GameObject x in inSight) {
            if (x.tag == targetTag) {
                targetList.Add(x);
            }
        }
        return targetList;
    }

    // Returns the nearest object to the human or null if none exists
    public GameObject CalculateNearestObject(List<GameObject> targetList) {
        GameObject nearestObject = null;
        if (targetList.Count > 0) {
            nearestObject = targetList[0];
            float nearestDis = Vector3.Distance(animalTransform.position, nearestObject.transform.position);

            for (int i = 0; i < targetList.Count; i++) {
                float distance = Vector3.Distance(animalTransform.position, targetList[i].transform.position);
                if (distance < nearestDis) {
                    nearestDis = distance;
                    nearestObject = targetList[i];
                }
            }
        }
        return nearestObject;  
    }

    // Determines whether an object can be grabbed from current position
    public bool IsReachable(GameObject target){
        float distance = Vector3.Distance(animalTransform.position, target.transform.position);
        if (distance < 1) {
            return true; 
        }
        return false;
    }

    // Determines whether to rotate in a pos or negative direction
    public int CalculateRelativePosition(Vector3 passedPosition) {
        Vector3 relativePosition = animalTransform.InverseTransformPoint(passedPosition);
        if (relativePosition.x < 0) {
            return -1;
        } else if (relativePosition.x > 0) { return 1; }
        return 0;
    }

    // Returns whether facing a position
    public bool IsFacing(Vector3 passedPos) {
        float angle = Vector3.Angle(passedPos - animalTransform.position, animalTransform.forward);
        if (angle <= 0.5f) { return true; }
        return false;
    }

    // Faces the passed position
    public void FacePosition(Vector3 passedPosition) {
        if(!IsFacing(passedPosition)) {
            decidedActions[5] = 1;
            if (CalculateRelativePosition(passedPosition) == -1) {
            thisAnimal.GetMotorSystem().SetArgs("rotation velocity", -0.05f);
            } else { thisAnimal.GetMotorSystem().SetArgs("rotation velocity", 0.05f); }
        }
    }

    // Moves to a random position (reset???)
    public void Explore() {
        Vector3 randomPos = CreateRandomPosition(3.0f);
        MoveToPos(randomPos);
    }

    public void SearchForObjects(string tag) {
        List<GameObject> sightedTargets = GetSightedTargets(tag);

        // While no useful objects are seen
        while (sightedTargets.Count < 1) {
            Debug.Log("No targets found");
            for(int i = 0; i < 180; i++) {
                decidedActions[5] = 1;
                thisAnimal.GetMotorSystem().SetArgs("rotation velocity", 1.0f);
                sightedTargets = GetSightedTargets(tag);
            }
            Explore();
        } 
        // Sighted an object, moving to it
        Debug.Log("walking to sighted object");
        Vector3 goalPos = (CalculateNearestObject(sightedTargets)).transform.position;
        MoveToPos(goalPos);
    }

    // Moves to passed position
    public void MoveToPos(Vector3 position) {
        EnsureStanding();
        FacePosition(position);
        thisAnimal.GetMotorSystem().SetArgs("step rate", 0.01f);

        if ((animalTransform.position - position).magnitude > 1) { 
            Debug.Log("walking to sighted object"); 
            decidedActions[6] = 1;
        }
    }

    public Vector3 CreateRandomPosition(float Range) {
        float xPos = animalTransform.position.x;
        float zPos = animalTransform.position.z;
        Vector3 toReturn = new Vector3(UnityEngine.Random.Range(xPos - Range, xPos + Range), thisAnimal.GetBody().GetHeight(), 
                                        UnityEngine.Random.Range(zPos - Range, zPos + Range));
        return toReturn;
    }
    
    // 
    public void UpdateFOV(Transform checkingObject, float maxAngle, float maxRadius) {
        Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        inSight = new List<GameObject>();

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
