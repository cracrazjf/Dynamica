using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    abstract public class Animal : LivingObject
    {
        public string displayName;
        public Camera visualInputCamera;
        
        private Body body;
        private DriveSystem driveSystem;
        private MotorSystem motorSystem;
        private SensorySystem sensorySystem;
        
        public Animal(string objectType, int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome) 
        : base (objectType, index, position, motherGenome, fatherGenome) {
            this.displayName = GetObjectType();             
        }

        public string GetDisplayName() {
            return displayName;
        }

        public void SetDisplayName(string named) {
            this.displayName = named;
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
    }
