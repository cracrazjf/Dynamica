using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlantUI : PanelUI
{
    protected Plant selectedPlant = null;

    public void UpdatePanel(){
        selectedPlant = World.GetPlant(passed.name);
    }

    public void InitPanel(){
        panel = GameObject.Find("PlantPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassCenter);
            } else if (child.name == "GenoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassGenome);
            }
        }
    }

    public void PassGenome() {
        GenomeUI.ReceiveClicked(selectedPlant.gameObject);
    }

    public void PassCenter() {
        MainUI.CenterObject(passed.transform);
    }
    
    public void InitNamer(){}
}