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
    private static Dictionary<string, GameObject> childDict = new Dictionary<string, GameObject>();

    public string activePage = "Weight";

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

    public void UpdatePanel() {
        WakePage(activePage);
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
                // Don't add header to this dictionary
                header = child.gameObject;
            } else {
                childDict.Add(child.name, child.gameObject);
                Debug.Log("Added this panel to dictionary: " + child.name);
                child.gameObject.SetActive(false);
            }
        }
    }

    public void InitButtons() {
        foreach (Transform child in header.transform) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            } else if (child.name == "WeightTab") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(delegate { WakePage("Weight"); });
            } else if (child.name == "PerformanceTab") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(delegate { WakePage("Performance"); });
            } else if (child.name == "KnowledgeTab") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(delegate { WakePage("Knowledge"); });
            } else if (child.name == "NetworkTab") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(delegate { WakePage("Network"); });
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
        VisualModeling.ReceiveClicked(selectedEntity);
    }

    public void WakePage(string panelName) {
        string checkName = panelName + "Page";
        foreach(KeyValuePair<string, GameObject> page in childDict) {
            if (page.Key == checkName) {
                page.Value.SetActive(true);
                activePage = panelName;
            } else { page.Value.SetActive(false); }
        }
    }
}