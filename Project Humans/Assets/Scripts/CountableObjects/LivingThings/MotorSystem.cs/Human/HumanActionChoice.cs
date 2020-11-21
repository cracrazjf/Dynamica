using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanActionChoice {

    public string actionLabel;
    public List<string> argumentList =  new List<string>();
    public Dictionary<string, float> argumentDict =  new Dictionary<string, float>();
    public Dictionary<string, float> actionValueDict = new Dictionary<string, float>();
    public Vector3 targetPos = new Vector3();

    public HumanActionChoice(List<string> actionLabelList){
        initActionChoices(actionLabelList);
    }


    public void initActionChoices(List<string> actionLabelList){
        this.actionValueDict.Clear();
        for (int i = 0; i < actionLabelList.Count; i++) {
            this.actionValueDict.Add(actionLabelList[i], 0);
        }
        
        this.argumentDict.Clear();
        //this.argumentDict.Add("movementDistance", 0);   // this is -inf to inf
        this.argumentDict.Add("movementVelocity", 0);   // this is 0 to 1
        this.argumentDict.Add("rotationAngle", 0);      // this is -1 to 1
        this.argumentDict.Add("rotationVelocity", 0);   // this is 0 to 1
        this.argumentDict.Add("hand", 0);


    }
}