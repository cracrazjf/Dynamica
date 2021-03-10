using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnimalUI : MonoBehaviour {

    protected Text[] stateText;
    protected Text goalText;
    protected Animal selectedAnimal = null;
    protected static GameObject passed;

    protected static bool needsUpdate;
    protected bool showPanel = false;

    protected Button tempButton;
    protected Button closePanelButton;
    protected GameObject panel;
    protected GameObject mainCam;
    protected GameObject halo;

    protected Text originalName;
    protected Text inputName;
    protected Transform panelNamer;

    protected Text inputCommand;
    protected Transform commandField;

    Button centerObjectButton;
    Button genomeButton;
    Button brainButton;

    private void Start() {
        InitPanel();
        InitNamer();
    }
    
    private void Update() {
        if (needsUpdate) {
            OnAwake();
        }
        if (showPanel) { UpdatePanel(); }
    }

    public void OnAwake() {
        selectedAnimal = World.GetAnimal(passed.name);
        originalName.text = selectedAnimal.GetDisplayName();

        panel.SetActive(true);
        halo.SetActive(true);

        showPanel = true;
        needsUpdate = false;
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
       Camera toSend =  selectedAnimal.GetGameObject().GetComponentInChildren<Camera>();
       MainUI.EnterAnimalCam(toSend);
    }

    public void UpdatePanel() {
        halo.transform.position = selectedAnimal.GetBody().GetXZPosition() + new Vector3(0, 0.01f, 0);

        float[] passedDrives = selectedAnimal.GetDriveSystem().GetStates(); 

        goalText.text = selectedAnimal.GetAction();
        stateText = new Text[5];
        for (int i = 0; i < 5; i++) {
            string label = selectedAnimal.GetDriveSystem().GetStateLabels()[i];
            string objectName = label + "Text";
            stateText[i] = GameObject.Find(objectName).GetComponent<Text>();
        }
        for(int i = 0; i < 5; i++) {
            float toDisplay = (passedDrives[i] * 100f);
            // Debug.Log(toDisplay);
            stateText[i].text = ((int)toDisplay).ToString();
        }
    }

    public static void ReceiveClicked(GameObject clicked) {
        // selectedObject = World.GetObject(clicked.name);
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
            } else if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassCenter);
            } else if (child.name == "GenoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassGenome);
            } 
        }
    }

    public void InitNamer() {
        foreach (Transform child in panel.transform) {
            if (child.name == "AnimalNamer") {
                panelNamer = child;
            }
            if (child.name == "CommandInput") {
                commandField = child;
            }
        }
        foreach (Transform child in panelNamer) {
            if (child.name == "CurrentName") {
                originalName = child.gameObject.GetComponent<Text>();
            } else if (child.name == "InputName") {
                inputName = child.gameObject.GetComponent<Text>();
            }
        }

        foreach (Transform child in commandField) {
            if (child.name == "InputName") {
                inputCommand = child.gameObject.GetComponent<Text>();
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

    public void PassAction() {
        if (panelNamer.gameObject.TryGetComponent(out InputField activeInput)) {
            selectedAnimal.SetCommand(activeInput.text);
            activeInput.text = "";
        }
    }
}