using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetUI : MonoBehaviour {

    protected Entity selectedObject = null;
    protected static GameObject passed;

    protected static bool needsUpdate = false;
    protected bool togglePanel = false;

    protected Button tempButton;
    protected Button closePanelButton;
    protected GameObject panel;
    protected GameObject mainCam;
    protected GameObject halo;

    protected Text originalName;
    protected Text inputName;
    protected InputField panelNamer;

    private void Start() {
        InitPanel();
        InitNamer();
    }
    
    private void Update() {
        if(needsUpdate) {
            TogglePanel();
            needsUpdate = false;
        }

        if(togglePanel) {
            UpdatePanel();
        }
    }

    public void InitXButton(){
        foreach (Transform child in panel.transform) {
            if (child.name == "ClosePanelButton") {
                closePanelButton = child.gameObject.GetComponent<Button>();
            } 
            closePanelButton.onClick.AddListener(ExitPanel);
        }
    }

    public static void ReceiveClicked(GameObject clicked) {
        // selectedObject = World.GetObject(clicked.name);
        passed = clicked;
        needsUpdate = true;
    }

    public void UpdatePanel(){}
    public void InitPanel() {
        panel = GameObject.Find("BrainPanel");
        panel.SetActive(false);
    }
    public void InitNamer(){}

    public void TogglePanel() {
        togglePanel = !togglePanel;
        panel.SetActive(togglePanel);
        UpdatePanel();
    }

    public void ExitPanel() {
        panel.SetActive(false);
        halo.SetActive(false);
    }
}