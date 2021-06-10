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
    protected static bool showPanel = false;
    protected static bool needsUpdate = false;

    protected Button tempButton;
    protected GameObject panel;
    protected GameObject header;
    protected GameObject body;

    private void Start() { 
        InitPanel();
        InitButtons();
    }
    
    private void Update() {
        if (needsUpdate) { UpdatePanel(); }
        if (showPanel) {
            panel.SetActive(true);
        } else { panel.SetActive(false); }
    }

    public static void OnAwake() {
        needsUpdate = true;
        showPanel = true;
    }

    public void UpdatePanel(){
        string output = GetOutput();
        string input = GetInput();
        
        outputText.text = output;
        inputText.text = input;

        needsUpdate = false;
    }

    public static void ReceiveClicked(Entity clicked) {
        selectedEntity = clicked;
        OnAwake();
    }

    public void InitPanel() {
        panel = GameObject.Find("BrainPanel");

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "Body") {
                body = child.gameObject;
            }
        }

        foreach (Transform child in body.transform) {
            if (child.name == "InputScrollView") {
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
            } else if (child.name == "GraphButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(WakeGraph);
            }
        }
    }
        
    public void ExitPanel() { showPanel = false; }

    // ACTUAL NN STUFF

    public string GetInput() {
        string sendInfo = "This entity is unresponsive";
        int vbadIndex = systemDrop.value;
        string aiType = selectedEntity.GetNetworkName();
        if (aiType != "Unresponsive"){
            Animal brainOwner = (Animal) selectedEntity;
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
            Animal brainOwner = (Animal) selectedEntity;
            string rawInfo = brainOwner.GetAI().GetStringOutput(vbadIndex);
            sendInfo = rawInfo;
        }
        return sendInfo;
    }

    public void WakeGraph() {
        GraphWindow.ReceiveClicked(selectedEntity);
    }
}