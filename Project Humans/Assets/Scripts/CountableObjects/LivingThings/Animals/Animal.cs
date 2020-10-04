using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    abstract public class Animal : LivingObject
    {
        public DriveSystem driveSystem;

        public Animal(string objectType, Genome motherGenome, Genome fatherGenome) : base (objectType, motherGenome, fatherGenome)
        {
            this.driveSystem = new DriveSystem(this);
        }

        public Vector3 chooseStartPosition(){
            var startPosition = new Vector3 (Random.Range(World.minPosition,World.maxPosition), 0.03f, Random.Range(World.minPosition,World.maxPosition));
            return startPosition;
        }


    }
