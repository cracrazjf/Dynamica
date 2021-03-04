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

    public bool doingNothing = true;
    
    /// <summary>
    /// Penguin constructor
    /// </summary>
    public Wolf(int index, Genome motherGenome, Genome fatherGenome): 
            base("Wolf", index, motherGenome, fatherGenome) {

        wolfPrefab = Resources.Load("Prefabs/GreyWolfPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(wolfPrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
        
        animator = this.gameObject.GetComponent<Animator>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void UpdateAnimal(){
        GetDriveSystem().UpdateDrives();
    }
}
