using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMotor : MonoBehaviour
{
    public Transform abdomenTrans;
    public Transform bodyTrans;
    public Transform leftEye;
    public Transform rightEye;
    public Transform leftHand;
    public Transform rightHand;
    Vector3 goalPos;
    int footUpdates = 0;
    bool firstRight = true; // change this at some point like handedness
    private float xMin = -1f, xMax = 1f;
    private float timeValue = 0.0f;
    public float rotatedAngle = 0.0f;
    public ConfigurableJoint rightFemur;
    public ConfigurableJoint leftFemur;
    public ConfigurableJoint rightTibia;
    public ConfigurableJoint leftTibia;
    public ConfigurableJoint rightHumerus;
    public ConfigurableJoint leftHumerus;
    public ConfigurableJoint rightRadius;
    public ConfigurableJoint leftRadius;
    public ConfigurableJoint neck;
    public bool setAxis;
    public bool isCrouching;
    public bool reached;
    public bool consumed;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //UseHand(-1, -0.6f, 0, -0.3f);
        //Consume(-1);
        //Crouch();
        //TakingSteps();
        //Rotate(-10);
        //IsCrouching();
        //Sit();
        //Lay();
    }

    public void TakingSteps()
    {
        bodyTrans.Translate(bodyTrans.forward * 2 * Time.deltaTime, Space.World);
    }
    public void Rotate(float angle)
    {
        bodyTrans.Rotate(0, angle * Time.deltaTime, 0, Space.World);
    }
    public void Crouch()
    {
        leftTibia.targetRotation = new Quaternion(-1, 0, 0, 1);
        rightTibia.targetRotation = new Quaternion(-1, 0, 0, 1);
        leftFemur.targetRotation = new Quaternion(0.5f, 0, 0, 1);
        rightFemur.targetRotation = new Quaternion(0.5f, 0, 0, 1);
        
        if (abdomenTrans.localPosition.y > -1.1)
        {
            abdomenTrans.Translate(-abdomenTrans.up * 2 * Time.deltaTime, Space.Self);
        }
        else
        {
            if (abdomenTrans.localRotation.x < 0.5)
            {
                abdomenTrans.Rotate(30 * Time.deltaTime, 0, 0);
            }
            else
            {
                isCrouching = true;
            }
        }
    }
    public void Stand()
    {
        if (abdomenTrans.localRotation.x > 0)
        {
            abdomenTrans.Rotate(-30 * Time.deltaTime, 0, 0);
        }
        leftTibia.targetRotation = new Quaternion(0, 0, 0, 1);
        rightTibia.targetRotation = new Quaternion(0, 0, 0, 1);
        leftFemur.targetRotation = new Quaternion(0, 0, 0, 1);
        rightFemur.targetRotation = new Quaternion(0, 0, 0, 1);
        if (abdomenTrans.localPosition.y < -0.2)
        {
            abdomenTrans.Translate(Vector3.up * 2 * Time.deltaTime, Space.Self);
        }
        
        
    }
    public void UseHand(float hand, float reach_x, float reach_y, float reach_z)
    {
        LayerMask layermask = ~(1 << 8 | 1 << 9);
        
        if (hand > 0)
        {
            rightHumerus.targetRotation = new Quaternion(reach_x, reach_y, reach_z, 1);
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(rightHand.position, 0.5f, hitColliders, layermask);
            if (numColliders > 0)
            {
                hitColliders[0].transform.parent = rightHand;
                rightHand.GetChild(0).localPosition = new Vector3(0, 0, 0);
                reached = true;
            }
        }
        if (hand < 0)
        {
            leftHumerus.targetRotation = new Quaternion(reach_x, reach_y, reach_z, 1);
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(leftHand.position, 0.5f, hitColliders, layermask);
            if (numColliders > 0)
            {
                hitColliders[0].transform.GetComponent<Rigidbody>().isKinematic = true;
                hitColliders[0].transform.GetComponent<CapsuleCollider>().isTrigger = true;
                hitColliders[0].transform.parent = leftHand;
                leftHand.GetChild(1).localPosition = new Vector3(0, 0, 0);
                leftHand.GetChild(1).localRotation = Quaternion.Euler(283.640747f, 84.107254f, 138.337387f);
                reached = true;
            }
        }
    }
    public void PutDownHand(int hand)
    {
        if (hand > 0)
        {
            rightHumerus.targetRotation = new Quaternion(0, 0, 0, 1);
            rightRadius.targetRotation = new Quaternion(0, 0, 0, 1);
        }
        if (hand < 0)
        {
            leftHumerus.targetRotation = new Quaternion(0, 0, 0, 1);
            leftRadius.targetRotation = new Quaternion(0, 0, 0, 1);
        }
    }
    public void Consume(int hand)
    {
        if (hand > 0)
        {
            rightRadius.targetRotation = new Quaternion(2, 0, 1, 1);
            consumed = true;
        }
        else
        {
            leftRadius.targetRotation = new Quaternion(-3, 0, 0, 1);
            consumed = true;
        }
    }
    public void Sit()
    {
        leftFemur.targetRotation = new Quaternion(1.0f, 0, 0, 1);
        rightFemur.targetRotation = new Quaternion(1.0f, 0, 0, 1);
        if (abdomenTrans.localPosition.y > -2.0)
        {
            abdomenTrans.Translate(-Vector3.up * 1 * Time.deltaTime, Space.Self);
        }
    }
    public void Lay()
    {
        
        if (abdomenTrans.localRotation.x >= -0.7)
        {
            abdomenTrans.Rotate(-30 * Time.deltaTime, 0, 0);
        }

        if (abdomenTrans.localPosition.y > -2.5)
        {
            Debug.Log("here");
            abdomenTrans.Translate(transform.right * 1 * Time.deltaTime, Space.Self);
        }
    }
    public void Sleep()
    {
        leftEye.localScale = new Vector3(0.4f, 0.0f, 0.4f);
        rightEye.localScale = new Vector3(0.4f, 0.0f, 0.4f);
    }
}
