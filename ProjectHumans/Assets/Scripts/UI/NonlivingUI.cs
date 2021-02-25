using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NonlivingUI : PanelUI
{
    public void UpdatePanel(){}
    public void InitPanel(){

        panel = GameObject.Find("NonlivingPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(MainUI.CenterObject(passed.transform));
            }
        }
    }
    public void InitNamer(){}
}
