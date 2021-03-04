using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Llama : Animal
{
    /// <value>Llama prefab</value>
    public GameObject llamaPrefab;
    public Rigidbody rigidbody;

    public bool doingNothing = true;
    
    /// <summary>
    /// Llama constructor
    /// </summary>
    public Llama(int index, Genome motherGenome, Genome fatherGenome): 
            base("Llama", index, motherGenome, fatherGenome) {

        llamaPrefab = Resources.Load("Prefabs/LlamaPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(llamaPrefab, startPosition, startRotation) as GameObject;
        this.gameObject.name = GetName();

        gameObject.SetActive(true);
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void UpdateAnimal(){
        //llama falls over. good llama.
    }
}