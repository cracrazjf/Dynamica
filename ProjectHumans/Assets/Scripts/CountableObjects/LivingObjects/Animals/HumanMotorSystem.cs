using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanMotorSystem : MotorSystem
{
    private Human thisHuman;
    float max_rotation_speed = 1;
    float maxStepDistance = 2;
    
    protected bool[] states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, bool> stateDict;
    
    protected float[] actionsArgs;
    protected List<string> argsLabelList;
    protected Dictionary<string, int> argsIndexDict;
    protected Dictionary<string, float> actionArgsDict;

    public float velocity = 0;

    public HumanMotorSystem(Human human) : base(human) {
        this.thisHuman = human;
        this.stateLabelList = new List<string> {
            // Originally actionStates, will be read-in eventually

            "sitting down", 
            "sitting up", 
            "laying down",
            "standing up", 
            "rotating", 
            "taking steps",
            "picking up", 
            "setting down",
            "eating", 
            "drinking",
            "waking up",
            "falling asleep"
        };
        this.InitStates(this.stateLabelList);

        this.argsLabelList = new List<string> {
            // Originally actionArguments, will be read-in eventually

            "movement velocity",
            "step rate",                          
            "rotation velocity",               
            "hand",
            "hand target x",
            "hand target y",
            "hand target z"
        };
        this.InitActionArguments(this.argsLabelList);
    }

    // Is this perhaps evidence we need only body or motorSystem, but not both? Most of these functions should ideally be in AI...
    public override void UpdateActionStates() {}

    public override void EndAction(string actionLabel) {
        this.SetState(actionLabel, false);
    }

    public void TakeSteps(float stepProportion) {
        if (stepProportion != 0) {
            this.SetState("taking steps", true);
            Vector3 temp = new Vector3(0, 0, stepProportion);
            thisHuman.gameObject.transform.position += temp;
        }
    }

    public void Rotate(float rotatingSpeed) {
        if (rotatingSpeed != 0) {
            this.SetState("rotating", true);
            this.thisHuman.gameObject.transform.Rotate(0, rotatingSpeed, 0, Space.World);
        }
    }

    public void Drink() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.SetState("drinking", true);
        this.thisHuman.GetDriveSystem().SetState("thirst", 0);
    }
    
    public void SitDown() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.SetState("sitting down", true);
        this.thisHuman.GetBody().SetState("standing", false);
        this.thisHuman.GetBody().SetState("sitting", true);                     
    }

    public void SitUp() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.SetState("sitting up", true);
        this.thisHuman.GetBody().SetState("laying", false);
        this.thisHuman.GetBody().SetState("sitting", true);          
    }
        
    public void StandUp() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.SetState("standing up", true);
        this.thisHuman.GetBody().SetState("sitting", false);
        this.thisHuman.GetBody().SetState("standing", true);  
    }

    public void LayDown() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.SetState("laying down", true);
        this.thisHuman.GetBody().SetState("sitting", false);
        this.thisHuman.GetBody().SetState("laying", true);  
    }

    public void Sleep() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        Debug.Log("sleep");
        this.SetState("falling asleep", true);
        this.thisHuman.GetBody().SetState("sleeping", true);  
    }
    
    public void WakeUp() {
        //bool doingNothing = !this.actionStateArray.Any(x => x);

        this.SetState("waking up", true);
        this.thisHuman.GetBody().SetState("sleeping", false);  
    }
    
    public void PickUp(float hand) {
        //bool doingNothing = !this.actionStateArray.Any(x => x);
        Collider[] pickableObj = new Collider[5];
        int pickUpHand = -1;
        int numObj = -1;

        if ((hand == 0) && (!this.thisHuman.GetBody().GetStateDict()["holding with left hand"])) {
            pickUpHand = 0;
        } else if ((hand == 1) && (!this.thisHuman.GetBody().GetStateDict()["holding with right hand"])){
            pickUpHand = 1;
        } else if (pickUpHand != -1) {
            this.SetState("picking up", true);

            for (int i = 0; i < numObj; i++) {
                if (!pickableObj[i].CompareTag("Human") && !pickableObj[i].CompareTag("ground")) {
                    pickableObj[i].GetComponent<Rigidbody>().isKinematic = true;
                    pickableObj[i].GetComponent<Rigidbody>().useGravity = false;
                    
                    if (pickUpHand == 0){
                        this.thisHuman.GetBody().SetState("holding with left hand", true);  
                    } else {
                        this.thisHuman.GetBody().SetState("holding with right hand", true);
                    }
                }
            }
        } 
    }

    public void SetDown(float hand) { Debug.Log("Can't set things down yet!"); }

    public void Eat(float hand) { Debug.Log("Can't eat yet!"); }
}

