using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Deer : Animal
{
    /// <value>Deer prefab</value>
    public GameObject deerPrefab;
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    
    /// <summary>
    /// Deer constructor
    /// </summary>
    public Deer(int index, Genome motherGenome, Genome fatherGenome): 
            base("Deer", index, motherGenome, fatherGenome) {

        deerPrefab = Resources.Load("Prefabs/DeerPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(deerPrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
        animator = this.gameObject.GetComponent<Animator>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void UpdateAnimal(){
        GetDriveSystem().UpdateDrives();
    }
}
