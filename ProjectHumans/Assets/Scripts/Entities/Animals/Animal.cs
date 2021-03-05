using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    public class Animal : Entity {

        public Camera visualInputCamera;
        
        private static AnimalBody animalBody;
        private static DriveSystem driveSystem;
        private static MotorSystem motorSystem;
        private static SensorySystem sensorySystem;
        protected AI activeAI;
        protected string action;

        public Animal(string objectType, int index, Genome motherGenome, Genome fatherGenome, Vector3 spawn) 
        : base (objectType, index, motherGenome, fatherGenome, spawn) {
            
            animalBody = new PrimateBody(this, spawn);
            body = animalBody;
            motorSystem = new PrimateMotorSystem(this);
            
            visualInputCamera = animalBody.GetGameObject().GetComponentInChildren<Camera>();
        
            driveSystem = new DriveSystem(this);
            sensorySystem = new SensorySystem(this);

            activeAI = new SimpleAI(this, GetBody(), GetDriveSystem(), GetMotorSystem(), GetSensorySystem(), GetPhenotype());
        }

        public override void UpdateEntity() {
            this.GetDriveSystem().UpdateDrives();
            float[ , ] visualInputMatrix = GetSensorySystem().GetVisualInput();
            int[] toSend = activeAI.ChooseAction(visualInputMatrix);
        
            this.GetMotorSystem().TakeAction(toSend);
            action = "In progress!";

            IncreaseAge(1);
        }

        public void ToggleBodyPart(string part, bool toggle) {
            this.GetBody().GetSkeletonDict()[part].gameObject.SetActive(toggle);
        }
    
        // getters and setters for body, drive system, motor system, sensory system, and action choice class
        public AnimalBody GetBody() { return animalBody; }

        public bool GetBodyState(string state) { return this.GetBody().GetStateDict()[state]; }

        public DriveSystem GetDriveSystem() { return driveSystem; }

        public MotorSystem GetMotorSystem() { return motorSystem; }

        public SensorySystem GetSensorySystem() { return sensorySystem; }

        public string GetAction() { return this.activeAI.GetAction(); }
    }
