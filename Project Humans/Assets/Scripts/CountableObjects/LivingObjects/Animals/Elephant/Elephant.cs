using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elephant : Animal
{

    /// <value>Penguin prefab</value>
    public GameObject elephantPrefab;
    public Rigidbody rigidbody;
    public FOVDetection fOVDetection;

    public bool doingNothing = true;
    

    /// <summary>
    /// Elephant constructor
    /// </summary>
    public Elephant(int index, Genome motherGenome, Genome fatherGenome): base("Elephant", index, motherGenome, fatherGenome) {
        Vector3 startPosition = this.chooseStartPosition();
        Quaternion startRotation = this.chooseStartRotation();
        elephantPrefab = Resources.Load("ElephantPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(elephantPrefab, startPosition, startRotation) as GameObject;
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
