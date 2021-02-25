using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnimalUI : MonoBehaviour {

    protected Text hunger;
    protected Text thirst;
    protected Text sleep;
    protected Text stamina;
    protected Text health;
    protected Text displayText;
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


    Button centerObjectButton;
    Button genomeButton;
    Button brainButton;

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

    public void PassAnimalCam() {
       Camera toSend =  selectedAnimal.gameObject.GetComponent<Camera>();
       MainUI.EnterAnimalCam(toSend);
    }

    public void UpdatePanel() {
        selectedAnimal = World.GetAnimal(passed.name);
        float[] toDisplay = new float[5];
        //Skipping for now because animals dont have drives
        //DriveSystem passed = selectedAnimal.GetDriveSystem();
        //float[] passedDrives = passed.GetDriveStateArray(); 

        //Debug.Log(passedDrives.ToString());

        // hunger.text = passedDrives["hunger"].ToString();
        // thirst.text = passedDrives["thirst"].ToString();
        // sleep.text = passedDrives["sleepiness"].ToString();
        // stamina.text = passedDrives["fatigue"].ToString();
        // health.text = passedDrives["health"].ToString();
    }

    public static void ReceiveClicked(GameObject clicked) {
        // selectedObject = World.GetObject(clicked.name);
        Debug.Log("Got an animal!");
        passed = clicked;
        needsUpdate = true;
    }

    public void InitPanel() {
        panel = GameObject.Find("AnimalPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassCenter);
            } else if (child.name == "GenoPanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassGenome);
            }
        }
    }

    public void InitNamer(){}

    public void PassGenome() {
        GenomeUI.ReceiveClicked(selectedAnimal.gameObject);
    }

    public void PassCenter() {
        MainUI.CenterObject(passed.transform);
    }

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