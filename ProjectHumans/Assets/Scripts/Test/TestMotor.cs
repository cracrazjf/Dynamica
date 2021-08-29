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
    public HingeJoint rightFemur;
    public HingeJoint leftFemur;
    public HingeJoint rightTibia;
    public HingeJoint leftTibia;
    public HingeJoint rightHumerus;
    public HingeJoint leftHumerus;
    public HingeJoint rightRadius;
    public HingeJoint leftRadius;
    public HingeJoint neck;
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
        //Crouch();
        //TakingSteps();
        //Rotate(-10);
        //IsCrouching();
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
        JointSpring femurHingeSpring = rightFemur.spring;
        JointSpring tibiaHingeSpring = rightTibia.spring;
        femurHingeSpring.targetPosition = -120;
        rightFemur.spring = femurHingeSpring;
        leftFemur.spring = femurHingeSpring;
        tibiaHingeSpring.targetPosition = 160;
        rightTibia.spring = tibiaHingeSpring;
        leftTibia.spring = tibiaHingeSpring;
        if (abdomenTrans.localPosition.y > -1.3)
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
        if (abdomenTrans.localPosition.y < -0.25)
        {
            abdomenTrans.Translate(Vector3.up * 2 * Time.deltaTime, Space.Self);
        }
        if (abdomenTrans.localRotation.x > 0)
        {
            abdomenTrans.Rotate(-30 * Time.deltaTime, 0, 0);
        }
        JointSpring femurHingeSpring = rightFemur.spring;
        JointSpring tibiaHingeSpring = rightTibia.spring;
        femurHingeSpring.targetPosition = 0;
        rightFemur.spring = femurHingeSpring;
        leftFemur.spring = femurHingeSpring;
        tibiaHingeSpring.targetPosition = 0;
        rightTibia.spring = tibiaHingeSpring;
        leftTibia.spring = tibiaHingeSpring;
    }
    public void UseHand(float hand, float reach_x, float reach_z, float angle)
    {
        LayerMask layermask = ~(1 << 8 | 1 << 9);
        if (hand > 0)
        {
            if (!setAxis)
            {
                Vector3 axis = new Vector3(reach_x, 0, reach_z);
                rightHumerus.axis = axis;
                setAxis = true;
            }
            JointSpring humerusHingeSpring = rightHumerus.spring;
            humerusHingeSpring.targetPosition = angle * 180;
            rightHumerus.spring = humerusHingeSpring;
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
            if (!setAxis)
            {
                Vector3 axis = new Vector3(reach_x, 0, reach_z);
                leftHumerus.axis = axis;
                setAxis = true;
            }
            JointSpring humerusHingeSpring = rightHumerus.spring;
            humerusHingeSpring.targetPosition = angle * 180;
            leftHumerus.spring = humerusHingeSpring;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(leftHand.position, 0.5f, hitColliders, layermask);
            if (numColliders > 0)
            {
                hitColliders[0].transform.GetComponent<Rigidbody>().isKinematic = true;
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
            Vector3 axis = new Vector3(0, 0, 0);
            rightHumerus.axis = axis;
            setAxis = false;
            JointSpring humerusHingeSpring = rightHumerus.spring;
            humerusHingeSpring.targetPosition = 0;
            rightHumerus.spring = humerusHingeSpring;
        }
        if (hand < 0)
        {
            Vector3 axis = new Vector3(0, 0, 0);
            leftHumerus.axis = axis;
            setAxis = false;
            JointSpring humerusHingeSpring = leftHumerus.spring;
            humerusHingeSpring.targetPosition = 0;
            leftHumerus.spring = humerusHingeSpring;
        }
    }
    public void Consume(int hand)
    {
        if(hand > 0)
        {
            JointSpring humerusHingeSpring = rightHumerus.spring;
            JointSpring radiusHingeSpring = rightRadius.spring;
            humerusHingeSpring.targetPosition = -50;
            rightHumerus.spring = humerusHingeSpring;
            radiusHingeSpring.targetPosition = -130;
            rightRadius.spring = radiusHingeSpring;
            consumed = true;
        }
        else
        {
            Vector3 axis = new Vector3(1, 0, 1);
            leftHumerus.axis = axis;
            JointSpring humerusHingeSpring = leftHumerus.spring;
            JointSpring radiusHingeSpring = leftRadius.spring;
            humerusHingeSpring.targetPosition = 30;
            leftHumerus.spring = humerusHingeSpring;
            radiusHingeSpring.targetPosition = 130;
            leftRadius.spring = radiusHingeSpring;
            consumed = true;
        }
    }
    public void Sit()
    {
        JointSpring femurHingeSpring = rightFemur.spring;
        JointSpring tibiaHingeSpring = rightTibia.spring;
        femurHingeSpring.targetPosition = -150;
        rightFemur.spring = femurHingeSpring;
        leftFemur.spring = femurHingeSpring;
        tibiaHingeSpring.targetPosition = 15;
        rightTibia.spring = tibiaHingeSpring;
        leftTibia.spring = tibiaHingeSpring;
        if (abdomenTrans.localPosition.y > -2.0)
        {
            abdomenTrans.Translate(-Vector3.up * 1 * Time.deltaTime, Space.Self);
        }
    }

    public void Lay()
    {
        JointSpring femurHingeSpring = rightFemur.spring;
        JointSpring tibiaHingeSpring = rightTibia.spring;
        femurHingeSpring.targetPosition = -50;
        rightFemur.spring = femurHingeSpring;
        leftFemur.spring = femurHingeSpring;
        tibiaHingeSpring.targetPosition = -100;
        rightTibia.spring = tibiaHingeSpring;
        leftTibia.spring = tibiaHingeSpring;
        //abdomenTrans.localPosition = new Vector3(0, -0.84f, 0);
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
