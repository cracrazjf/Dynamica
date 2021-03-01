using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    abstract public class Animal : LivingObject
    {
        public Camera visualInputCamera;
        
        private Body body;
        private DriveSystem driveSystem;
        private MotorSystem motorSystem;
        private SensorySystem sensorySystem;
        protected AI activeAI;
        protected string action;

        public Animal(string objectType, int index, Genome motherGenome, Genome fatherGenome) 
        : base (objectType, index, motherGenome, fatherGenome) {
            this.displayName = GetObjectType();             
        }

        public virtual void UpdateAnimal() {
            Debug.Log("No update defined for this animal");
        }

        // getters and setters for body, drive system, motor system, sensory system, and action choice class
        public Body GetBody() {
            return body;
        }

        public DriveSystem GetDriveSystem() {
            return driveSystem;
        }

        public MotorSystem GetMotorSystem() {
            return motorSystem;
        }

        public SensorySystem GetSensorySystem() {
            return sensorySystem;
        }

        public void SetBody(Body passed) {
            body = passed;
        }

        public void SetDriveSystem(DriveSystem passed) {
            driveSystem = passed;
        }

        public void SetMotorSystem(MotorSystem passed){
            motorSystem = passed;
        }

        public void SetSensorySystem(SensorySystem passed){
            sensorySystem = passed;
        }

        public string GetGoal() {
            return this.activeAI.GetGoal();
        }

        public string GetAction() {
            return this.GetAction();
        }
    }
