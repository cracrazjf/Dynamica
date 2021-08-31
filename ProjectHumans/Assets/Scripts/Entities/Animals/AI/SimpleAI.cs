using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

public class SimpleAI: AI {
    MotorSystem thisMotor;
    static Vector3 blankPos = new Vector3(0, 0, 0);
    Transform transform;
    List <GameObject> inSight = new List<GameObject>();
    Vector3 randomPos = blankPos;
    Matrix <float> decidedActions;
    List<Action> goalList;
    int goalIndex;
    public float thirst = 10;
    bool facingTarget;
    public float rotatedAngle = 0.0f;
    Vector3 randomPosition = Vector3.negativeInfinity;

    public SimpleAI(Animal animal, Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype traits):
    base(body, drives, motor, senses, traits) {
        thisMotor = motor;
        thisAnimal = animal;
        //InitGoalDict();
    }

    public override Matrix < float > ChooseAction() {
        transform = thisAnimal.GetGameObject().transform;
        decidedActions = Matrix < float > .Build.Dense(actionStates.Count(), 1);
        UpdateFOV(transform, 45, 10);
        DecreaseThirst();
        return decidedActions;
    }

    public void ChooseGoal() {
        goalIndex = driveStateLabelList.Count;
    }
    void DecreaseThirst()
    {
        if (inSight.Count > 0)
        {
            rotatedAngle = 0;
            ReachAndGrab();
        }
        else
        {
            if (rotatedAngle <= 360)
            {
                thisMotor.stateDict["rotate"] = 1.0f;
                thisMotor.actionList[4]();
                rotatedAngle += 10 * Time.deltaTime;
            }
            else
            {
                Explore();
            }
        }
    }
    void ReachAndGrab()
    {
        foreach (GameObject x in inSight)
        {

            if (IsReachable(x) || thisMotor.reached)
            {

                if (thisMotor.isCrouching)
                {
                    if (thisMotor.leftHand.childCount > 1)
                    {
                        thisMotor.Stand();
                        if (thirst > 9)
                        {
                            thisMotor.stateDict["active"] = -1;
                            thisMotor.actionList[6]();
                            thirst = 0;
                        }
                    }
                    else
                    {
                        thisMotor.actionList[10]();
                        thisMotor.stateDict["active left"] = 1;
                        thisMotor.stateDict["RP x"] = 1;
                        thisMotor.stateDict["RP z"] = 0.2f;
                        thisMotor.stateDict["right"] = 0.33f;
                    }
                }
                else
                {
                    thisMotor.actionList[0]();
                }
            }
            else
            {
                if (x.CompareTag("Water"))
                {
                    FacePosition(x.transform.position);
                    if (facingTarget)
                    {
                        thisMotor.actionList[5]();
                    }
                }
            }
        }
    }
    void Explore()
    {
        if (randomPosition.Equals(Vector3.negativeInfinity))
        {
            randomPosition = GenerateRandomPos();
            Debug.Log("picked a new position");
            Debug.Log(randomPosition);
        }
        if (Vector3.Distance(transform.position, randomPosition) < 1)
        {
            randomPosition = Vector3.negativeInfinity;
        }
        else
        {
            FacePosition(randomPosition);
            if (IsFacing(randomPosition))
            {
                thisMotor.stateDict["take steps"] = 1.0f;
                thisMotor.actionList[5]();
            }
        }

    }

    public void Sleep()
    {
        thisMotor.actionList[2]();
        thisMotor.actionList[7]();
    }
    public void FacePosition(Vector3 targetPos)
    {
        if (!IsFacing(targetPos))
        {
            if (GetRelativePosition(targetPos) == -1)
            {
                thisMotor.stateDict["rotate"] = 1.0f;
                thisMotor.actionList[4]();
            }
            else
            {
                thisMotor.stateDict["rotate"] = -1.0f;
                thisMotor.actionList[4]();
            }
        }
    }

    public int GetRelativePosition(Vector3 targetPos)
    {
        Vector3 relativePosition = transform.InverseTransformPoint(targetPos);
        if (relativePosition.x < 0)
        {
            return -1;
        }
        else if (relativePosition.x > 0)
        {
            return 1;
        }
        return 0;
    }

    public bool IsFacing(Vector3 targetPos)
    {
        float angle = Vector3.Angle(transform.forward, targetPos - transform.position);
        Debug.Log(angle);
        if (angle <= 13f)
        {
            facingTarget = true;
            return true;
        }
        facingTarget = false;
        return false;
    }

    public bool IsReachable(GameObject target)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 1.0f)
        {
            return true;
        }
        return false;
    }
    public Vector3 GenerateRandomPos()
    {
        float randomX = UnityEngine.Random.Range(-10, 10);
        float randomZ = UnityEngine.Random.Range(-10, 10);
        return new Vector3(transform.position.x + randomX, 0.6f, transform.position.z + randomZ);
    }
    public void UpdateFOV(Transform checkingObject, float maxAngle, float maxRadius)
    {
        LayerMask layermask = ~(1 << 9 | 1 << 8);
        Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps, layermask);
        inSight.Clear();
        for (int i = 0; i < count + 1; i++)
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
                            inSight.Add(overlaps[i].gameObject);
                        }
                    }
                }
            }
        }
    }
}