using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Deer : Animal
{
    /// <value>Deer prefab</value>
    public GameObject deerPrefab;
    public Rigidbody rigidbody;
    public FOVDetection fOVDetection;

    public bool doingNothing = true;
    
    /// <summary>
    /// Deer constructor
    /// </summary>
    public Deer(int index, Genome motherGenome, Genome fatherGenome): base("Deer", index, motherGenome, fatherGenome) {
        Vector3 startPosition = this.chooseStartPosition(null);
        Quaternion startRotation = this.chooseStartRotation();
        deerPrefab = Resources.Load("DeerPrefab",typeof(GameObject)) as GameObject;

        this.gameObject = GameObject.Instantiate(deerPrefab, startPosition, startRotation) as GameObject;
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
