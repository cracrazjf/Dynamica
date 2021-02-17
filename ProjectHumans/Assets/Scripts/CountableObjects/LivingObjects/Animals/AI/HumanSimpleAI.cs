using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanSimpleAI : AI
{
    Human thisHuman;
    Transform humanTransform;

    string currentGoal = "None";
    List<GameObject> inSight = new List<GameObject>();
    float rotatedAngle = 0;

    bool generatedNewRandomPosition = false;
    Vector3 randomPosition;

    public HumanSimpleAI(Human human, Dictionary<string, bool> bodyStateDict, Dictionary<string, float> driveStateDict, 
                        Dictionary<string, bool> actionChoiceDict, Dictionary<string, float> actionArgumentDict, 
                        Dictionary<string, float> traitDict) : base(bodyStateDict, driveStateDict, actionChoiceDict, actionArgumentDict, traitDict) {
        this.thisHuman = human;
        actionChoiceStruct.actionArgumentDict = actionArgumentDict;
        actionChoiceStruct.actionChoiceDict = actionChoiceDict;
    }

    public override ActionChoiceStruct ChooseAction(float[,] visualInput, Dictionary<string, float> passedTraitDict)
    {
        this.traitDict = passedTraitDict;

        this.humanTransform = thisHuman.gameObject.transform;
        InFov(thisHuman.gameObject.transform, 45, 10);
        Debug.DrawRay(humanTransform.position, humanTransform.forward * 10, Color.red);
        
        if (currentGoal == "None") { test(); }
        else if (currentGoal == "Decrease Thirst") { DecreaseThirst(); }
        else if (currentGoal == "Decrease Sleepiness") { DecreaseSleepiness(); }
        else if (currentGoal == "Decrease Hunger") { DecreaseHunger(); }
        else if (currentGoal == "Decrease Fatigue") { DecreaseFatigue(); }
        return actionChoiceStruct;
    }
    public void test()
    {
        //this.actionChoiceStruct.actionChoiceDict["taking steps"] = true;
        //this.actionChoiceStruct.actionArgumentDict["step rate"] = 0.01f;
    }
    public void ChooseGoal()
    {
        if (driveStateDict["thirst"] > traitDict["thirst_threshold"])
        {   currentGoal = "Decrease Thirst";}
        if (driveStateDict["hunger"] > traitDict["hunger_threshold"])
        {   currentGoal = "Decrease Hunger";}
        if (driveStateDict["sleepiness"] > traitDict["sleepiness_threshold"])
        {   currentGoal = "Decrease Sleepiness";}
        if (driveStateDict["fatigue"] > traitDict["fatigue_threshold"])
        {   currentGoal = "Decrease Fatigue";}
        if (driveStateDict["health"] < traitDict["health_threshold"])
        {   currentGoal = "Increase Health";}
    }
  
    public void DecreaseThirst() {
        if (bodyStateDict["standing"]) {
            if (IsVisible("Water")) {
                rotatedAngle = 0;
                GameObject target = CalculateNearestObject(GetTargetObjectList("Water"));
                if (IsReachable(target)) {
                    this.thisHuman.GetMotorSystem().EndAction("taking steps");
                    this.actionChoiceStruct.actionChoiceDict["drinking"] = true;
                    currentGoal = "None";
                } else if (IsFacing(target.transform.position)) {
                    this.thisHuman.GetMotorSystem().EndAction("rotating");
                    this.actionChoiceStruct.actionChoiceDict["taking steps"] = true;
                    this.actionChoiceStruct.actionArgumentDict["step rate"] = 0.01f;
                } else {
                    FacePosition(target.transform.position);
                }
            } else {
                SearchForObjects();
            }
        } else {
            if (bodyStateDict["sitting"]) {
                this.actionChoiceStruct.actionChoiceDict["standing up"] = true;
            }
            if (bodyStateDict["laying"]) {
                this.actionChoiceStruct.actionChoiceDict["sitting up"] = true;
            }
            if (bodyStateDict["sleeping"]) {
                this.actionChoiceStruct.actionChoiceDict["waking up"] = true;
            }
        }
    }

    public void DecreaseHunger()
    {
        if (bodyStateDict["standing"]) {
            if (this.thisHuman.GetBody().labelLH == "Food") {
                this.actionChoiceStruct.actionChoiceDict["eating"] = true;
                this.actionChoiceStruct.actionArgumentDict["hand"] = 0;
                if (!bodyStateDict["holding with left hand"]) {
                    currentGoal = "None";
                }
            } else if (this.thisHuman.GetBody().labelRH == "Food") {
                this.actionChoiceStruct.actionChoiceDict["eating"] = true;
                this.actionChoiceStruct.actionArgumentDict["hand"] = 1;
                if (!bodyStateDict["holding with right hand"]) {
                    currentGoal = "None";
                }
            } else if (IsVisible("Food")) {
                rotatedAngle = 0;
                GameObject target = CalculateNearestObject(GetTargetObjectList("Food"));
                if (IsReachable(target)) {
                    this.actionChoiceStruct.actionArgumentDict["hand target x"] = target.transform.position.x;
                    this.actionChoiceStruct.actionArgumentDict["hand target y"] = target.transform.position.y;
                    this.actionChoiceStruct.actionArgumentDict["hand target z"] = target.transform.position.z;
                    this.thisHuman.GetMotorSystem().EndAction("taking steps");

                    if (bodyStateDict["holding with left hand"] && bodyStateDict["holding with right hand"]) {
                        //Both hands full
                        this.actionChoiceStruct.actionChoiceDict["setting down"] = true;
                        this.actionChoiceStruct.actionArgumentDict["hand"] = 0;
                    } else if (!bodyStateDict["holding with left hand"]) {
                        //Grab with LH
                        this.actionChoiceStruct.actionChoiceDict["picking up"] = true;
                        this.actionChoiceStruct.actionArgumentDict["hand"] = 0;
                    } else if (!bodyStateDict["holding with right hand"]) {
                        //Grab with RH
                        this.actionChoiceStruct.actionChoiceDict["picking up"] = true;
                        this.actionChoiceStruct.actionArgumentDict["hand"] = 1;
                    }

                } else if (IsFacing(target.transform.position)) {
                    this.thisHuman.GetMotorSystem().EndAction("rotating");
                    this.actionChoiceStruct.actionChoiceDict["taking steps"] = true;
                    this.actionChoiceStruct.actionArgumentDict["step rate"] = 0.01f;
                } else { 
                    FacePosition(target.transform.position); 
                }
            } else {
                SearchForObjects();
            }
        } else if (bodyStateDict["sitting"] ){
            this.actionChoiceStruct.actionChoiceDict["standing up"] = true;
        } else if (bodyStateDict["laying"]) {
            this.actionChoiceStruct.actionChoiceDict["sitting up"] = true;
        } else if (bodyStateDict["sleeping"]) {
            this.actionChoiceStruct.actionChoiceDict["waking up"] = true;
        }
    }

    public void DecreaseSleepiness() {
        if (bodyStateDict["laying"]) {
            this.actionChoiceStruct.actionChoiceDict["falling asleep"] = true;
        } else if (bodyStateDict["sitting"]) {
            this.actionChoiceStruct.actionChoiceDict["laying down"] = true;
        } else {
            this.actionChoiceStruct.actionChoiceDict["sitting down"] = true;
        }
    }

    public void DecreaseFatigue() {
        if (bodyStateDict["standing"]) {
            this.actionChoiceStruct.actionChoiceDict["sitting down"] = true;
        } else if (bodyStateDict["laying"]) {
            this.actionChoiceStruct.actionChoiceDict["sitting up"] = true;
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
        if (angle <= 0.5f) { 
            return true;
            }
        return false;
    }

    public void FacePosition(Vector3 passedPosition) {
        if (CalculateRelativePosition(passedPosition) == -1) {
            actionChoiceStruct.actionChoiceDict["rotating"] = true;
            actionChoiceStruct.actionArgumentDict["rotation velocity"] = -0.05f;
        } else {
            actionChoiceStruct.actionChoiceDict["rotating"] = true;
            actionChoiceStruct.actionArgumentDict["rotation velocity"] = 0.05f;
        }
    }

    public void SearchForObjects() {
        if (rotatedAngle < 360) {
            actionChoiceStruct.actionChoiceDict["rotating"] = true;
            actionChoiceStruct.actionArgumentDict["rotation velocity"] = 1.0f;  
            rotatedAngle += 2.0f;
        } else if (!generatedNewRandomPosition) {
            randomPosition = CreateRandomPosition(3.0f);
        }

        FacePosition(randomPosition);
        if(IsFacing(CalculateNearestObject(GetTargetObjectList("Food")).transform.position)) {
            thisHuman.GetMotorSystem().EndAction("rotating");
            actionChoiceStruct.actionChoiceDict["taking steps"] = true;
            actionChoiceStruct.actionArgumentDict["step rate"] = 0.01f;
            if ((thisHuman.gameObject.transform.position - randomPosition).magnitude < 1) {
                generatedNewRandomPosition = false;
            } else {
                this.thisHuman.GetMotorSystem().EndAction("taking steps");
                FacePosition(randomPosition);
            }
        }
    }

    public Vector3 CreateRandomPosition(float Range) {
        Vector3 randomPosition = new Vector3(Random.Range(thisHuman.gameObject.transform.position.x - Range,
                                thisHuman.gameObject.transform.position.x + Range), 0,
                                Random.Range(thisHuman.gameObject.transform.position.z - Range,
                                thisHuman.gameObject.transform.position.z + Range));
        generatedNewRandomPosition = true;
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
