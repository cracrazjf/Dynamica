using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    abstract public class Animal : LivingObject
    {
        public string displayName;

        private Body body;
        private DriveSystem driveSystem;
        private MotorSystem motorSystem;
        private SensorySystem sensorySystem;
        private struct actionChoiceStruct{
            bool[] actionChoiceArray;
            float[] actionArgumentArray;
        };
        
        public Animal(string objectType, int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome) 
        : base (objectType, index, position, motherGenome, fatherGenome)
        {
            this.displayName = GetObjectType();            
        }

        public string GetDisplayName(){
            return displayName;
        }

        public virtual void UpdateAnimal(){
            Debug.Log("No update defined for this animal");
        }

        // getters and setters for body, drive system, motor system, sensory system, and action choice class
        public Body GetBody(){
            return body;
        }

        public DriveSystem GetDriveSystem(){
            return driveSystem;
        }

        public MotorSystem GetMotorSystem(){
            return motorSystem;
        }

        public SensorySystem GetSensorySystem(){
            return sensorySystem;
        }

        public actionChoiceStruct GetActionChoiceStruct(){
            return actionChoiceStruct;
        }

        public bool[] getActionChoiceArray(){
            return actionChoiceStruct.actionChoiceArray;
        }

        public float[] getActionArgumentArray() {
            return actionChoiceStruct.actionArgumentArray;
        }

        public void SetBody(Body passed){
            body = passed;
        }

        public void SetDriveSystem(DriveSystem passed){
            driveSystem = passed;
        }

        public void SetMotorSystem(MotorSystem passed){
            motorSystem = passed;
        }

        public void SetSensorySystem(SensorySystem passed){
            sensorySystem = passed;
        }

        public void SetActionChoiceStruct(struct passed){
            actionChoiceStruct = passed;
        }

        public void SetActionChoiceArray(bool[] passed){
            actionChoiceStruct.actionChoiceArray = passed;
        }

        public void SetActionArgumentArray(float[] passed){
            actionChoiceStruct.actionArgumentArray = passed;
        }

    }
