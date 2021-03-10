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
    protected Animal selectedAnimal;
    protected static GameObject passed;

    protected static bool needsUpdate = false;
    protected bool showPanel = false;

    protected Button tempButton;
    protected Button closePanelButton;
    protected GameObject panel;
    protected GameObject mainCam;



    private void Start() {
        InitPanel();
        InitXButton();
    }
    
   private void Update() {
        if (needsUpdate) {
            OnAwake();
        }
        if (showPanel) { UpdatePanel(); }
    }

    public void OnAwake() {
        Debug.Log(passed.name);
        selectedAnimal = World.GetAnimal(passed.name);

        panel.SetActive(true);

        showPanel = true;
        needsUpdate = false;
    }

    public void InitXButton(){
        foreach (Transform child in panel.transform) {
            if (child.name == "ClosePanelButton") {
                closePanelButton = child.gameObject.GetComponent<Button>();
            } 
            closePanelButton.onClick.AddListener(ExitPanel);
        }
    }

    public void UpdatePanel(){
        string toDisplay = "";
        displayGenome = selectedAnimal.GetGenome();
        displayPhenotype = selectedAnimal.GetPhenotype();
        
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
        // selectedObject = World.GetObject(clicked.name);
        passed = clicked;
        needsUpdate = true;
    }

    public void ToggleMutable() {
        showMutable = !showMutable;
    }

    public void InitPanel(){
        panel = GameObject.Find("GenomePanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "ToggleMuteButton") {
                //muteText = child.gameObject.GetComponentInChildren<Text>();
                //tempButton = child.gameObject.GetComponent<Button>();
                //tempButton.onClick.AddListener(ToggleMutable);
                child.gameObject.SetActive(false);
            } else if (child.name == "GeneticScrollView") {
                displayText = child.gameObject.GetComponentInChildren<Text>();
            } 
        }
    }

    public void ExitPanel() {
        panel.SetActive(false);
        showPanel = false;
    }
}