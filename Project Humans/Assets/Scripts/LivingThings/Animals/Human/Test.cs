/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.CodeDom.Compiler;

public class Test : MonoBehaviour {

    public Human human;
    public Transform rightHand;
    public GameObject Bag;
    public GameObject Storage;

    public Transform leftHand;
    public Transform humanObject;
    public string currentGoal;
    public string currentTask;
    public List<float> actionValueList = new List<float>();
    public FOVDetection fovscript;
	public string current_action;

	
	public void ChooseAction() {
	
	string current_hungry_action = "None";
	
	if (human.driveSystem.stateValueList[hungerIndex] >= human.phenotype.traitValueList[hungerThresholdIndex]) {
			List<String> hungry_action_list = new List<string> []{"search", "go", "grab", "eat"};
			string current_goal = "hungry";
			
			if (current_goal == hungry) {
				if (hungry_action_list.Contains(hungry_action)) {
					 actionValueList[action_index] = 1;
				}
				else if (current_action == "“None”") {
					int hungry_action_index = hungry_action_list.Count - 1;
					string hungry_action = hungry_action_list[hungry_action_index];
					int action_index = human.humanMotorSystem.actionIndexDict[hungry_action];
					bool can_do_action = false;

					else if (hungry_action == "eat") {
						can_eat();

					}
					else if (hungry_action == "grab") {
						can_grab();
					}
					if (can_do_action) {
						actionValueList[action_index] = 1;
					}
					else {
						hungry_action_index -= 1;
					}
					
				}
				else {
					Debug.log("gg");
				}
				

				
			}
		}
	}

	private bool can_eat() {
		if (rightHand.GetChild(0).tag == "food") {
			return true;
		}
		else if (leftHand.GetChild(0).tag == "food") {
			return true;
		}
	}

	private bool can_grab() {
		for(int i = 0; i < human.fovdetection.objects_in_vision.Count +1; i++) {
            if (human.fovdetection.objects_in_vision[i].tag == "food") {  // if edible objects are in current field of vision, appraoch to the nearest one and eat it;
                var distance = Vector3.Distance(human.fovdetection.objects_in_vision[i].transform.position, transform.position);
                if (distance < infiniteNumber) { // check which food is the nearest one
                    infiniteNumber = distance;
                    
					if (distance <= 2) {
						return true;
					}
                }
			}
		}
	} 
	
	
	
    if current_action in hungry_action_list:
           actionValueList[action_index] = 1;
    
    else if current_action == “None”:
		hungry_action_index =    len(hungry_action_list) - 1;
		hungry_action = hungry_action_list[hungry_action_index];
		action_index = human.humanMotorSystem.actionIndexDict[hungry_action];

		can_do_action = False;

		if hungry_action == ‘eat’:
			if food_in_LH or food_in_RH:
				can_do_action = True
		else if hungry_action == ‘grab’:
			if food_in_reach:
				can_do_action = True
		else if hungry_action = ‘go’:
			if food_visible:
				can_do_action = True
		else if hungry_action = ‘search’:
			can_do_action = True
		
		if can_do_action:
			actionValueList[action_index] = 1
		else:
			hungry_action_index -= 1
    
    else:
        print a warning message that we screwed up

bool can_eat()
    if so
        return true
    else
        return false
		
*/