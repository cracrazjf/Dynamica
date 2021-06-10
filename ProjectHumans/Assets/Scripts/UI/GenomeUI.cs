using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GenomeUI : MonoBehaviour {

    protected bool showMutable = true;
    protected Text muteText;

    protected Text genomeText;
    protected Text phenotypeText;
    protected Genome displayGenome;
    protected Phenotype displayPhenotype;

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
        displayGenome = selectedEntity.GetGenome();
        displayPhenotype = selectedEntity.GetPhenotype();
        
        string toSend = displayPhenotype.GetQualInfo();
        phenotypeText.text = toSend + displayPhenotype.GetTraitInfo();

        genomeText.text = displayGenome.GetDisplayInfo(showMutable);

        needsUpdate = false;
    }

    public static void ReceiveClicked(Entity clicked) {
        selectedEntity = clicked;
        OnAwake();
    }

    public void InitPanel(){
        panel = MainUI.GetUXPos("GenomePanel").gameObject;
    
        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "Body") {
                body = child.gameObject;
            }
        }

        foreach (Transform child in body.transform) {
            if (child.name == "GenomeScrollView") {
                genomeText = child.gameObject.GetComponentInChildren<Text>();
            } else if (child.name == "PhenotypeScrollView") {
                phenotypeText = child.gameObject.GetComponentInChildren<Text>();
            } 
        }
    }

    public void InitButtons() {
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
    
    public void ExitPanel() { showPanel = false; }
    public void ToggleMutable() { showMutable = !showMutable; }
}