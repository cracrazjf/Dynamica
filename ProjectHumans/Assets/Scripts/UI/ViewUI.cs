using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ViewUI : MonoBehaviour {
    public static string passed;
    protected Camera viewCam;
    protected RenderTexture thisRender;
    protected Animal selectedAnimal;

    protected static bool needsUpdate;
    public static bool toExit = true;
    protected bool showPanel = false;

    protected GameObject cameraView;
    protected GameObject panel;
    protected Button tempButton;

    protected Text originalName;
    protected Text inputName;
    protected Transform header;

    private void Start() { 
        InitPanel(); 
        panel.SetActive(false);
    }
    
    private void Update() {
        if (toExit) { ExitPanel(); }
        if (needsUpdate) { OnAwake(); }
        if (showPanel) { UpdatePanel(); }
    }

    public void OnAwake() {
        Debug.Log("Someone woke me up!");
        panel.SetActive(true);
        selectedAnimal = World.GetAnimal(passed);
        showPanel = true;
        needsUpdate = false;
    }

    public void UpdatePanel() {
        // Set viewport texture to camera texture
        viewCam = selectedAnimal.GetSensorySystem().GetInternalCam();
        RawImage projectTo = cameraView.GetComponent<RawImage>();
        thisRender = new RenderTexture(viewCam.targetTexture);

        projectTo.texture = thisRender;
    }

    public static void ReceiveClicked(string clicked) {
        passed = clicked;
        needsUpdate = true;
    }

    public void InitPanel() {
        panel = GameObject.Find("ViewPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
               header = child;
            } if (child.name == "RawImage") {
                cameraView = child.gameObject;
            }
        }

        foreach (Transform child in header) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
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