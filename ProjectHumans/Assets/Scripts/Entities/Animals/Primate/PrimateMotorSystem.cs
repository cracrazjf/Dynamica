using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System.Linq;
public class PrimateMotorSystem : MotorSystem
{
    Transform abdomenTrans;
    PrimateBody primateBody;
    Vector3 goalPos;
    int footUpdates = 0;
    bool rightStep = true; // change this at some point like handedness
    private float xMin = -1.0f, xMax = 1.0f;
    private float timeValue = 0.0f;

    public PrimateMotorSystem(Animal animal) : base(animal) {
        Debug.Log("An ape was born!");
        abdomenTrans = thisBody.abdomen.transform;
        primateBody = (PrimateBody)thisBody;
        StandUp();
    }
    
    public override void Crouch() {
        if (stateDict["crouch"] == -1f) {
            CrouchDown(); }
        else if (stateDict["crouch"] == 1f) {
            SitUp();
        } else { Rest(); }
    }

    public override void Sit() {
        if (stateDict["sit"] == -1f) {
            SitDown();
        } else { SitUp(); }
    }

    public override void Lay() {
        if (stateDict["lay"] == -1f) {
            LayDown();
        } else if (stateDict["lay"] == 1f) {
            SitUp();
        } else { Rest(); }
    }

    public override void Stand() {
        if (stateDict["stand"] == 1f) {
            StandUp();
        } else if (stateDict["stand"] == -1f) {
            SitDown();
        } else { Rest(); }
    }
    public override void Rotate() {
        float degree = stateDict["rotate"] * 0.5f;
        float rotatingSpeed = degree * thisAnimal.GetPhenotype().GetTrait("max_rotation");

        thisBody.globalPos.Rotate(0, rotatingSpeed, 0, Space.World);
    }
    public override void TakeSteps() {
        float direction = stateDict["take steps"];
        float stepProportion = direction * thisAnimal.GetPhenotype().GetTrait("max_step") * 0.5f;

        float crouchAdjust = thisBody.GetTargetRotation("Femur_R").x;
        float xQ = Mathf.Lerp(xMin, xMax, timeValue) + crouchAdjust/90;
        timeValue += 0.5f * Time.deltaTime;

        if (timeValue > 1.0f) {
            float temp = xMax;
            xMax = xMin;
            xMin = temp;
            
            timeValue = 0.0f;
        }

        thisBody.RotateJointTo("Femur_R", new Quaternion(xQ, 0, 0, 1));
        thisBody.RotateJointTo("Femur_L", new Quaternion(-xQ, 0, 0, 1));

        if (footUpdates < 500) {
            LegUp(rightStep);
            footUpdates++;
        } else {
            footUpdates = 0;
            rightStep = !rightStep;
        }

        // Super important to move parts of the body, not the whole gameObject. 
        abdomenTrans.Translate(abdomenTrans.forward * stepProportion * Time.deltaTime);
    }
    public override void UseHand() {
        GameObject holder = GetActiveHand();

        if (stateDict["hand action"] == 1f) { 
            PickUp();
        } else { DropArm(); }
    }
    public override void Consume() {
        Debug.Log("Tried to eat something");
        GameObject holder = GetActiveHand();

        if (ArmTo(thisBody.head.transform.position)) {
            thisBody.EatObject(holder.name);
            DropArm();
        }
    }
    public override void Sleep() {
        if (stateDict["consume"] == 1f) {
            FallAsleep();
        } else { WakeUp(); }
    }

    public override void Rest() {
        Debug.Log("Resting");
        thisBody.RestAdjust();
    }

    public override void Look() {
        if (stateDict["look"] == 1f) {
            LookAt();
        } else { LookForward(); }
    }

    // BEGIN HELPER FUNCTIONS
    void SitDown() {
        if (((PrimateBody)thisBody).CheckCrouchingTop()) {
            Vector3 toSend = abdomenTrans.position;
            double sitHeight = thisBody.GetHeight() / 2.25;
            Debug.Log("Checking to see if more " + toSend.y + " " + sitHeight);
            if (toSend.y > sitHeight) {
                BendWaist(50f);
                BendLegs(50f, -40f);
                abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));
            } else {
                thisBody.EnsureKinematic("Abdomen");
                thisBody.FreeRotation("Hips");
                Debug.Log("I think I'm sitting");
                thisBody.SetState("sitting", 1f);
            }
        } else { CrouchDown(); }
    }
    void SitUp() {
        if (thisBody.CheckLaying()) {
            Debug.Log("Trying condition one");
            BendLegs(45f, 0f);
            BendWaist(50f);
            if (abdomenTrans.position.y < .5f) {
                thisBody.EnsureKinematic("Abdomen");
                abdomenTrans.Translate(Vector3.up * (Time.deltaTime * .5f));
            }
        } else {
            Debug.Log("Trying condition two"); 
            BendWaist(1f);
        }
    }

    void LayDown() {
        Debug.Log("Tried to lay down");

        if (thisAnimal.GetBodyState("sitting")) {
            if (!primateBody.CheckLaying()) {
                ExtendLegs();
            }
            Collapse();
        } else { SitDown(); }
    }
    
    void StandUp() {
        Vector3 toSend = abdomenTrans.localPosition;

        if (Math.Pow(toSend.y - 0f, 2) > 0.05 ) {
            if (toSend.y < 0) {
                abdomenTrans.Translate(Vector3.up * (Time.deltaTime) * .5f);
            } else {
                abdomenTrans.Translate(Vector3.up * (Time.deltaTime) * -.5f);
            } 
        } else {
            Debug.Log("I think I'm standing");
            thisBody.SetState("standing", 1f);
        }
    }
    
    void PickUp() {
        Debug.Log("Tried to pick something up");
        GameObject holder = GetActiveHand();

        if (ArmToGoal()) {
            GrabWithHolder(holder);
            DropArm();
        }
    }
    
    void SetDown() {
        Debug.Log("Tried to set something down");
        GameObject holder = GetActiveHand();
        if (thisBody.CheckSitting()) {
            RemoveFromHolder(holder);
        } else { KneelDown(); }
    }
    // Functional
    void WakeUp() {
        Debug.Log("Waking up");
        ToggleEyes(true);
        thisBody.SetState("sleeping", -1f);
    }

    void FallAsleep() {
        Debug.Log("Sleeping");
        ToggleEyes(false);

        if (thisBody.CheckLaying()) {
            thisBody.SetState("sleeping", 1f);
            thisBody.SleepAdjust();
        } else { LayDown(); }
    }

    // Functional (but hated)
    void LookForward() {
        Quaternion toSend = Quaternion.LookRotation(thisBody.globalPos.forward);
        thisBody.RotateJointTo("Head", toSend);
    }

   // Need to look at active hand
    void LookAt() {
        GameObject holder = GetActiveHand();
        Vector3 direction = (holder.transform.position - thisBody.head.transform.position).normalized;
        Quaternion toSend = Quaternion.LookRotation(direction);
        thisBody.RotateJointTo("Head", toSend);
    }
    
    bool ArmTo(Vector3 targetPos) {
        string arm = GetActiveHand().name;
        thisBody.EnsureKinematic(arm);
        Transform armTrans = thisBody.GetSkeletonDict()[arm].transform;
        armTrans.position = Vector3.Slerp(armTrans.position, targetPos, Time.deltaTime);
        return (armTrans.position == targetPos);
    }
    // Functional
    void BendKnees(float degree) {
        Quaternion toSend = new Quaternion(degree, 0, 0, 0);
        thisBody.RotateJointTo("Tibia_L", toSend);
        thisBody.RotateJointTo("Tibia_R", toSend);
    }
    void LegUp(bool right) {
        Quaternion forward = new Quaternion(30, 0, 0, 0);
        Quaternion back = new Quaternion(-30, 0, 0, 0);

        if (right) {
            thisBody.RotateJointBy("Tibia_L", back);
            thisBody.RotateJointBy("Tibia_R", forward);
        } else {
            thisBody.RotateJointBy("Tibia_L", forward);
            thisBody.RotateJointBy("Tibia_R", back);
        }
    }
    
    void BendLegs(float xDegree, float yDegree) {
        Quaternion sendLeft = new Quaternion(xDegree, yDegree, 0, 0);
        Quaternion sendRight = new Quaternion(xDegree, -yDegree, 0, 0);
        thisBody.RotateJointTo("Femur_L", sendLeft);
        thisBody.RotateJointTo("Femur_R", sendRight);
    }

    void BendWaist(float xDegree) {
        Quaternion toSend = new Quaternion(xDegree, 0, 0, 0);
        thisBody.RotateJointTo("Hips", toSend);
    }

    bool ArmToGoal() {
        Vector3 abAdj = primateBody.abAdjLeft;
        string arm = GetActiveHand().name;
        if (stateDict["active right"] == 1f) {
            abAdj = primateBody.abAdjRight;
        }

        float xTrans = stateDict["RP x"] * thisBody.xMax;
        float yTrans = stateDict["RP y"] * thisBody.yMax;
        float zTrans = stateDict["RP z"] * thisBody.zMax;

        Vector3 toAdd = new Vector3(.5f, -.5f, 1f);
        // need to attach .abs to these
        if (xTrans <= 1f && yTrans <= 1f && zTrans <= 1f) {
            toAdd = new Vector3(xTrans, yTrans, zTrans);
        } else {
             Debug.Log("Reach out of bounds!");
        }
        
        Transform armTrans = GetActiveHand().transform;
        Vector3 goalPos = abdomenTrans.position + abAdj + toAdd;

        thisBody.EnsureKinematic(armTrans.name);
        armTrans.position = Vector3.Slerp(armTrans.position, goalPos, Time.deltaTime);
        return (armTrans.position == goalPos);
    }
    // Functional
    void DropArm() {
        string arm = GetActiveHand().name;
        thisBody.DisableKinematic(arm);
    }
    
    void KneelDown() {
        Vector3 toSend = abdomenTrans.position;
        if (toSend.y > thisBody.GetHeight() / 2 + 0.5) {
            BendLegs(30f, 0f);
            BendKnees(-45f);
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -1));
        } else { Debug.Log("I think I'm kneeling"); }
    }

    void CrouchDown() {
        Debug.Log("Crouch was called");
        if (((PrimateBody)thisBody).CheckCrouchingBottom()) {
            BendLegs(-1f, 0f);
            BendLegs(80f, 0f);
            BendKnees(-30f);
            abdomenTrans.Translate(Vector3.up * (Time.deltaTime * -0.75f));
            BendWaist(-1f);
            BendWaist(15f);
        } else { thisBody.SetState("crouching", 1f); }
    }
    
    void ExtendLegs() {
        Debug.Log("Stretching my toes");

        Vector3 goalR = abdomenTrans.position + primateBody.footAdjRight;
        Vector3 goalL = abdomenTrans.position + primateBody.footAdjLeft;

        thisBody.SlerpTargetTo("Foot_R", goalR);
        thisBody.SlerpTargetTo("Foot_L", goalL);
    }

    void Collapse() {
        Debug.Log("Collapsing");
        thisBody.DisableKinematic("Abdomen");
    }

    void GrabWithHolder(GameObject holder) {
        Debug.Log("Tried to affix something");
        Vector3 forCollider = holder.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(forCollider, .025f);
        Rigidbody toConnect = holder.GetComponent<Rigidbody>();

        foreach (var hit in hitColliders) {
            if (!thisBody.GetSkeletonDict().ContainsKey(hit.gameObject.name)) {
                if (CheckMovableObject(hit.gameObject)) {
                    FixedJoint newJoint = hit.gameObject.AddComponent<FixedJoint>() as FixedJoint;
                    newJoint.connectedBody = toConnect;
                } else { Debug.Log("Object too big to lift!"); }
            } else { Debug.Log("Nothing to pick up!"); }
        }
    }

    void RemoveFromHolder(GameObject holder) {
        string holderName = holder.name;
        if (thisBody.GetHeld(holderName) != null) { thisBody.RemoveObject(holderName); }
    }

    bool CheckMovableObject(GameObject toLift) {
        float liftMass = toLift.GetComponent<Rigidbody>().mass;
        return (liftMass < thisBody.rigidbody.mass);
    }

    GameObject GetActiveHand() {
        if (stateDict["active right"] == 1f) {
            return thisBody.GetSkeleton("Hand_R");
        } else { return thisBody.GetSkeleton("Hand_L"); }
    }

    void ToggleEyes(bool open) {
        thisBody.GetSkeleton("Eye_R").SetActive(open);
        thisBody.GetSkeleton("Eye_L").SetActive(open);
    }
}
