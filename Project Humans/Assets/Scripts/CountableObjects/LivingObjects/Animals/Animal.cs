using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    abstract public class Animal : LivingObject
    {
        private DriveSystem driveSystem;
        private MotorSystem motorSystem;
        private NervousSystem nervousSystem;
        private ActionChoice actionChoice;
        // private FOVDetection fovDetection;

        public string displayName;

        public Animal(string objectType, int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome) 
        : base (objectType, index, position, motherGenome, fatherGenome)
        {

            this.displayName = GetObjectType();
            this.driveSystem = new DriveSystem(this);
        }

        public string GetDisplayName(){
            return displayName;
        }

        public virtual void UpdateAnimal(){
            Debug.Log("No update defined for this animal");
        }

        public DriveSystem GetDriveSystem(){
            return driveSystem;
        }

        public virtual ActionChoice GetActionChoice(){
            return actionChoice;
        }

        // public virtual FOVDetection GetFOVDetection(){
        //     return fovDetection;
        // }

        public virtual MotorSystem GetMotorSystem(){
            return motorSystem;
        }

        public virtual NervousSystem GetNervousSystem(){
            return nervousSystem;
        }

        public void SetDriveSystem(DriveSystem passed){
            driveSystem = passed;
        }

        public void SetActionChoice(ActionChoice passed){
            actionChoice = passed;
        }

        // public void SetFOVDetection(FOVDetection passed){
        //     fovDetection = passed;
        // }

        public void SetMotorSystem(MotorSystem passed){
            motorSystem = passed;
        }

        public void SetNervousSystem(NervousSystem passed){
            nervousSystem = passed;
        }
        
    }
