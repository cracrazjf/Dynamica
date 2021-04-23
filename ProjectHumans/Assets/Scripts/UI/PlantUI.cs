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
    protected GameObject panel;
    protected GameObject mainCam;
    protected GameObject halo;

    protected Text originalName;
    protected Text inputName;
    protected Transform panelNamer;

    private void Start() { InitPanel(); }
    
    private void Update() {
        if (needsUpdate) { OnAwake(); }
        if (showPanel) { UpdatePanel(); }
    }

    public void OnAwake() {
        selectedPlant = World.GetPlant(passed.name);
        
        panel.SetActive(true);
        halo.SetActive(true);
        
        InitNamer();
        InitButtons();

        showPanel = true;
        needsUpdate = false;
        originalName.text = selectedPlant.GetDisplayName();
    }

    public void UpdatePanel() {
        halo.transform.position = selectedPlant.GetBody().GetXZPosition() + new Vector3(0, 0.01f, 0);
        //goalText.text = selectedAnimal.GetAction();
    }

    public void InitPanel(){
        panel = GameObject.Find("PlantPanel");
        panel.SetActive(false);
        halo = GameObject.Find("Halo");
        
        foreach (Transform child in panel.transform) {
            if (child.name == "EntityNamer") {
                panelNamer = child;
            } 
        }
    }

    public void InitButtons() {
        foreach (Transform child in panel.transform) {
            if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassCenter);
            } else if (child.name == "GenoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassGenome);
            } else if (child.name == "TrashButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(TrashEntity);
            } else if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
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
    
    public void InitNamer(){
        foreach (Transform child in panelNamer) {
            if (child.name == "CurrentName") {
                originalName = child.gameObject.GetComponent<Text>();
            } else if (child.name == "InputName") {
                inputName = child.gameObject.GetComponent<Text>();
            }
        }
    }

    public void ExitPanel() {
        panel.SetActive(false);
        halo.SetActive(false);
        showPanel = false;
    }

    public void Rename() {
        if (panelNamer.gameObject.TryGetComponent(out InputField activeInput)) {
            selectedPlant.SetDisplayName(activeInput.text);
            activeInput.text = "";
        }
    }

    public void TrashEntity() {
        World.RemoveEntity(selectedPlant.GetName());
        ExitPanel();
    }
}