using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System.Linq;
public class SimpleMotorSystem : MotorSystem
{
    Transform abdomenTrans;
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
    HingeJoint head;

    public SimpleMotorSystem(Animal animal) : base(animal) {
        Debug.Log("A simple ape was born!");
        abdomenTrans = thisBody.abdomen.transform;
        leftEye = thisBody.head.transform.GetChild(1).GetChild(1);
        rightEye = thisBody.head.transform.GetChild(1).GetChild(0);
        primateBody = (PrimateBody)thisBody;
        rightFemur = thisBody.GetSkeleton("Femur_R").GetComponent<HingeJoint>();
        leftFemur = thisBody.GetSkeleton("Femur_L").GetComponent<HingeJoint>();
        rightTibia = thisBody.GetSkeleton("Tibia_R").GetComponent<HingeJoint>();
        leftTibia = thisBody.GetSkeleton("Tibia_L").GetComponent<HingeJoint>();
        rightHumerus = thisBody.GetSkeleton("Humerus_R").GetComponent<HingeJoint>();
        leftHumerus = thisBody.GetSkeleton("Humerus_L").GetComponent<HingeJoint>();
        rightRadius = thisBody.GetSkeleton("Radius_R").GetComponent<HingeJoint>();
        leftRadius = thisBody.GetSkeleton("Radius_L").GetComponent<HingeJoint>();
        head = thisBody.head.GetComponent<HingeJoint>();
    }

    public override void Consume()
    {
        if(stateDict["consume"] == 1)
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

    public override void Crouch()
    {
        if (abdomenTrans.localPosition.y > thisAnimal.GetPhenotype().GetTrait("default_max_reach_y"))
        {
            abdomenTrans.Translate(-abdomenTrans.up * 5 * Time.deltaTime, Space.Self);
        }
        else
        {
            if (abdomenTrans.localRotation.x < 0.5)
            {
                abdomenTrans.Rotate(30 * Time.deltaTime, 0, 0);
            }
        }
        JointSpring femurHingeSpring = rightFemur.spring;
        JointSpring tibiaHingeSpring = rightTibia.spring;
        femurHingeSpring.targetPosition = -120;
        rightFemur.spring = femurHingeSpring;
        leftFemur.spring = femurHingeSpring;
        tibiaHingeSpring.targetPosition = 160;
        rightTibia.spring = tibiaHingeSpring;
        leftTibia.spring = tibiaHingeSpring;

        
    }
        

    public override void Lay()
    {
        JointSpring femurHingeSpring = rightFemur.spring;
        JointSpring tibiaHingeSpring = rightTibia.spring;
        femurHingeSpring.targetPosition = -80;
        rightFemur.spring = femurHingeSpring;
        leftFemur.spring = femurHingeSpring;
        tibiaHingeSpring.targetPosition = -100;
        rightTibia.spring = tibiaHingeSpring;
        leftTibia.spring = tibiaHingeSpring;
        abdomenTrans.localPosition = new Vector3(0, -0.84f, 0);
        if (abdomenTrans.localRotation.x >= -0.7)
        {
            abdomenTrans.Rotate(-30 * Time.deltaTime, 0, 0);
        }
        else
        {
            abdomenTrans.localPosition = new Vector3(0, -2.5f, 0);
        }

    }

    public override void Look()
    {
        if(stateDict["look horizontally"] != 0)
        {
            Debug.Log("here");
            JointSpring headSpring = head.spring;
            head.axis = new Vector3(0, 1, 0);
            //headSpring.targetPosition = 90;
            //head.spring = headSpring;

        }
        if(stateDict["look vertically"] != 0)
        {
            JointSpring headSpring = head.spring;
            head.axis = new Vector3(1, 0, 0);
            headSpring.targetPosition = stateDict["look vertically"] * 180;
            head.spring = headSpring;
        }
    }

    public override void Rest()
    {
        Sit();
    }

    public override void Rotate() {
        float degree = stateDict["rotate"];
        float rotatingSpeed = degree * thisAnimal.GetPhenotype().GetTrait("max_rotation");
        abdomenTrans.Rotate(0, 10 * Time.deltaTime, 0, Space.Self);
    }

    public override void Sit()
    {
        JointSpring femurHingeSpring = rightFemur.spring;
        JointSpring tibiaHingeSpring = rightTibia.spring;
        femurHingeSpring.targetPosition = -180;
        rightFemur.spring = femurHingeSpring;
        leftFemur.spring = femurHingeSpring;
        tibiaHingeSpring.targetPosition = 120;
        rightTibia.spring = tibiaHingeSpring;
        leftTibia.spring = tibiaHingeSpring;
        abdomenTrans.localPosition = new Vector3(0, -2.0f, 0);
    }

    public override void Sleep()
    {
        leftEye.localScale = new Vector3(0.4f, 0.0f, 0.4f);
        rightEye.localScale = new Vector3(0.4f, 0.0f, 0.4f);
    }

    public override void Stand()
    {
        if(thisBody.GetState("crouching") == 1)
        {
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
            abdomenTrans.localPosition = new Vector3(0, 0, 0);
        }
        else if (thisBody.GetState("sitting") == 1)
        {
            JointSpring femurHingeSpring = rightFemur.spring;
            JointSpring tibiaHingeSpring = rightTibia.spring;
            femurHingeSpring.targetPosition = 0;
            rightFemur.spring = femurHingeSpring;
            leftFemur.spring = femurHingeSpring;
            tibiaHingeSpring.targetPosition = 0;
            rightTibia.spring = tibiaHingeSpring;
            leftTibia.spring = tibiaHingeSpring;
            abdomenTrans.localPosition = new Vector3(0, 0, 0);
        }
    }

    public override void TakeSteps() {
        float stepRange = stateDict["take steps"] * thisAnimal.GetPhenotype().GetTrait("max_step");
        abdomenTrans.Translate(abdomenTrans.forward * stepRange * Time.deltaTime, Space.Self);
    }

    public override void UseHand()
    {
        float reach_x = stateDict["RP x"] * thisAnimal.GetPhenotype().GetTrait("default_max_reach_x");
        float reach_z = stateDict["RP z"] * thisAnimal.GetPhenotype().GetTrait("default_max_reach_z");
        Vector3 direction = new Vector3(1, 0, 0);
        if(stateDict["right"] != 0)
        {
            rightHumerus.axis = direction;
            JointSpring humerusHingeSpring = rightHumerus.spring;
            humerusHingeSpring.targetPosition = stateDict["right"] * 180;
            rightHumerus.spring = humerusHingeSpring;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(thisBody.GetSkeleton("Hand_R").transform.position, 0.5f, hitColliders);
            if (numColliders > 0)
            {
                hitColliders[0].transform.parent = thisBody.GetSkeleton("Hand_R").transform;
                thisBody.GetSkeleton("Hand_R").transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            }
        }
        if(stateDict["left"] != 0)
        {
            leftHumerus.axis = direction;
            JointSpring humerusHingeSpring = rightHumerus.spring;
            humerusHingeSpring.targetPosition = stateDict["left"] * 180;
            leftHumerus.spring = humerusHingeSpring;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(thisBody.GetSkeleton("Hand_R").transform.position, 0.5f, hitColliders);
            if (numColliders > 0)
            {
                hitColliders[0].transform.parent = thisBody.GetSkeleton("Hand_L").transform;
                thisBody.GetSkeleton("Hand_L").transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}