using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StreamUI : MonoBehaviour {

    protected Text displayText;

    protected static Entity selectedEntity;
    protected static bool showPanel = false;
    protected static bool needsUpdate = false;

    protected Button tempButton;
    protected GameObject panel;
    protected GameObject header;
    protected GameObject mainCam;

    private void Start() {
        InitPanel();
        InitButtons();
        showPanel = false;
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
        string toDisplay = selectedEntity.GetStream();
        displayText.text = toDisplay;

        needsUpdate = false;
    }

    public static void ReceiveClicked(Entity clicked) {
        selectedEntity = clicked;
        OnAwake();
    }

    public void InitPanel(){
        panel = MainUI.GetUXPos("ThoughtPanel").gameObject;
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "ThoughtScrollView") {
                displayText = child.gameObject.GetComponentInChildren<Text>();
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
            }
        }
    }
        
    public void ExitPanel() { showPanel = false; }
}