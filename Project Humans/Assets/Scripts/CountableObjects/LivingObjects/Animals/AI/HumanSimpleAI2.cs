using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class HumanSimpleAI2 : AI
{
    public Human thisHuman;
    Vector3 randomPoint;
    int Range = 10;
    string currentGoal = "None";
    string targetTag = "None";
    public Vector3 pickUpPosition = new Vector3();
    bool doingNothing;
    bool newRandomPos = false;
    string objectTypeInLH = "None";
    string objectTypeInRH = "None";
    public float rotateAngle = 360; // what is this, and should it be in phenotype?
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> objects_in_vision = new List<GameObject>();

    Dictionary<string, int> bodyStateIndexDict;
    Dictionary<string, int> driveStateIndexDict;
    Dictionary<string, int> actionStateIndexDict;
    Dictionary<string, int> actionArgumentIndexDict;
    Dictionary<string, float> traitDict;
    
    float[,] visualInputMatrix;
    bool[] bodyStateArray;
    bool[] actionStateArray;
    float[] driveStateArray;

    public Animal.ActionChoiceStruct actionChoiceStruct;

    public HumanSimpleAI2 (Human human,
                           Dictionary<string, int> bodyStateIndexDict,
                           Dictionary<string, int> driveStateIndexDict,
                           Dictionary<string, int> actionStateIndexDict, 
                           Dictionary<string, int> actionArgumentIndexDict,
                           Dictionary<string, float> traitDict)
    {
        this.thisHuman = human;
        this.bodyStateIndexDict = bodyStateIndexDict;
        this.driveStateIndexDict = driveStateIndexDict;
        this.actionStateIndexDict = actionStateIndexDict;
        this.actionArgumentIndexDict = actionArgumentIndexDict;
        this.traitDict = traitDict;
        initActionChoiceStruct();
    }

    public Animal.ActionChoiceStruct ChooseAction(float[,] visualInputMatrix, bool[] bodyStateArray, bool[] actionStateArray, float[] driveStateArray, Dictionary<string, float> passedTraitDict){
        this.bodyStateArray = bodyStateArray;
        this.actionStateArray = actionStateArray;
        this.driveStateArray = driveStateArray;
        this.traitDict = passedTraitDict;

        InFov(this.thisHuman.gameObject.transform, 45,10);

        bool doingNothing = !this.actionStateArray.Any(x => x);

        if (currentGoal == "None"){
            ChooseGoal();
        }

        if (currentGoal == "Decrease Thirst"){
            DecreaseThirst();
        }
        else if (currentGoal == "Decrease Sleepiness"){
            DecreaseSleepiness();
        }
        else if (currentGoal == "Decrease Hunger"){
            DecreaseHunger();
        }
        else if (currentGoal == "Decrease Fatigue"){
            DecreaseHunger();
        }

        return actionChoiceStruct;
    }

    public void ChooseGoal(){

        if (driveStateArray[driveStateIndexDict["thirst"]] > traitDict["thirst_threshold"]) {
            currentGoal = "Decrease Thirst";
        }
        if (driveStateArray[driveStateIndexDict["hunger"]] > traitDict["hunger_threshold"]) {
            currentGoal = "Decrease Hunger";
        }
        if (driveStateArray[driveStateIndexDict["sleepiness"]] > traitDict["sleepiness_threshold"]) {
            currentGoal = "Decrease Sleepiness";
        }
        if (driveStateArray[driveStateIndexDict["fatigue"]] > traitDict["fatigue_threshold"]) {
            currentGoal = "Decrease Fatigue";
        }
        if (driveStateArray[driveStateIndexDict["health"]] < traitDict["health_threshold"]) {
            currentGoal = "Increase Health";
        }
        rotateAngle = 360; // and why is it being set here, wasnt it defined outside and passed in? does it change sometimes?
    }

    public void DecreaseThirst(){

        if (bodyStateArray[bodyStateIndexDict["standing"]]) {
            if (CheckIfTargetVisible("Water").Count > 0) {
                List<GameObject> targets = CheckIfTargetVisible("Water");
                GameObject target = CalculateClosestObject(targets);
                if (CheckIfTargetReachable(target)) {
                    thisHuman.getActionChoiceArray()[actionStateIndexDict["drinking"]] = true;
                    currentGoal = "None";
                }
                else {
                    GoToObject(target);
                }
            }
            else {
                Rotate(1.0f);
                SearchForThing();
            }
        }
        else {
            if (bodyStateArray[bodyStateIndexDict["sitting"]]) {
                thisHuman.getActionChoiceArray()[actionStateIndexDict["standing up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["laying"]]) {
                thisHuman.getActionChoiceArray()[actionStateIndexDict["sitting up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["sleeping"]]) {
                thisHuman.getActionChoiceArray()[actionStateIndexDict["waking up"]] = true;
            }
        }
    }

    public void DecreaseHunger(){
        if (bodyStateArray[bodyStateIndexDict["sitting"]]) {
            if (objectTypeInLH == "Food"){
                thisHuman.getActionChoiceArray()[actionStateIndexDict["eatting"]] = true;
                thisHuman.getActionArgumentArray()[actionArgumentIndexDict["hand"]] = 0;
                if (!bodyStateArray[bodyStateIndexDict["holding with left hand"]]) {
                    objectTypeInLH = "None";
                    currentGoal = "None";
                }
                
            }
            else{
                if (objectTypeInRH == "Food"){
                    thisHuman.getActionChoiceArray()[actionStateIndexDict["eatting"]] = true;
                    thisHuman.getActionArgumentArray()[actionArgumentIndexDict["hand"]] = 1;
                    currentGoal = "None";
                    objectTypeInRH = "None";
                }
                else{
                    if (CheckIfTargetVisible("Food").Count > 0) {
                        List<GameObject> targets = CheckIfTargetVisible("Food");
                        GameObject target = CalculateClosestObject(targets);
                        if (CheckIfTargetReachable(target)) {
                            if ((objectTypeInLH != "None") && (objectTypeInRH != "None")){
                                thisHuman.getActionChoiceArray()[actionStateIndexDict["setting down"]] = true;
                                thisHuman.getActionArgumentArray()[actionArgumentIndexDict["hand"]] = 0;
                                objectTypeInLH = "None";
                            }
                            else{
                                if (objectTypeInLH == "None") {
                                    Debug.Log("here");
                                    pickUpPosition = target.transform.position;
                                    thisHuman.getActionChoiceArray()[actionStateIndexDict["picking up"]] = true;
                                    thisHuman.getActionArgumentArray()[actionArgumentIndexDict["hand"]] = 0;
                                    if (bodyStateArray[bodyStateIndexDict["holding with left hand"]]){
                                        objectTypeInLH = "Food";
                                    }
                                }
                                else if (objectTypeInRH == "None"){
                                    pickUpPosition = target.transform.position;
                                    thisHuman.getActionChoiceArray()[actionStateIndexDict["picking up"]] = true;
                                    thisHuman.getActionArgumentArray()[actionArgumentIndexDict["hand"]] = 1;
                                    if (bodyStateArray[bodyStateIndexDict["holding with right hand"]]){
                                        objectTypeInRH = "Food";
                                    }
                                }
                            }
                        }
                        else {
                            bool doingNothing = !actionStateArray.Any(x => x);
                            if (doingNothing || actionStateArray[actionStateIndexDict["taking steps"]]) {
                                // this needs to be be fixed so that target.transform is only modifying x and z, not y, correct?
                                this.thisHuman.gameObject.transform.LookAt(target.transform);
                                GoToObject(target);
                            }
                            
                        }
                    }
                    else {
                        Rotate(1.0f);
                        SearchForThing();
                    }
                }
            }
        }
        
        else {
            if (bodyStateArray[bodyStateIndexDict["sitting"]]) {
                thisHuman.getActionChoiceArray()[actionStateIndexDict["standing up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["laying"]]) {
                thisHuman.getActionChoiceArray()[actionStateIndexDict["sitting up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["sleeping"]]) {
                thisHuman.getActionChoiceArray()[actionStateIndexDict["waking up"]] = true;
            }
        }
    }
    
    public void DecreaseSleepiness() {
        if (bodyStateArray[bodyStateIndexDict["laying"]]) {
            thisHuman.getActionChoiceArray()[actionStateIndexDict["falling asleep"]] = true;
        }
        else {
            thisHuman.getActionChoiceArray()[actionStateIndexDict["laying down"]] = true;
        }
    }

    public void DecreaseFatigue() {
        if (bodyStateArray[bodyStateIndexDict["standing"]]) {
            thisHuman.getActionChoiceArray()[actionStateIndexDict["sitting down"]] = true;
        }
        else if (bodyStateArray[bodyStateIndexDict["laying"]]) {
            thisHuman.getActionChoiceArray()[actionStateIndexDict["sitting up"]] = true;
        }
    }
    
    public List<GameObject> CheckIfTargetVisible(string targetTag) {
        List<GameObject> targets = new List<GameObject>();
        
        if (objects_in_vision.Count != 0) {
            for (int i = 0; i < objects_in_vision.Count; i++) {
                    if (targetTag != null) {
                        if (objects_in_vision[i].CompareTag(targetTag)) {
                            targets.Add(objects_in_vision[i]);
                    }
                }
            }
        }
        return targets;
    }

    public GameObject CalculateClosestObject(List<GameObject> targets) {
        GameObject closestObject = null;
        float closestTargetDistance = Mathf.Infinity;
        for (int i = 0; i <targets.Count; i++) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, targets[i].transform.position);
            if (distance < closestTargetDistance) {
                closestTargetDistance = distance;
                closestObject = targets[i];
            }
        }
        return closestObject;

    }

    // this checks if reachable
    public bool CheckIfTargetReachable(GameObject target) {
        if (target != null) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, target.transform.position);
            if (distance <=1 && ((HumanMotorSystem)thisHuman.GetMotorSystem()).velocity == 0) { // is there a way we can get rid of this velocity?
                return true;
            }
            else {
                return false;
            }
        }
        return false;
        
    }

    public void GoToObject(GameObject target) {
        float maxStepDistance = traitDict["thirst_threshold"];

        if (target != null) {
            
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position,target.transform.position);
            if (distance > 0) {
                if (distance > 1) {
                    var stepRate = 1.0f;
                    var step = stepRate * maxStepDistance;
                    if (step >= distance) {
                        var nextStep = distance;
                        thisHuman.getActionChoiceArray()[actionStateIndexDict["taking steps"]] = true;
                        thisHuman.getActionArgumentArray()[actionArgumentIndexDict["movement"]] = 1;
                        thisHuman.getActionArgumentArray()[actionArgumentIndexDict["step rate"]] = nextStep / traitDict["max_step_distance"];

                    }
                    else {
                        thisHuman.getActionChoiceArray()[actionStateIndexDict["taking steps"]] = true;
                        thisHuman.getActionArgumentArray()[actionArgumentIndexDict["movement"]] = 0.5f;
                    }
                }
                else if (distance <= 1){
                    thisHuman.getActionChoiceArray()[actionStateIndexDict["taking steps"]] = true;
                    thisHuman.getActionArgumentArray()[actionArgumentIndexDict["movement"]] = 0;
                }
            }
        }
    }

    public void Rotate(float Angle) {
        thisHuman.getActionChoiceArray()[actionStateIndexDict["rotating"]] = true;
        thisHuman.getActionArgumentArray()[actionArgumentIndexDict["rotation angel"]] = 1;
    }

    public void SearchForThing(){
        if (((HumanMotorSystem)thisHuman.GetMotorSystem()).rotateAngle == 0) {
            if (!newRandomPos) {
            randomPoint = new Vector3(Random.Range(thisHuman.gameObject.transform.position.x - Range, 
                                thisHuman.gameObject.transform.position.x + Range), 0, 
                                Random.Range(thisHuman.gameObject.transform.position.z - Range, 
                                thisHuman.gameObject.transform.position.z + Range));
            newRandomPos = true;
            }
            thisHuman.gameObject.transform.LookAt(randomPoint);
            thisHuman.getActionChoiceArray()[actionStateIndexDict["taking steps"]] = true;
            thisHuman.getActionArgumentArray()[actionArgumentIndexDict["movement"]] = 0.5f;
            if ((thisHuman.gameObject.transform.position - randomPoint).magnitude < 3) {
                newRandomPos = false;
            }
        }

    }
    
    public void InFov(Transform checkingObject, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        objects_in_vision.Clear();
        
        for (int i = 0; i < count +1 ; i++)
        {
            
            if (overlaps[i] != null)
            {
                //Debug.Log(count);

                    Vector3 directionBetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxAngle)
                {
                    Ray ray = new Ray(checkingObject.position, overlaps[i].transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
                        //Debug.Log("hit");
                        if (hit.transform == overlaps[i].transform)
                        {

                            if (!objects_in_vision.Contains(overlaps[i].gameObject) && overlaps[i].tag != "ground")
                            {
                                objects_in_vision.Add(overlaps[i].gameObject);

                            }
                        }

                    }

                }
            }
        }
    }

    public void initActionChoiceStruct(){
        this.actionChoiceStruct = new Animal.ActionChoiceStruct();
        this.actionChoiceStruct.actionChoiceArray = new bool[actionStateIndexDict.Count];
        this.actionChoiceStruct.actionArgumentArray = new float[actionArgumentIndexDict.Count];
    }
    
}



