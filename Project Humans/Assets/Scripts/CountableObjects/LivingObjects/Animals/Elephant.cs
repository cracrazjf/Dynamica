using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Elephant : Animal
{

    /// <value>Penguin prefab</value>
    public GameObject elephantPrefab;
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    

    /// <summary>
    /// Elephant constructor
    /// </summary>
    public Elephant(int index, Nullable<Vector3> position, Genome motherGenome, Genome fatherGenome): 
            base("Elephant", index, position, motherGenome, fatherGenome) {

        elephantPrefab = Resources.Load("ElephantPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(elephantPrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);

        animator = this.gameObject.GetComponent<Animator>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void UpdateAnimal(){
        GetDriveSystem().UpdateDrives();
    }
}