using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetUI : MonoBehaviour
{

    bool togglePanel = true;

    GameObject panel;
    GameObject mainCam;

    private void Start()
    {
        panel = GameObject.Find("BrainPanel");
    }
    
    private void Update(){}

    public void TogglePanel() {
        togglePanel = !togglePanel;
        panel.SetActive(togglePanel);
    }

    public void ExitPanel() {
        panel.SetActive(false);
    }
}