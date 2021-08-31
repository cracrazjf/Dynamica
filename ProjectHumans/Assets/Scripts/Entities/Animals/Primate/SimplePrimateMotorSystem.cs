using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System.Linq;
public class SimplePrimateMotorSystem : MotorSystem {
   
    Transform abdomenTrans;
    Transform bodyTrans;
    Transform leftEye;
    Transform rightEye;
    PrimateBody primateBody;
    Vector3 goalPos;
    int footUpdates = 0;
    bool firstRight = true; // change this at some point like handedness
    private float xMin = -1f, xMax = 1f;
    private float timeValue = 0.0f;
    HingeJoint rightFemur;
    HingeJoint leftFemur;
    HingeJoint rightTibia;
    HingeJoint leftTibia;
    HingeJoint rightHumerus;
    HingeJoint leftHumerus;
    HingeJoint rightRadius;
    HingeJoint leftRadius;
    HingeJoint neck;
    


    public SimplePrimateMotorSystem(Animal animal) : base(animal) {
        Debug.Log("A simple ape was born!");
        bodyTrans = thisAnimal.GetGameObject().transform;
        abdomenTrans = thisBody.GetSkeleton("Abdomen").transform;
        leftEye = thisBody.head.transform.GetChild(0).GetChild(1);
        rightEye = thisBody.head.transform.GetChild(0).GetChild(0);
        primateBody = (PrimateBody)thisBody;
        rightFemur = thisBody.GetSkeleton("Femur_R").GetComponent<HingeJoint>();
        leftFemur = thisBody.GetSkeleton("Femur_L").GetComponent<HingeJoint>();
        rightTibia = thisBody.GetSkeleton("Tibia_R").GetComponent<HingeJoint>();
        leftTibia = thisBody.GetSkeleton("Tibia_L").GetComponent<HingeJoint>();
        rightHumerus = thisBody.GetSkeleton("Humerus_R").GetComponent<HingeJoint>();
        leftHumerus = thisBody.GetSkeleton("Humerus_L").GetComponent<HingeJoint>();
        rightRadius = thisBody.GetSkeleton("Radius_R").GetComponent<HingeJoint>();
        leftRadius = thisBody.GetSkeleton("Radius_L").GetComponent<HingeJoint>();
        neck = thisBody.GetSkeleton("Neck").GetComponent<HingeJoint>();
        rightHand = thisBody.GetSkeleton("Hand_R").transform;
        leftHand = thisBody.GetSkeleton("Hand_L").transform;
    }
    public override void Consume() {
        if(stateDict["active"] == 1)
        {
            JointSpring humerusHingeSpring = rightHumerus.spring;
            JointSpring radiusHingeSpring = rightRadius.spring;
            humerusHingeSpring.targetPosition = -80;
            rightHumerus.spring = humerusHingeSpring;
            radiusHingeSpring.targetPosition = -130;
            rightRadius.spring = radiusHingeSpring;
        }
        else
        {
            JointSpring humerusHingeSpring = leftHumerus.spring;
            JointSpring radiusHingeSpring = leftRadius.spring;
            humerusHingeSpring.targetPosition = 80;
            leftHumerus.spring = humerusHingeSpring;
            radiusHingeSpring.targetPosition = 130;
            leftRadius.spring = radiusHingeSpring;
        }
    }

    public override void TakeSteps()
    {
        Debug.Log("taking steps");
        float stepRange = stateDict["take steps"] * thisAnimal.GetPhenotype().GetTrait("max_step");
        bodyTrans.Translate(bodyTrans.forward * stepRange * Time.deltaTime, Space.World);
    }

    public override void Rotate()
    {
        float degree = stateDict["rotate"];
        float rotatingSpeed = degree * thisAnimal.GetPhenotype().GetTrait("max_rotation");
        bodyTrans.Rotate(0, rotatingSpeed * Time.deltaTime, 0, Space.Self);
    }
    public override void Crouch() {
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
    public override void Stand()
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
    public override void UseHand()
    {
        float reach_x = stateDict["RP x"];
        float reach_z = stateDict["RP z"];
        LayerMask layermask = ~(1 << 8 | 1 << 9);
        if (stateDict["active right"] != 0)
        {
            if (!setAxis)
            {
                Vector3 axis = new Vector3(reach_x, 0, reach_z);
                rightHumerus.axis = axis;
                setAxis = true;
            }
            JointSpring humerusHingeSpring = rightHumerus.spring;
            humerusHingeSpring.targetPosition = stateDict["right"] * 180;
            rightHumerus.spring = humerusHingeSpring;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = 0;//Physics.OverlapSphereNonAlloc(rightHand.position, 0.5f, hitColliders, layermask);
            if (numColliders > 0)
            {
                hitColliders[0].transform.parent = rightHand;
                rightHand.GetChild(0).localPosition = new Vector3(0, 0, 0);
                reached = true;
            }
        }
        if (stateDict["active left"] != 0)
        {
            if (!setAxis)
            {
                Vector3 axis = new Vector3(reach_x, 0, reach_z);
                leftHumerus.axis = axis;
                setAxis = true;
            }
            JointSpring humerusHingeSpring = leftHumerus.spring;
            humerusHingeSpring.targetPosition = stateDict["active left"] * 180;
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
    public override void Lay() {
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
            abdomenTrans.Translate(thisAnimal.GetGameObject().transform.right * 1 * Time.deltaTime, Space.Self);
        }
    }

    public override void Look() {
        if(stateDict["look horizontally"] != 0)
        {
            JointSpring headSpring = neck.spring;
            neck.axis.Set(0, 1, 0);
            headSpring.targetPosition = stateDict["look horizontally"] * 90;
            neck.spring = headSpring;

        }
        if(stateDict["look vertically"] != 0)
        {
            JointSpring headSpring = neck.spring;
            neck.axis.Set(1, 0, 0);
            headSpring.targetPosition = stateDict["look vertically"] * 90;
            neck.spring = headSpring;
        }
    }

    public override void Rest() {
        Sit();
    }

    public override void Sit() {
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

    public override void Sleep() {
        leftEye.localScale = new Vector3(0.4f, 0.0f, 0.4f);
        rightEye.localScale = new Vector3(0.4f, 0.0f, 0.4f);
    }



    

    
}