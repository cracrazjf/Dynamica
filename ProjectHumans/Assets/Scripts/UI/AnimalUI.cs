using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnimalUI : MonoBehaviour {

    protected Text[] stateText;
    protected Text goalText;
    protected Animal selectedAnimal = null;
    protected static GameObject passed;

    protected static bool needsUpdate;
    public static bool toExit = false;
    protected bool showPanel = false;

    protected Button tempButton;
    protected GameObject panel;
    protected GameObject mainCam;
    protected GameObject halo;

    protected Dropdown actionDrop;
    protected Dropdown paramDrop;

    protected Text originalName;
    protected Text inputName;
    protected Transform panelNamer;
    protected Transform buttonParent;

    protected Text inputCommand;
    protected Transform commandField;

    Button centerObjectButton;
    Button genomeButton;
    Button brainButton;

    private void Start() { InitPanel(); }
    
    private void Update() {
        if (toExit) { ExitPanel(); }
        if (needsUpdate) { OnAwake(); }
        if (showPanel) { UpdatePanel(); }
    }

    public void OnAwake() {
        NonlivingUI.Sleep();
        PlantUI.Sleep();
        
        selectedAnimal = World.GetAnimal(passed.name);
        
        panel.SetActive(true);
        halo.SetActive(true);
        
        InitNamer();
        InitDriveDisplays();
        InitButtons();

        showPanel = true;
        needsUpdate = false;
        originalName.text = selectedAnimal.GetDisplayName();
    }

    public void PassAnimalCam() {
       Camera toSend =  selectedAnimal.GetGameObject().GetComponentInChildren<Camera>();
       MainUI.EnterAnimalCam(toSend);
    }

    public void UpdatePanel() {
        halo.transform.position = selectedAnimal.GetBody().GetXZPosition() + new Vector3(0, 0.01f, 0);
        //goalText.text = selectedAnimal.GetAction();
        UpdateDriveDisplays();
    }

    private void UpdateDriveDisplays() {
        Vector<float> passedDrives = selectedAnimal.GetDriveSystem().GetStates(); 
        for(int i = 0; i < 5; i++) {
            float toDisplay = (passedDrives[i] * 100f);
            // Debug.Log(toDisplay);
            stateText[i].text = ((int)toDisplay).ToString();
        }
    }

    public static void ReceiveClicked(GameObject clicked) {
        toExit = false;
        Debug.Log("Got an animal!");
        passed = clicked;

        needsUpdate = true;
    }

    public void InitPanel() {
        panel = GameObject.Find("AnimalPanel");
        halo = GameObject.Find("Halo");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "GoalInfo") {
                goalText = child.gameObject.GetComponentInChildren<Text>();
            } else if (child.name == "ActionDropdown") {
                actionDrop = child.gameObject.GetComponent<Dropdown>();
            } else if (child.name == "ParamDropdown") {
                paramDrop = child.gameObject.GetComponent<Dropdown>();
            } else if (child.name == "AnimalButtons") {
                buttonParent = child;
            } else if (child.name == "AnimalNamer") {
                panelNamer = child;
            } 
        }
    }

    public void InitDriveDisplays() {
        stateText = new Text[5];
        for (int i = 0; i < 5; i++) {
            string objectName = (selectedAnimal.GetDriveSystem().GetStateLabels()[i]) + "Text";
            stateText[i] = GameObject.Find(objectName).GetComponent<Text>();
        }
    }

    public void InitButtons() {
        foreach (Transform child in buttonParent) {
            if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassCenter);
            } else if (child.name == "GenoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassGenome);
            } else if (child.name == "CommandButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassAction);
            } else if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            } else if (child.name == "TrashButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(TrashEntity);
            } 
        }
    }

    public void InitNamer() {
        foreach (Transform child in panelNamer) {
            if (child.name == "CurrentName") {
                originalName = child.gameObject.GetComponent<Text>();
            } else if (child.name == "InputName") {
                inputName = child.gameObject.GetComponent<Text>();
            }
        }
    }

    public void PassGenome() {
        GenomeUI.ReceiveClicked(selectedAnimal.GetGameObject());
        Debug.Log("Tried to pass to genome");
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
            selectedAnimal.SetDisplayName(activeInput.text);
            activeInput.text = "";
        }
    }

    public void TrashEntity() {
        World.RemoveEntity(selectedAnimal.GetName());
        ExitPanel();
    }

    public void PassAction() {
        var command = actionDrop.value;
        int param = Int32.Parse(paramDrop.options[paramDrop.value].text);

        // Debug.Log("Passed command A " + command + " with parameter of " + param);

        selectedAnimal.SetCommand(command, param);
    }

    public static void Sleep() {
        toExit = true;
    }
}