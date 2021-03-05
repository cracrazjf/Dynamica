using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlantUI : MonoBehaviour {

    protected Plant selectedPlant = null;
    protected static GameObject passed;

    protected static bool needsUpdate = false;
    protected bool showPanel = false;

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
        if (needsUpdate) {
            showPanel = true;
            needsUpdate = false;
        }
        if (showPanel) { UpdatePanel(); }
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
        selectedPlant = World.GetPlant(passed.name);
    }

    public void InitPanel(){
        panel = GameObject.Find("PlantPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassCenter);
            } else if (child.name == "GenoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassGenome);
            }
        }
    }

    public static void ReceiveClicked(GameObject clicked) {
        Debug.Log("Got a plant!");
        passed = clicked;
        needsUpdate = true;
    }

    public void PassGenome() {
        GenomeUI.ReceiveClicked(selectedPlant.GetGameObject());
    }

    public void PassCenter() {
        MainUI.CenterObject(passed.transform);
    }
    
    public void InitNamer(){}

    public void ExitPanel() {
        panel.SetActive(false);
        halo.SetActive(false);
        showPanel = false;
    }

    public void Rename() {
        selectedPlant.SetDisplayName(inputName.text);
        panelNamer.text = "";
    }
}