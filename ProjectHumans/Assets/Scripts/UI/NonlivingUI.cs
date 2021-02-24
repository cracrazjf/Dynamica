using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonlivingUI : PanelUI
{
    public void ReceiveClicked(GameObject clicked) {
        selectedObject = World.GetObject(clicked.name);
        UpdatePanel();
    }

    public void UpdatePanel(){}
    public void InitPanel(){}
    public void InitNamer(){}
}
