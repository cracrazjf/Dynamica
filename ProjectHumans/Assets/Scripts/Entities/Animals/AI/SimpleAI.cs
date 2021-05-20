using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

public class SimpleAI: AI {
  static Vector3 blankPos = new Vector3(0, 0, 0);
  Transform animalTransform;
  List <GameObject> inSight;
  Vector3 randomPos = blankPos;
  Matrix <float> decidedActions;
  List<Action> goalList;
  int goalIndex;

  public SimpleAI(Animal animal, Body body, DriveSystem drives, MotorSystem motor, SensorySystem senses, Phenotype traits):
    base(body, drives, motor, senses, traits) {
      thisAnimal = animal;
      //InitGoalDict();
    }

  public override Matrix < float > ChooseAction() {
    animalTransform = thisAnimal.GetBody().globalPos;
    decidedActions = Matrix < float > .Build.Dense(actionStates.Count(), 1);
    UpdateFOV(animalTransform, 45, 10);
    ChooseGoal();
    //goalDict[currentGoal].DynamicInvoke();
    return decidedActions;
  }

  public void ChooseGoal() {
    goalIndex = driveStateLabelList.Count;
    // for (int i = 0; i < goalIndex; i++) {
    //   Debug.Log(driveStateLabelList[i]);
    // }

    // for (int i = 0; i < goalIndex; i++) {
      
    //   if (driveStates[i] > traits[i]) {
    //     goalIndex = i;
    //   }
    // }
  }

  public void InitGoalDict() {
    goalList = new List<Action>();
    
    goalList.Add(Consume);
    goalList.Add(Consume);
    goalList.Add(DecreaseFatigue);
    goalList.Add(DecreaseSleepiness);
    goalList.Add(IncreaseHealth);
    goalList.Add(Explore);
  }
  // Action functions
  // Sets laying down to true;
  public void DecreaseSleepiness() {
    decidedActions[2, 0] = 1;
  }

  //// Sets resting to true
  public void DecreaseFatigue() {
    decidedActions[8, 0] = 1;
  }

  //// Sets resting to true
  public void IncreaseHealth() {
    decidedActions[8, 0] = 1;
  }

  public void Consume() {
    // Check for both hands (or positions of potential holding)
    Dictionary < string, GameObject > heldItems = thisAnimal.GetBody().GetHoldings();
    foreach(GameObject heldItem in heldItems.Values) {
      if (IsEdible(heldItem)) {
        decidedActions[8, 0] = 1;
      }
    }
    if (decidedActions[8, 1] != 1) {
      AcquireObject("Object");
    }
  }

  //// Makes human stand if not already
  public void EnsureStanding() {
    if (bodyStates[bodyStateIndexDict["standing"]] != 1) {} else {
      decidedActions[3, 0] = 1;
    }
  }

  public void Explore() {
    //Debug.Log("Exploring");
    if (randomPos == blankPos) {
      randomPos = GetRandomPosition(3.0f);
      Debug.Log("New pos is " + randomPos);
    }
    MoveToPos(randomPos);
  }

  //// Seeks out an object of the passed tag
  public void AcquireObject(string tag) {
    Debug.Log("Acquiring an object");
    GameObject target = GetNearestObject(GetSightedTargets(tag));
    if (target != null) {
      Debug.Log("Targeting a " + target.name);
      if (IsReachable(target)) {
        decidedActions[6, 0] = 1;
      } else {
        MoveToPos(target.transform.position);
      }
    } else {
      SearchForObjects("Object");
    }
  }

  public void SearchForObjects(string tag) {
    Debug.Log("Searching for an object");
    List < GameObject > sightedTargets = GetSightedTargets(tag);
    // While no useful objects are seen... changed to if else loops indefinitely
    if (sightedTargets.Count < 1) {
      Debug.Log("No targets found");
      for (int i = 0; i < 180; i++) {
        decidedActions[4, 0] = 1;
        sightedTargets = GetSightedTargets(tag);
      }
      Explore();
    } else {
      // Sighted an object, moving to it
      Debug.Log("Investigating something");
      Vector3 goalPos = (GetNearestObject(sightedTargets)).transform.position;
      MoveToPos(goalPos);
    }
  }

  // Moves to passed position
  public void MoveToPos(Vector3 position) {
    EnsureStanding();
    FacePosition(position);
    if ((animalTransform.position - position).magnitude > .1) {
      //Debug.Log("Walking to and fro");
      decidedActions[5, 0] = 1;
    } else {
      randomPos = blankPos;
    }
  }

  // Faces the passed position
  public void FacePosition(Vector3 targetPos) {
    if (!IsFacing(targetPos)) {
      if (GetRelativePosition(targetPos) == -1) {
        decidedActions[4, 0] = -1;
      } else {
        decidedActions[4, 0] = 1;;
      }
    }
  }

  // Helper functions
  // Returns whether facing a position
  public bool IsFacing(Vector3 targetPos) {
    float angle = Vector3.Angle(targetPos - animalTransform.position, animalTransform.forward);
    if (angle <= 0.5f) {
      return true;
    }
    return false;
  }

  // Determines whether an object can be grabbed from current position
  public bool IsReachable(GameObject target) {
    float distance = Vector3.Distance(animalTransform.position, target.transform.position);
    if (distance < 1) {
      return true;
    }
    return false;
  }

  // Determines whether the item in question should be eaten
  public bool IsEdible(GameObject item) {
    return true;
  }

  // Returns all objects inFOV of the passed tag
  public List < GameObject > GetSightedTargets(string targetTag) {
    List < GameObject > targetList = new List < GameObject > ();
    foreach(GameObject x in inSight) {
      if (x.tag == targetTag && x.tag != "Human") {
        targetList.Add(x);
        Debug.Log("Can see a " + x.name);
      }
    }
    return targetList;
  }

  // Returns the nearest object to the human or null if none exists
  public GameObject GetNearestObject(List < GameObject > targetList) {
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

  // Determines whether to rotate in a pos or negative direction
  public int GetRelativePosition(Vector3 targetPos) {
    Vector3 relativePosition = animalTransform.InverseTransformPoint(targetPos);
    if (relativePosition.x < 0) {
      return -1;
    } else if (relativePosition.x > 0) {
      return 1;
    }
    return 0;
  }

  public Vector3 GetRandomPosition(float Range) {
    float xPos = animalTransform.position.x;
    float zPos = animalTransform.position.z;
    Vector3 toReturn = new Vector3(UnityEngine.Random.Range(xPos - Range, xPos + Range), thisAnimal.GetBody().GetHeight(),
      UnityEngine.Random.Range(zPos - Range, zPos + Range));
    return toReturn;
  }

  public void UpdateFOV(Transform checkingObject, float maxAngle, float maxRadius) {
    Collider[] overlaps = new Collider[60];
    int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);
    inSight = new List < GameObject > ();
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