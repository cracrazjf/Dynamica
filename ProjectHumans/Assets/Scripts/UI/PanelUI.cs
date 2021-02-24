using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUI : MonoBehaviour {

    protected CountableObject selectedObject = null;

    protected bool togglePanel = false;
    protected GameObject panel;
    protected GameObject mainCam;
    protected GameObject halo;

    public string emptyString = "";
    protected Text originalName;
    protected Text inputName;
    protected InputField panelNamer;

    private void Start() {
        InitPanel();
        InitNamer();
    }
    
    private void Update() {
        if(togglePanel) {
            UpdatePanel();
        }
    }
    
    public virtual void InitPanel(){}
    public virtual void InitNamer(){}
    public virtual void ReceiveClicked(GameObject clicked) {}
    public virtual void UpdatePanel(){}

    public void TogglePanel() {
        togglePanel = !togglePanel;
        panel.SetActive(togglePanel);
        UpdatePanel();
    }

    public void CenterObject() {
       Transform toSend =  selectedObject.gameObject.transform;
       MainUI.CenterObject(toSend);
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