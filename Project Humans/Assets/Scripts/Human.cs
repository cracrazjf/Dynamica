using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Human : MonoBehaviour
{
    // fixed traits
    public Transform human;
    float rotationleft = 360;
    float rotationspeed = 10;
    public float movementSpeed = 1.0f;
    public string sex = "None";

    public float hungerChange = 0.001f;
    public float thirstChange = 0.001f;
    public float fatigueChange = 0.001f;
    public float sleepinessChange = 0.001f;
    public float healthChange = 0.001f;

    public float hungerThreshold = 0.90f;
    public float thirstThreshold = 0.90f;
    public float fatigueThreshold = 0.99f;
    public float sleepinessThreshold = 0.90f;

    // changing states
    public float hunger = 0.0f;
    public float thirst = 0.0f;
    public float fatigue = 0.0f;
    public float sleepiness = 0.0f;
    public float health = 0.0f;

    // goals and tasks
    public string currentGoal;
    public string currentTask;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.position += transform.forward * Time.deltaTime * movementSpeed;

        //ChooseAction();
    }

    void ChooseAction()
    {
        if (hunger >= hungerThreshold){
            Eat();
        }        
        else if (thirst >= thirstThreshold){
            Drink();
        }
        else if (sleepiness >= sleepinessThreshold){
            Sleep();
        }
        else if (fatigueChange >= fatigueThreshold){
            Rest();
        }
        else{
            string maxDrive = GetMaxDrive();
            if (maxDrive == "hunger"){
                Eat();
            }
            else if (maxDrive == "thirst"){
                Drink();
            }
            else if (maxDrive == "fatigue"){
                Rest();
            }
            else if (maxDrive == "sleepiness"){
                Sleep();
            }
            else{
                Rest();
            }

        }
    }

    void Eat()
    {
        // add the code for moving to food, at consuming it
        //      start turning 360 degrees
        //      if in the course of turning, food is seen, stop
        //          walk to the food seen
        //          upon contact, consume food (remove apple) and update hunger
        //      else
        //          begin searching for food
        //          
    }

    void Drink()
    {
        // add the code for moving to water, at consuming it
        float rotation = rotationspeed * Time.deltaTime;
        if (rotationleft > rotation)
        {
            rotationleft -= rotation;
        }
        else
        {
            rotation = rotationleft;
            rotationleft = 0;
        }
        transform.Rotate(0, rotation, 0);
        //      if in the course of turning, water is seen, stop
        //      walk to the food seen
        //      upon contact, consume food (remove apple) and update hunger
    }

    void Rest()
    {
        // do nothing
    }

    void Sleep()
    {
        // add the code for laying down and sleeping
    }

    void Search(string searchObject)
    {

    }

    string GetMaxDrive()
    {
        string maxDrive = "hunger";
        float maxDriveAmount = hunger;
        if (thirst > maxDriveAmount){
            maxDrive = "thirst";
            maxDriveAmount = thirst;
        }
        else if (fatigue > maxDriveAmount){
            maxDrive = "fatigue";
            maxDriveAmount = fatigue;
        }
        else if (sleepiness > maxDriveAmount){
            maxDrive = "sleepiness";
            maxDriveAmount = sleepiness;
        }
        else if (health > maxDriveAmount){
            maxDrive = "health";
            maxDriveAmount = health;
        }
        return maxDrive;
    }

    void UpdateDrives()
    {
        if (hunger < 1.0){
            hunger += hungerChange;
        }
        if (thirst < 1.0){
            thirst += thirstChange;
        }
        if (fatigue > 0.0){
            fatigue -= fatigueChange;
        }
        if (sleepiness < 1.0){
            sleepiness += sleepinessChange;
        }
        if (health < 1.0){
            health += healthChange;
        }
    }
}
