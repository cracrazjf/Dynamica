using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Penguin : Animal
{
    /// <value>Penguin prefab</value>
    public GameObject penguinPrefab;
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    
    /// <summary>
    /// Penguin constructor
    /// </summary>
    public Penguin(int index, Genome motherGenome, Genome fatherGenome): 
            base("Penguin", index, motherGenome, fatherGenome) {

        penguinPrefab = Resources.Load("Prefabs/PenguinPrefab",typeof(GameObject)) as GameObject;
        this.gameObject = GameObject.Instantiate(penguinPrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();
        SetDriveSystem(new DriveSystem(this));

        gameObject.SetActive(true);
        animator = this.gameObject.GetComponent<Animator>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void UpdateAnimal(){
        this.GetDriveSystem().UpdateDrives();
    }
}
