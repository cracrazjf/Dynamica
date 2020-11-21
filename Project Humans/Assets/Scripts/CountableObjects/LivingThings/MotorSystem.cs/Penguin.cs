using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Penguin : Animal
{

    /// <value>Penguin prefab</value>
    public GameObject penguinPrefab;
    public Rigidbody rigidbody;
    public FOVDetection fOVDetection;

    public bool doingNothing = true;
    

    /// <summary>
    /// Penguin constructor
    /// </summary>
    public Penguin(string objectType, Genome motherGenome, Genome fatherGenome): base(objectType, 1.0f, 1.0f, motherGenome, fatherGenome) {

        
        //Instantiate penguinPrefab
        Vector3 startPosition = this.chooseStartPosition();
        Quaternion startRotation = this.chooseStartRotation();

    
        penguinPrefab = Resources.Load("PenguinPrefab",typeof(GameObject)) as GameObject;
        this.gameObject = GameObject.Instantiate(penguinPrefab, startPosition, startRotation) as GameObject;// instantiate 
        this.gameObject.name = GetObjectType();

        gameObject.SetActive(true);
        

        this.gameObject.AddComponent<FOVDetection>();
        animator = this.gameObject.GetComponent<Animator>();
        
        fOVDetection = this.gameObject.GetComponent<FOVDetection>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void UpdateAnimal(){
        this.driveSystem.UpdateDrives();
    }
}
