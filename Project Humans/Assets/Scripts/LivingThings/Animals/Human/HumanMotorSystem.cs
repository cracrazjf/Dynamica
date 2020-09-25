using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMotorSystem : MonoBehaviour
{
    public Human human;
    public Phenotype phenotype;
    public HumanHCAI humanhcai;
    private FOVDetection fovscript;
    public GameObject target;
    public Animator animator;
    public List<GameObject> allObjectList = new List<GameObject>();
   
    public float maxAngle;
    public float maxRadius;
    public string active_AI;
    public bool actionSwitch = false;
    
    
   
    // what is the living thing object that this class belongs to

    /// <value>List of actions ranging from -1 to 1</value>
    public var actionLabelList = new List<string>({ "accelerate",   // value -1..1 translating to speed of accel./deccel.
                                                    "rotate",       // value from -1 to 1, translating into -180..180 degree rotation
                                                    "sit",          // begin to sit
                                                    "stand",        // begin to stand
                                                    "lay",          // begin to lay down
                                                    "sleep",        // begin to sleep
                                                    "wake_up",      // begin to wake
                                                    "pick_up_LH", "pick_up_RH",
                                                    "set_down_LH","set_down_RH",
                                                    "put_bag_LH", "put_bag_RH",
                                                    "get_bag_LH", "get_bag__RH",
                                                    "eat_LH", "eat_RH",
                                                    "drink_LH", "drink_RH",
                                                    "rest"});  // do we want rest to be explicit, or just not doing anything else

    public int numActions; // 15
    public Dictionary<string, int> actionIndexDict = new Dictionary<string, int>();
    public List<float> actionThreshold = new List<float>();// generate an actionThreshold list
    public List<float> actionValueList = new List<float>(); // [1 0 0 0 0 0 1 0 0 0 0 1 0]
    public List<float> actionDisplayList = new List<float>();

    List<HumanBehavior> ActionFunctionList = new List<HumanBehavior>();


    

    private void Start()
    {
        CreateActionFunctionList();
        human = gameObject.GetComponent<Human>();

        numActions = actionLabelList.Count;
        for (int i = 0; i < numActions; i++)
        {
            actionThreshold.Add(i);
            actionIndexDict.Add(actionLabelList[i], i);
            actionDisplayList.Add(i);
            //actionValueList.Add(human.humanhcai.actionValueList[i]);
            actionDisplayList[i] = 1;
            actionThreshold[i] = 0.8f;
        }
    }



    public void ChooseAction()
    {
        if (active_AI == "HCAI")
        {
            actionValueList = humanhcai.ChooseAction();
        }
        else if (active_AI == "NNAI")
        {
            // todo add the call to the neural network ai
        }
        else
        {
            // generate an error message telling us no AI was active
        }
    }


    // public void TakeAction()
    // {
    //     for (int i = 0; i < numActions; i++)
    //     {
    //         if (human.humanhcai.actionValueList[i] != 0)
    //         {
    //             ActionFunctionList[i]();
    //         }
    //     }
    // }


    delegate void HumanBehavior();

    void CreateActionFunctionList()
    {
        // create a list of delegate objects as placeholders for the methods.
        // note the methods must all be of type void with no parameters
        // that is they must all have the same signature.

        ActionFunctionList.Add(Accelerate);
        ActionFunctionList.Add(Rotate);
        ActionFunctionList.Add(sit);
        ActionFunctionList.Add(stand);
        ActionFunctionList.Add(lay);
        ActionFunctionList.Add(sleep);
        ActionFunctionList.Add(wake_up);
        ActionFunctionList.Add(pick_up_with_LH);
        ActionFunctionList.Add(pick_up_with_RH);
        ActionFunctionList.Add(set_down_with_LH);
        ActionFunctionList.Add(set_down_with_RH);
        ActionFunctionList.Add(put_in_bag_with_LH);
        ActionFunctionList.Add(put_in_bag_with_RH); 
        ActionFunctionList.Add(get_from_bag_with_LH);
        ActionFunctionList.Add(get_from_bag_with_RH);
        ActionFunctionList.Add(eat_from_LH);
        ActionFunctionList.Add(eat_from_RH);
        ActionFunctionList.Add(drink_with_LH);
        ActionFunctionList.Add(drink_with_RH);
        ActionFunctionList.Add(rest);

        //Debug.Log("added");
        // call a method ...

    }
    
    void Accelerate()
    {

        // -1...1, what does that tranlate into, in Unity terms, -1 +1
        //accelerationRate = actionValueList[actionIndexDict["acceleration"]];
        actionSwitch = true;
        phenotype.currentVelocity = phenotype.currentVelocity + (phenotype.accelerationRate * Time.deltaTime);
        transform.Translate(0, 0, phenotype.currentVelocity);
        phenotype.currentVelocity = Mathf.Clamp(phenotype.currentVelocity, phenotype.initialVelocity, phenotype.finalVelocity);
        animator.SetFloat("Velocity", phenotype.currentVelocity *5);
        if (phenotype.currentVelocity > 0)
        {
            animator.SetBool("moving", true);

        }
        if (phenotype.currentVelocity == 0)
        {
            animator.SetBool("moving", false);
            actionSwitch = false;
        }
    }
    void Rotate()
    {
        actionSwitch = true;
        phenotype.rotation = phenotype.rotationspeed * Time.deltaTime;
        if (phenotype.rotationleft > phenotype.rotation)
        {
            phenotype.rotationleft -= phenotype.rotation;
        }
        if (phenotype.rotationleft <= phenotype.rotation)
        {
            phenotype.rotation = phenotype.rotationleft;
            phenotype.rotationleft = 0;
        
        
            
            
            transform.Rotate(0, phenotype.rotation, 0);
        }
    }
    void pick_up_with_LH()
    {
        animator.SetBool("pickup", true);
        animator.SetFloat("pickuporsetdown", 0);
    }

    void pick_up_with_RH()
    {
        Debug.Log("picked up with RH");
    }
    void set_down_with_LH()
    {
        animator.SetBool("pickup", true);
        animator.SetFloat("pickuporsetdown", 1);
    }

    void set_down_with_RH()
    {
        Debug.Log("set_down_with_RH");
    }

    void put_in_bag_with_LH()
    {
        Debug.Log("put in bag with LH");
    }
    void put_in_bag_with_RH()
    {
        Debug.Log("put in bag with RH");
    }

    void get_from_bag_with_LH()
    {
        Debug.Log("get_from_bag_with_LH"); 
    }

     void get_from_bag_with_RH()
    {
        Debug.Log("get_from_bag_with_RH"); 
    }

    void sit()
    {
        animator.SetBool("rest", true);
        animator.SetFloat("sit_or_stand", 0);
    }

    void stand()
    {
        animator.SetFloat("sit_or_stand", 0);
        animator.SetBool("rest", false);

    }

    void lay()
    {
        Debug.Log("lay");       
    }

    void eat_from_LH()
    {
        animator.SetBool("eat", true);
    }

    void eat_from_RH()
    {
        Debug.Log("eat with RH");
    }

    void drink_with_LH()
    {
        animator.SetBool("drink", true);
        animator.SetFloat("bottleorlake", 1);
        
    }

    void drink_with_RH()
    {
        Debug.Log("drink with RH");
        
    }


    void sleep() {
        Debug.Log("sleep");
    }
    

    void wake_up()
    {
        Debug.Log("wake_up");
    }

    void rest()
    {
        Debug.Log("rest");
    }

    //Draw line to visualize visionfield
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);


        for (int i = 0; i < human.fovdetection.objects_in_vision.Count ; i++)
        {
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (fovscript.objects_in_vision[i].transform.position - transform.position).normalized * maxRadius);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);


    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water" && human.humanMotorSystem.target.tag == "water")
        {
            
            human.humanMotorSystem.target = null;
            human.humanMotorSystem.allObjectList.Remove(other.gameObject);
            Destroy(other.gameObject);
            //human.driveSystem.thirst = 0.0f;

        }
    }
}



