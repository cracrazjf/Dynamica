using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Test : MonoBehaviour
{
    public float initialVelocity = 0.0f;
    public float finalVelocity = 50.0f;
    public float currentVelocity = 0.0f;
    public float accelerationRate = 10.0f;
    public Animator animator;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        Accelerate(); 
        
    }
    void Accelerate()
    {
        gameObject.transform.Translate(0,0,5 *Time.deltaTime);




    }
    
}

