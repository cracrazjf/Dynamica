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
    public Penguin(int index, Genome motherGenome, Genome fatherGenome): base("Penguin", index, motherGenome, fatherGenome) {
        Vector3 startPosition = this.chooseStartPosition(null);
        Quaternion startRotation = this.chooseStartRotation();
        penguinPrefab = Resources.Load("PenguinPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(penguinPrefab, startPosition, startRotation) as GameObject;
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
