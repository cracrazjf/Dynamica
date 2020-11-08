using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    abstract public class Animal : LivingObject
    {
        public DriveSystem driveSystem;
        
        private string name;
        private string displayName;

        public Animal(string objectType, Genome motherGenome, Genome fatherGenome) : base (objectType, motherGenome, fatherGenome)
        {
            this.name = objectType;
            this.displayName = this.name;
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

        public void UpdateAnimal() {
            Debug.Log("No update defined");
        }

    }
