using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class HumanSimpleAI2
{
    public Human thisHuman;
    Vector3 randomPoint;
    int Range = 10;
    string currentGoal = "None";
    public Vector3 pickUpPosition = new Vector3();
    bool rotated360 = false;
    bool newRandomPos = false;
    bool IsFacingToObject = false;
    float rotatedAngle = 0;
    string objectTypeInLH = "None";
    string objectTypeInRH = "None";
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> objects_in_vision = new List<GameObject>();

    Dictionary<string, int> bodyStateIndexDict;
    Dictionary<string, int> driveStateIndexDict;
    Dictionary<string, int> actionStateIndexDict;
    Dictionary<string, int> actionArgumentIndexDict;
    Dictionary<string, float> traitDict;
  
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
        this.actionChoiceStruct.actionChoiceArray = new bool[actionStateIndexDict.Count];
        this.actionChoiceStruct.actionArgumentArray = new float[actionArgumentIndexDict.Count];
        InFov(thisHuman.gameObject.transform, 45,10);
        objectTypeInLH = ((HumanBody)this.thisHuman.GetBody()).objectTypeInLH;
        objectTypeInRH = ((HumanBody)this.thisHuman.GetBody()).objectTypeInRH;
        

        bool doingNothing = !this.actionStateArray.Any(x => x);

        if (currentGoal == "None"){
            ChooseGoal();
        }
        else if (currentGoal == "Decrease Thirst"){
            DecreaseThirst();
        }
        else if (currentGoal == "Decrease Sleepiness"){
            DecreaseSleepiness();
        }
        else if (currentGoal == "Decrease Hunger"){
            DecreaseHunger();
            
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["picking up"]] = true;
            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
            if (objectTypeInLH == "Food"){
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["eating"]] = true;
                this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
            }
        }
        else if (currentGoal == "Decrease Fatigue"){
            DecreaseFatigue();
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
    }

    public void DecreaseThirst(){
        if (bodyStateArray[bodyStateIndexDict["standing"]]) {
            if (CheckIfTargetVisible("Water").Count > 0) {
                Debug.Log("seeing a water");
                List<GameObject> targets = CheckIfTargetVisible("Water");
                GameObject target = CalculateClosestObject(targets);
                if (CheckIfTargetReachable(target)) {
                    this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["drinking"]] = true;
                    currentGoal = "None";
                }
                else {
                    IsFacingTowardObejct(target.transform.position);
                    if (IsFacingToObject) {
                        GoToObject();
                    }
                    else {
                        FacingTowardObejct(target.transform.position);
                    }
                    
                }
            }
            else {
                //SearchForThing();
            }
        }
        else {
            if (bodyStateArray[bodyStateIndexDict["sitting"]]) {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["standing up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["laying"]]) {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["sleeping"]]) {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["waking up"]] = true;
            }
        }
    }

    public void DecreaseHunger(){
        if (bodyStateArray[bodyStateIndexDict["standing"]]) {
            if (objectTypeInLH == "Food"){
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["eating"]] = true;
                this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
                if (!bodyStateArray[bodyStateIndexDict["holding with left hand"]]) {
                    objectTypeInLH = "None";
                    currentGoal = "None";
                }
                
            }
            else{
                if (objectTypeInRH == "Food"){
                    this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["eating"]] = true;
                    this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 1;
                    currentGoal = "None";
                    objectTypeInRH = "None";
                }
                else{
                    if (CheckIfTargetVisible("Food").Count > 0) {
                        List<GameObject> targets = CheckIfTargetVisible("Food");
                        GameObject target = CalculateClosestObject(targets);
                        if (CheckIfTargetReachable(target)) {
                            Vector3 relativePosition = this.thisHuman.gameObject.transform.InverseTransformPoint(target.transform.position);
                            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand target x"]] = relativePosition.x;
                            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand target y"]] = relativePosition.y;  
                            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand target z"]] = relativePosition.z;
                            if ((objectTypeInLH != "None") && (objectTypeInRH != "None")){
                                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["setting down"]] = true;
                                this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
                                objectTypeInLH = "None";
                            }
                            else{
                                if (objectTypeInLH == "None") {
                                    this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["picking up"]] = true;
                                    this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
                                    if (bodyStateArray[bodyStateIndexDict["holding with left hand"]]){
                                        objectTypeInLH = "Food";
                                    }
                                }
                                else if (objectTypeInRH == "None"){
                                    pickUpPosition = target.transform.position;
                                    this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["picking up"]] = true;
                                    this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 1;
                                    if (bodyStateArray[bodyStateIndexDict["holding with right hand"]]){
                                        objectTypeInRH = "Food";
                                    }
                                }
                            }
                        }
                        else {
                            IsFacingTowardObejct(target.transform.position);
                            if (IsFacingToObject) {
                                GoToObject();
                            }
                            else {
                                FacingTowardObejct(target.transform.position);
                            }
                
                        }
                    }
                    else {
                        SearchForThing();
                    }
                }
            }
        }
        
        else {
            if (bodyStateArray[bodyStateIndexDict["sitting"]]) {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["standing up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["laying"]]) {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["sleeping"]]) {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["waking up"]] = true;
            }
        }
    }
    
    public void DecreaseSleepiness() {
        if (bodyStateArray[bodyStateIndexDict["laying"]]) {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["falling asleep"]] = true;
        }
        else {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["laying down"]] = true;
        }
    }

    public void DecreaseFatigue() {
        if (bodyStateArray[bodyStateIndexDict["standing"]]) {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting down"]] = true;
        }
        else if (bodyStateArray[bodyStateIndexDict["laying"]]) {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting up"]] = true;
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

    public bool CheckIfTargetReachable(GameObject passedTarget) {
        if (passedTarget != null) {
            var distance = Vector3.Distance(thisHuman.gameObject.transform.position, passedTarget.transform.position);
            if (distance <=0.5 && !this.thisHuman.GetMotorSystem().GetActionState(actionStateIndexDict["taking steps"])) { // is there a way we can get rid of this velocity?
                return true;
            }
            else {
                return false;
            }
        }
        return false;
    }

    public float CalculateRotationAngle(Vector3 passedPosition) {
        Vector3 targetDirection = passedPosition - this.thisHuman.gameObject.transform.position;
        float angle = Vector3.Angle(targetDirection, this.thisHuman.gameObject.transform.forward);
        return angle;
    }
    public int CalculateRelativePosition(Vector3 passedPosition) {
        Vector3 relativePosition = this.thisHuman.gameObject.transform.InverseTransformPoint(passedPosition);
        if (relativePosition.x < 0) {
            return -1;
        }
        else if (relativePosition.x > 0) {
            return 1;
        }
        else {
            return 0;
        }
    }
    public void IsFacingTowardObejct(Vector3 passedPosition) {
        float angle = CalculateRotationAngle(passedPosition);
        if (angle < 2) {
            angle = 0;
        }
        if(angle == 0){
            IsFacingToObject = true;
        }
        else {
            IsFacingToObject = false;
        }
    }
    public void FacingTowardObejct(Vector3 passedPosition) {
        if(CalculateRelativePosition(passedPosition) == -1) {
            Rotate(-10.0f);
        }
        else{
            Rotate(10.0f);
        }
    }

    public void GoToObject() {
        this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["taking steps"]] = true;
        this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["step rate"]] = 1.0f;
    }
    public void Rotate(float anglePerSec) {
        this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["rotating"]] = true;
        this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["rotation angle"]] = anglePerSec;
        rotatedAngle += anglePerSec * Time.deltaTime;
    }
    public void resetRotatedAngle(){
        rotatedAngle = 0;
    }

    public void SearchForThing(){
        if (rotatedAngle <= 360 && !rotated360) {
            Rotate(30.0f);
        }
        else {
            rotated360 = true;
            resetRotatedAngle();
            if (!newRandomPos) {
            randomPoint = new Vector3(Random.Range(thisHuman.gameObject.transform.position.x - Range, 
                                thisHuman.gameObject.transform.position.x + Range), 0, 
                                Random.Range(thisHuman.gameObject.transform.position.z - Range, 
                                thisHuman.gameObject.transform.position.z + Range));
            newRandomPos = true;
            }
            IsFacingTowardObejct(randomPoint);
            if (IsFacingToObject) {
                    GoToObject();
            }
            else {
                FacingTowardObejct(randomPoint);
            }
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
                    Vector3 directionBetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxAngle)
                {
                    Ray ray = new Ray(checkingObject.position, overlaps[i].transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
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



