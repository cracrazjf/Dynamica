using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GenomeUI : MonoBehaviour {

    bool showMutable = true;
    public Text displayText;
    public Text muteText;

    protected Genome displayGenome;
    protected Phenotype displayPhenotype;
    protected Animal selectedAnimal;
    protected CountableObject selectedObject = null;
    protected static GameObject passed;

    protected static bool needsUpdate = false;
    protected bool togglePanel = false;

    public Button tempButton;
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

    public void UpdatePanel(){
        string toDisplay = "";
        displayGenome = selectedAnimal.GetGenome();
        displayPhenotype = selectedAnimal.GetPhenotype();
        displayText.text = toDisplay;
        
        if (showMutable) {
            toDisplay = displayPhenotype.GetDisplayInfo();
        } else { toDisplay = displayGenome.GetConstantInfo(); }
        displayText.text = toDisplay;
    }

    public static void ReceiveClicked(GameObject clicked) {
        // selectedObject = World.GetObject(clicked.name);
        passed = clicked;
        needsUpdate = true;
    }

    public void ToggleMutable() {
        showMutable = !showMutable;
        UpdatePanel();

        muteText.text = "Immutable";
        if (showMutable) { muteText.text = "Mutable"; } 
    }

    public void InitPanel(){
        panel = GameObject.Find("GenomePanel");
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

    public void Rename() {
        selectedObject.SetDisplayName(inputName.text);
        panelNamer.text = "";
    }
}