using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMotor : MonoBehaviour
{
    public Transform abdomenTrans;
    public Transform bodyTrans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TakingSteps();
        //Rotate();
    }

    void TakingSteps()
    {
        bodyTrans.Translate(bodyTrans.forward * 2 * Time.deltaTime, Space.World);
    }
    void Rotate()
    {
        bodyTrans.Rotate(0, 10 * Time.deltaTime, 0, Space.World);
    }
    
}
