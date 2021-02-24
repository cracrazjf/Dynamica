using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : PanelUI
{
    protected Plant selectedPlant = null;


    public void ReceiveClicked(GameObject clicked) {
        selectedPlant = World.GetPlant(clicked.name);
        UpdatePanel();
    }

    public void UpdatePanel(){}
    public void InitPanel(){}
    public void InitNamer(){}
}