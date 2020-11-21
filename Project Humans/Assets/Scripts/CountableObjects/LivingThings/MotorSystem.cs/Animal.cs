using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    abstract public class Animal : LivingObject
    {
        public DriveSystem driveSystem;
        
        public string displayName;

        public Animal(string objectType, float nutrition, float healthEffect, Genome motherGenome, Genome fatherGenome) 
        : base (objectType, nutrition, healthEffect, motherGenome, fatherGenome)
        {

            this.displayName = GetObjectType();
            this.driveSystem = new DriveSystem(this);
        }

        public Vector3 chooseStartPosition(){
            var startPosition = new Vector3 (Random.Range(World.minPosition,World.maxPosition), 0.03f, Random.Range(World.minPosition,World.maxPosition));
            return startPosition;
        }

        public Quaternion chooseStartRotation(){
            var startRotation = Quaternion.Euler(0.0f, Random.Range(World.minPosition,World.maxPosition), 0.0f);
            return startRotation;
        }

        public string GetDisplayName(){
            return displayName;
        }

        public DriveSystem GetDrive(){
            return driveSystem;
        }

        public virtual void UpdateAnimal(){
            Debug.Log("No update defined for this animal");
        }

        public virtual void GetMotorSystem(){
            Debug.Log("No update defined for this animal");
        }
    }
