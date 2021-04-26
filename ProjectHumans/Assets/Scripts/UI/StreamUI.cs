using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StreamUI : MonoBehaviour {

    
    protected Text displayText;
    
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

    private void Start() {
        InitPanel();
    }
    
   private void Update() {
        if (toExit) { ExitPanel(); }
        if (needsUpdate) { OnAwake(); }
    }

    public void OnAwake() {
        NetUI.Sleep();
        GenomeUI.Sleep();

        Debug.Log(passed.name);
        selectedEntity = World.GetEntity(passed.name);

        panel.SetActive(true);

        showPanel = true;
        needsUpdate = false;
        UpdatePanel();
    }

    public void UpdatePanel(){
        string toDisplay = selectedEntity.GetStream();
        displayText.text = toDisplay;
    }

    public static void ReceiveClicked(GameObject clicked) {
        selectedEntity = World.GetEntity(clicked.name);
        passed = clicked;
        needsUpdate = true;
    }

    public void InitPanel(){
        panel = GameObject.Find("ThoughtPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "ThoughtScrollView") {
                displayText = child.gameObject.GetComponentInChildren<Text>();
            } 
        }

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
        
    public void ExitPanel() {
        panel.SetActive(false);
        showPanel = false;
    }

    public static void Sleep() {
        toExit = true;
    }
}