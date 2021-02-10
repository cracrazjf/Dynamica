using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanSimpleAI
{
    Human thisHuman;
    Transform humanTransform;
    
    Dictionary<string, int> bodyStateIndexDict;
    Dictionary<string, int> driveStateIndexDict;
    Dictionary<string, int> actionStateIndexDict;
    Dictionary<string, int> actionArgumentIndexDict;
    Dictionary<string, float> traitDict;
    Animal.ActionChoiceStruct actionChoiceStruct;

    bool[] bodyStateArray;
    bool[] actionStateArray;
    float[] driveStateArray;

    string currentGoal = "None";
    List<GameObject> objInVis = new List<GameObject>();
    float rotatedAngle = 0;

    bool objectIsVisible;
    bool IsFacingToObject;
    bool generatedNewRandomPosition = false;
    Vector3 randomPosition;

    public HumanSimpleAI(Human human,
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
    public Animal.ActionChoiceStruct ChooseAction(bool[] bodyStateArray, bool[] actionStateArray, float[] driveStateArray, Dictionary<string, float> passedTraitDict)
    {
        this.bodyStateArray = bodyStateArray;
        this.actionStateArray = actionStateArray;
        this.driveStateArray = driveStateArray;
        this.traitDict = passedTraitDict;
        this.actionChoiceStruct.actionChoiceArray = new bool[actionStateIndexDict.Count];
        this.actionChoiceStruct.actionArgumentArray = new float[actionArgumentIndexDict.Count];

        this.humanTransform = thisHuman.gameObject.transform;
        InFov(thisHuman.gameObject.transform, 45, 10);
        Debug.DrawRay(humanTransform.position, humanTransform.forward * 10, Color.red);
        //Debug.Log(currentGoal);
        if (currentGoal == "None")
        {
            ChooseGoal();
        }
        else if (currentGoal == "Decrease Thirst")
        {
            DecreaseThirst();
        }
        else if (currentGoal == "Decrease Sleepiness")
        {
            DecreaseSleepiness();
        }
        else if (currentGoal == "Decrease Hunger")
        {
            DecreaseHunger();
        }
        else if (currentGoal == "Decrease Fatigue")
        {
            DecreaseFatigue();
        }
        return actionChoiceStruct;
    }
    public void ChooseGoal()
    {
        if (driveStateArray[driveStateIndexDict["thirst"]] > traitDict["thirst_threshold"])
        {
            currentGoal = "Decrease Thirst";
        }
        if (driveStateArray[driveStateIndexDict["hunger"]] > traitDict["hunger_threshold"])
        {
            currentGoal = "Decrease Hunger";
        }
        if (driveStateArray[driveStateIndexDict["sleepiness"]] > traitDict["sleepiness_threshold"])
        {
            currentGoal = "Decrease Sleepiness";
        }
        if (driveStateArray[driveStateIndexDict["fatigue"]] > traitDict["fatigue_threshold"])
        {
            currentGoal = "Decrease Fatigue";
        }
        if (driveStateArray[driveStateIndexDict["health"]] < traitDict["health_threshold"])
        {
            currentGoal = "Increase Health";
        }
    }
    public void Test()
    {
    }
    public void DecreaseThirst()
    {
        if (bodyStateArray[bodyStateIndexDict["standing"]])
        {
            CheckIfObjectVisible("Water");
            if (objectIsVisible)
            {
                rotatedAngle = 0;
                GameObject target = CalculateNearestObject(GetTargetObjectList("Water"));
                if (CheckIfObjectReachable(target))
                {
                    this.thisHuman.GetMotorSystem().EndAction("taking steps");
                    this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["drinking"]] = true;
                    currentGoal = "None";
                }
                else
                {
                    IsFacingTowardObejct(target.transform.position);
                    if (IsFacingToObject)
                    {
                        this.thisHuman.GetMotorSystem().EndAction("rotating");
                        this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["taking steps"]] = true;
                        this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["step rate"]] = 0.01f;
                    }
                    else
                    {
                        FacingTowardObejct(target.transform.position);
                    }
                }
            }
            else
            {
                SearchForObjects();
            }
        }
        else
        {
            if (bodyStateArray[bodyStateIndexDict["sitting"]])
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["standing up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["laying"]])
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["sleeping"]])
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["waking up"]] = true;
            }
        }
    }
    public void DecreaseHunger()
    {
        if (bodyStateArray[bodyStateIndexDict["standing"]])
        {
            if (((HumanBody)this.thisHuman.GetBody()).objectTypeInLH == "Food")
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["eating"]] = true;
                this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
                if (!this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with left hand")))
                {
                    currentGoal = "None";
                }
            }
            else if (((HumanBody)this.thisHuman.GetBody()).objectTypeInRH == "Food")
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["eating"]] = true;
                this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 1;
                if (!this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with right hand")))
                {
                    currentGoal = "None";
                }
            }
            else
            {
                CheckIfObjectVisible("Food");
                if (objectIsVisible)
                {
                    rotatedAngle = 0;
                    GameObject target = CalculateNearestObject(GetTargetObjectList("Food"));
                    if (CheckIfObjectReachable(target))
                    {
                        this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand target x"]] = target.transform.position.x;
                        this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand target y"]] = target.transform.position.y;
                        this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand target z"]] = target.transform.position.z;
                        this.thisHuman.GetMotorSystem().EndAction("taking steps");
                        if (this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with left hand"))
                            && this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with right hand")))
                        {
                            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["setting down"]] = true;
                            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
                        }
                        else if (!this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with left hand")))
                        {
                            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["picking up"]] = true;
                            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 0;
                        }
                        else if (!this.thisHuman.GetBody().GetBodyState(this.thisHuman.GetBody().GetBodyStateIndex("holding with right hand")))
                        {
                            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["picking up"]] = true;
                            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["hand"]] = 1;
                        }
                    }
                    else
                    {
                        IsFacingTowardObejct(target.transform.position);
                        if (IsFacingToObject)
                        {
                            this.thisHuman.GetMotorSystem().EndAction("rotating");
                            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["taking steps"]] = true;
                            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["step rate"]] = 0.01f;
                        }
                        else
                        {
                            FacingTowardObejct(target.transform.position);
                        }
                    }
                }
                else
                {
                    SearchForObjects();
                }
            }
        }

        else
        {
            if (bodyStateArray[bodyStateIndexDict["sitting"]])
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["standing up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["laying"]])
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting up"]] = true;
            }
            if (bodyStateArray[bodyStateIndexDict["sleeping"]])
            {
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["waking up"]] = true;
            }
        }
    }
    public void DecreaseSleepiness()
    {
        if (bodyStateArray[bodyStateIndexDict["laying"]])
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["falling asleep"]] = true;
        }
        else if (bodyStateArray[bodyStateIndexDict["sitting"]])
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["laying down"]] = true;
        }
        else
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting down"]] = true;
        }
    }
    public void DecreaseFatigue()
    {
        if (bodyStateArray[bodyStateIndexDict["standing"]])
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting down"]] = true;
        }
        else if (bodyStateArray[bodyStateIndexDict["laying"]])
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["sitting up"]] = true;
        }
    }
    public void CheckIfObjectVisible(string targetTag)
    {
        if (objInVis.Count > 0)
        {
            foreach (GameObject x in objInVis) {
                if (x.tag == targetTag)
                {
                    objectIsVisible = true;
                }
                else
                {
                    objectIsVisible = false;
                }
            }
        }
        else
        {
            objectIsVisible = false;
        }
        
    }
    public List<GameObject> GetTargetObjectList(string targetTag)
    {
        List<GameObject> targetList = new List<GameObject>();
        foreach (GameObject x in objInVis)
        {
            if (x.tag == targetTag)
            {
                targetList.Add(x);
            }
        }
        return targetList;
    }
    public GameObject CalculateNearestObject(List<GameObject> targetList)
    {
        if (targetList.Count == 1)
        {
            return targetList[0];
        }
        else
        {
            GameObject nearestObject = null;
            float largeNumber = Mathf.Infinity;
            for (int i =0; i < targetList.Count; i++)
            {
                float distance = Vector3.Distance(humanTransform.position, targetList[i].transform.position);
                if (distance < largeNumber)
                {
                    largeNumber = distance;
                    nearestObject = targetList[i];
                }
            }
            return nearestObject;
        }
        
    }
    public bool CheckIfObjectReachable(GameObject target)
    {
        float distance = Vector3.Distance(humanTransform.position, target.transform.position);
        if (distance < 1)
        {
            return true; 
        }
        else
        {
            return false;
        }
    }
    public float CalculateRotationAngle(Vector3 passedPosition)
    {
        Vector3 targetDirection = passedPosition - humanTransform.position;
        float angle = Vector3.Angle(targetDirection, humanTransform.forward);
        return angle;
    }
    public int CalculateRelativePosition(Vector3 passedPosition)
    {
        Vector3 relativePosition = humanTransform.InverseTransformPoint(passedPosition);
        if (relativePosition.x < 0)
        {
            return -1;
        }
        else if (relativePosition.x > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public void IsFacingTowardObejct(Vector3 passedPosition)
    {
        float angle = CalculateRotationAngle(passedPosition);
        if (angle <= 0.5f)
        {
            angle = 0;
        }
        if (angle == 0)
        {
            IsFacingToObject = true;
        }
        else
        {
            IsFacingToObject = false;
        }
    }
    public void FacingTowardObejct(Vector3 passedPosition)
    {
        if (CalculateRelativePosition(passedPosition) == -1)
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["rotating"]] = true;
            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["rotation velocity"]] = -0.05f;
        }
        else
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["rotating"]] = true;
            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["rotation velocity"]] = 0.05f;
        }
    }
    public void SearchForObjects()
    {
        if (rotatedAngle < 360)
        {
            this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["rotating"]] = true;
            this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["rotation velocity"]] = 1.0f;  
            rotatedAngle += 2.0f;
        }
        else
        {
            if(!generatedNewRandomPosition)
            {
                randomPosition = CreateRandomPosition(3.0f);
            }
            IsFacingTowardObejct(randomPosition);
            if(IsFacingToObject)
            {
                this.thisHuman.GetMotorSystem().EndAction("rotating");
                this.actionChoiceStruct.actionChoiceArray[actionStateIndexDict["taking steps"]] = true;
                this.actionChoiceStruct.actionArgumentArray[actionArgumentIndexDict["step rate"]] = 0.01f;
                if ((thisHuman.gameObject.transform.position - randomPosition).magnitude < 1)
                {
                    generatedNewRandomPosition = false;
                }
            }
            else
            {
                this.thisHuman.GetMotorSystem().EndAction("taking steps");
                FacingTowardObejct(randomPosition);
            }
        }

    }
    public Vector3 CreateRandomPosition(float Range)
    {
        Vector3 randomPosition = new Vector3(Random.Range(thisHuman.gameObject.transform.position.x - Range,
                                thisHuman.gameObject.transform.position.x + Range), 0,
                                Random.Range(thisHuman.gameObject.transform.position.z - Range,
                                thisHuman.gameObject.transform.position.z + Range));
        generatedNewRandomPosition = true;
        return randomPosition;
    }
    public void initActionChoiceStruct()
    {
        actionChoiceStruct = new Animal.ActionChoiceStruct();
        actionChoiceStruct.actionChoiceArray = new bool[actionStateIndexDict.Count];
        actionChoiceStruct.actionArgumentArray = new float[actionArgumentIndexDict.Count];
    }
    public void InFov(Transform checkingObject, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        objInVis.Clear();

        for (int i = 0; i < count + 1; i++)
        {

            if (overlaps[i] != null)
            {
                Vector3 directionBetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                directionBetween.y *= 0;

                float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxAngle)
                {
                    Ray ray = new Ray(new Vector3(checkingObject.position.x, 0.1f, checkingObject.position.z), overlaps[i].transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
                        if (hit.transform == overlaps[i].transform)
                        {
                            if (!objInVis.Contains(overlaps[i].gameObject) && overlaps[i].tag != "ground")
                            {
                                objInVis.Add(overlaps[i].gameObject);

                            }
                        }

                    }

                }
            }
        }
    }
}
