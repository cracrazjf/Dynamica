using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanHCAI {

    public Human thisHuman;
    public Phenotype phenotype;
    public Transform rightHand;
    public Transform leftHand;
    public Transform humanObject;
    public int hungry_action_index;

    public string currentGoal;
    public string currentTask;
    public List<float> actionValueList = new List<float>();
    

    public HumanHCAI (Human human) {
        this.thisHuman = human;

        currentTask = "Rest";
        currentGoal = "None";
        hungry_action_index = hungry_action_list.Count - 1;

        // for (int i = 0; i < human.humanMotorSystem.actionLabelList.Count; i++) {
        //     actionValueList.Add(0);
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
            "accelerate", // value -1..1 translating to speed of accel./deccel.
            "rotate", // value from -1 to 1, translating into -180..180 degree rotation
            "sit", // not to remain sitting, to go from standing to sitting
            "stand", // not to remain standing
            "lay",    // not to remain laying
            "sleep",
            "wake_up",
            "pick_up_with_LH", // -1 means dont, 1 means do
            "pick_up_with_RH",
            "set_down_with_LH",
            "set_down_with_RH",
            "put_in_bag_with_LH",
            "put_in_bag_with_RH",
            "get_from_bag_with_LH",
            "get_from_bag_with_RH",
            "eat_from_LH",
            "eat_from_RH",
            "drink_with_LH",
            "drink_with_RH",
            "rest"});
    */
    public string current_action;
    public List<string> hungry_action_list = new List<string>(new string[] { "search", "go", "grab", "eat_with_LH" });
    public string current_hungry_action = "None";
    public List<float> ChooseAction()
    {

    //     int hungerIndex = human.driveSystem.stateIndexDict["hunger"];
    //     int hungerThresholdIndex = phenotype.traitIndexDict["hunger_threshold"];

    //     if (human.driveSystem.stateValueList[hungerIndex] >= phenotype.traitIndexDict["hungerThresholdIndex"])
    //     {

    //         currentGoal = "hungry";

    //         string hungry_action = hungry_action_list[hungry_action_index];
    //         int action_index = human.humanMotorSystem.actionIndexDict[hungry_action];

    //         if (currentGoal == "hungry")
    //         {
    //             if (hungry_action_list.Contains(hungry_action))
    //             {
    //                 actionValueList[action_index] = 1;
    //             }
    //             else if (current_hungry_action == "“None”")
    //             {


    //                 bool can_do_action = false;
    //                 if (hungry_action == "eat")
    //                 {
    //                     can_eat();

    //                 }
    //                 else if (hungry_action == "grab")
    //                 {
    //                     can_grab();
    //                 }
    //                 if (can_do_action)
    //                 {
    //                     Debug.Log(action_index);
    //                     actionValueList[action_index] = 1;
    //                 }
    //                 else
    //                 {
    //                     hungry_action_index -= 1;
    //                 }

    //             }
    //             else
    //             {
    //                 Debug.Log("gg");
    //             }



    //         }
         return actionValueList;
    }

    // private bool can_eat() {
	// 	if (rightHand.GetChild(0).tag == "food") {
	// 		return true;
	// 	}
	// 	else if (leftHand.GetChild(0).tag == "food") {
	// 		return true;
	// 	}
    //     return false;
	// }

	// private bool can_grab() {
    //     var infiniteNumber = Mathf.Infinity;
	// 	for(int i = 0; i < human.fovdetection.objects_in_vision.Count +1; i++) {
    //         if (human.fovdetection.objects_in_vision[i].tag == "food") {  // if edible objects are in current field of vision, appraoch to the nearest one and eat it;
    //             var distance = Vector3.Distance(human.fovdetection.objects_in_vision[i].transform.position, human.gameObject.transform.position);
    //             if (distance < infiniteNumber) { // check which food is the nearest one
    //                 infiniteNumber = distance;
                    
	// 				if (distance <= 2) {
	// 					return true;
	// 				}
                    
                    
    //             }
                
	// 		}
            
	// 	}
    //     return false;
	// } 
	
    

    int GetMaxDriveIndex()
    {
        return 0;       // get the index of the largest value in human.driveSystem.drivevalueList
    }         
}