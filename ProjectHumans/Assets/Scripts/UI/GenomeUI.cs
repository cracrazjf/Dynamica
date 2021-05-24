using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GenomeUI : MonoBehaviour {

    protected bool showMutable = true;
    protected Text displayText;
    protected Text muteText;

    protected Genome displayGenome;
    protected Phenotype displayPhenotype;
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
        if (showPanel) { panel.SetActive(true); }
    }

    public void OnAwake() {
        NetUI.Sleep();
        StreamUI.Sleep();

        Debug.Log(passed.name);
        selectedEntity = World.GetEntity(passed.name);

        panel.SetActive(true);

        showPanel = true;
        needsUpdate = false;
        UpdatePanel();
    }

    public void UpdatePanel(){
        string toDisplay = "";
        displayGenome = selectedEntity.GetGenome();
        displayPhenotype = selectedEntity.GetPhenotype();
        
        if (showMutable) {
            toDisplay = displayPhenotype.GetDisplayInfo();
            muteText.text = "Mutable"; 

        } else { 
            toDisplay = displayGenome.GetConstantInfo(); 
            muteText.text = "Immutable";
        }
        displayText.text = toDisplay;

    }

    public static void ReceiveClicked(GameObject clicked) {
        selectedEntity = World.GetEntity(clicked.name);
        passed = clicked;
        needsUpdate = true;
    }

    public void ToggleMutable() {
        showMutable = !showMutable;
    }

    public void InitPanel(){
        panel = MainUI.GetUXPos("GenomePanel").gameObject;
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "GeneticScrollView") {
                displayText = child.gameObject.GetComponentInChildren<Text>();
            } 
        }

        foreach (Transform child in header.transform) {
            if (child.name == "ToggleMuteButton") {
                muteText = child.gameObject.GetComponentInChildren<Text>();
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleMutable);
                child.gameObject.SetActive(false);
            } else if (child.name == "ClosePanelButton") {
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