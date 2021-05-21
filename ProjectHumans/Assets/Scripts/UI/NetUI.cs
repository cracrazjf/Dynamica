using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NetUI : MonoBehaviour {

    protected Text inputText;
    protected Text outputText;
    protected Dropdown systemDrop;

    protected static Entity selectedEntity;
    protected static GameObject passed;

    protected static bool needsUpdate = false;
    public static bool toExit = false;
    protected bool showPanel = false;

    protected Button tempButton;
    protected Button closePanelButton;
    protected GameObject panel;
    protected GameObject header;
    protected GameObject mainCam;

    private void Start() { InitPanel(); }
    
    private void Update() {
        if (toExit) { ExitPanel(); }
        if (needsUpdate) { OnAwake(); }
        if (showPanel) { 
            panel.SetActive(true); 
            UpdatePanel();
        }
    }

    public void OnAwake() {
        GenomeUI.Sleep();
        StreamUI.Sleep();
        
        selectedEntity = World.GetEntity(passed.name);
        panel.SetActive(true);
    
        InitButtons();
        showPanel = true;
        needsUpdate = false;
    }

    public void UpdatePanel(){
        string output = GetOutput();
        string input = GetInput();
        
        outputText.text = output;
        inputText.text = input;
    }

    public static void ReceiveClicked(GameObject clicked) {
        selectedEntity = World.GetEntity(clicked.name);
        passed = clicked;
        needsUpdate = true;
    }

    public void InitPanel() {
        panel = GameObject.Find("BrainPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "InputScrollView") {
                inputText = child.gameObject.GetComponentInChildren<Text>();
            } else if (child.name == "OutputScrollView") {
                outputText = child.gameObject.GetComponentInChildren<Text>();
            } 
        }
    }

    public void InitButtons() {
        foreach (Transform child in header.transform) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            } else if (child.name == "RefreshButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(UpdatePanel);
            } else if (child.name == "SystemDropdown") {
                systemDrop = child.gameObject.GetComponent<Dropdown>();
            }
        }
    }
        
    public void ExitPanel() {
        panel.SetActive(false);
        showPanel = false;
    }

    // ACTUAL NN STUFF

    public string GetInput() {
        string sendInfo = "This entity is unresponsive";
        int vbadIndex = systemDrop.value;
        string aiType = selectedEntity.GetNetworkName();
        if (aiType != "Unresponsive"){
            Animal brainOwner = World.GetAnimal(selectedEntity.GetName());
            string rawInfo = brainOwner.GetAI().GetStringInput(vbadIndex);
            sendInfo = rawInfo;
        }
        return sendInfo;
    }

    public string GetOutput() {
        string sendInfo = "This entity is unresponsive";
        int vbadIndex = systemDrop.value;
        string aiType = selectedEntity.GetNetworkName();
        if (aiType != "Unresponsive"){
            Animal brainOwner = World.GetAnimal(selectedEntity.GetName());
            string rawInfo = brainOwner.GetAI().GetStringOutput(vbadIndex);
            sendInfo = rawInfo;
        }
        return sendInfo;
    }

    public static void Sleep() {
        toExit = true;
    }
}