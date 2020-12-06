using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChoice {

    public string actionLabel;
    public List<string> argumentList =  new List<string>();
    public Dictionary<string, float> argumentDict =  new Dictionary<string, float>();
    public Dictionary<string, float> actionValueDict = new Dictionary<string, float>();
    public Vector3 targetPos = new Vector3();

    public ActionChoice(List<string> actionLabelList, List<string> actionArgumentList){
        initActionChoices(actionLabelList, actionArgumentList);
    }

    public void initActionChoices(List<string> actionLabelList, List<string> actionArgumentList){
        this.actionValueDict.Clear();
        this.argumentDict.Clear();

        for (int i = 0; i < actionLabelList.Count; i++) {
            this.actionValueDict.Add(actionLabelList[i], 0);
        }
        
        for (int i = 0; i < actionArgumentList.Count; i++){
            this.argumentDict.Add(actionArgumentList[i], 0);
        }
    }
}