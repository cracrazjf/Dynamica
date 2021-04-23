using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NonlivingUI : MonoBehaviour {

    protected Item selectedItem = null;
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
        selectedItem = World.GetItem(passed.name);
        
        panel.SetActive(true);
        halo.SetActive(true);
        
        InitNamer();
        InitButtons();

        showPanel = true;
        needsUpdate = false;
        originalName.text = selectedItem.GetDisplayName();
    }

    public void UpdatePanel() {
        halo.transform.position = selectedItem.GetBody().GetXZPosition() + new Vector3(0, 0.01f, 0);
        //goalText.text = selectedAnimal.GetAction();
    }

    public static void ReceiveClicked(GameObject clicked) {
        Debug.Log("Got something!");
        passed = clicked;
        needsUpdate = true;
    }

    public void PassGenome() {
        GenomeUI.ReceiveClicked(selectedItem.GetGameObject());
    }

    public void InitPanel(){

        panel = GameObject.Find("NonlivingPanel");
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
            } else if (child.name == "TrashButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(TrashEntity);
            } else if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            }
        }
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

    public void PassCenter() {
        MainUI.CenterObject(passed.transform);
    }

    public void ExitPanel() {
        panel.SetActive(false);
        halo.SetActive(false);
        showPanel = false;
    }

    public void Rename() {
        if (panelNamer.gameObject.TryGetComponent(out InputField activeInput)) {
            selectedItem.SetDisplayName(activeInput.text);
            activeInput.text = "";
        }
    }

    public void TrashEntity() {
        World.RemoveEntity(selectedItem.GetName());
        ExitPanel();
    }
}
