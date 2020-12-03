using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Wolf : Animal
{

    /// <value>Wolf prefab</value>
    public GameObject wolfPrefab;
    public Rigidbody rigidbody;
    public FOVDetection fOVDetection;

    public bool doingNothing = true;
    
    /// <summary>
    /// Penguin constructor
    /// </summary>
    public Wolf(int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome): 
            base("Wolf", index, position, motherGenome, fatherGenome) {

        wolfPrefab = Resources.Load("GreyWolfPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(wolfPrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
        

        this.gameObject.AddComponent<FOVDetection>();
        animator = this.gameObject.GetComponent<Animator>();
        
        fOVDetection = this.gameObject.GetComponent<FOVDetection>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void UpdateAnimal(){
        GetDriveSystem().UpdateDrives();
    }
}
