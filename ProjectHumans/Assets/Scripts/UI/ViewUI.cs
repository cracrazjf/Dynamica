using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ViewUI : MonoBehaviour {
    protected Camera viewCam;
    protected RenderTexture thisRender;

    protected static Animal selectedAnimal;
    protected static bool needsUpdate = false;
    protected static bool showPanel = false;

    protected GameObject cameraView;
    protected GameObject panel;

    protected Button tempButton;
    protected Text originalName;
    protected Text inputName;
    protected Transform header;

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
        Debug.Log("Someone woke me up!");
        needsUpdate = true;
        showPanel = true;
    }

    public void UpdatePanel() {
        // Set viewport texture to camera texture
        viewCam = selectedAnimal.GetSensorySystem().GetInternalCam();
        RawImage projectTo = cameraView.GetComponent<RawImage>();
        viewCam.targetTexture = (RenderTexture) projectTo.texture;
        
        //thisRender = new RenderTexture(viewCam.targetTexture);
        //projectTo.texture = thisRender;

        needsUpdate = false;
    }

    public static void ReceiveClicked(Animal clicked) {
        selectedAnimal = clicked;
        OnAwake();
    }

    public void InitPanel() {
        panel = MainUI.GetUXPos("ViewPanel").gameObject;

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
               header = child;
            } if (child.name == "RawImage") {
                cameraView = child.gameObject;
            }
        }
    }

    public void InitButtons() {
        foreach (Transform child in header) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            }
        }
    }

    public void ExitPanel() { showPanel = false; }
}