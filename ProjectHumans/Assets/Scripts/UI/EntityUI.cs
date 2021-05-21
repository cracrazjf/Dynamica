using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EntityUI : MonoBehaviour {

    protected Text[] stateText;
    protected Text goalText;
    protected Animal selectedAnimal = null;
    protected Entity selectedEntity;
    protected static GameObject passed;

    protected static bool needsUpdate;
    public static bool toExit = false;
    protected bool showPanel = false;
    static bool isAnimal;

    protected Button tempButton;
    protected GameObject panel;
    protected GameObject mainCam;
    protected GameObject halo;

    protected Text originalName;
    protected Text inputName;
    protected Transform panelNamer;
    protected Transform buttonParent;
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
         
        selectedEntity = World.GetEntity(passed.name);
        
        panel.SetActive(true);
        halo.SetActive(true);
        
        InitButtons();
        InitNamer();
        if (isAnimal) {
            InitDriveDisplays();
        } else { HideDriveDisplays(); }

        showPanel = true;
        needsUpdate = false;
        originalName.text = selectedEntity.GetDisplayName();
    }

    public void PassAnimalCam() {
       Camera toSend =  selectedAnimal.GetGameObject().GetComponentInChildren<Camera>();
       MainUI.EnterAnimalCam(toSend);
    }

    public void UpdatePanel() {
        //halo.transform.position = selectedAnimal.GetBody().GetXZPosition() + new Vector3(0, 0.01f, 0);
        //goalText.text = selectedAnimal.GetAction();
        if(isAnimal) { UpdateDriveDisplays(); }
    }

    private void UpdateDriveDisplays() {
        Vector<float> passedDrives = selectedAnimal.GetDriveSystem().GetStates(); 
        for(int i = 0; i < 5; i++) {
            float adjust = 100f;
            if (i < 2) { adjust = -100f; }

            float toDisplay = (passedDrives[i] * 100f);
            stateText[i].text = ((int)toDisplay).ToString();
        }
    }

    public static void ReceiveClicked(GameObject clicked, bool animal) {
        toExit = false;
        Debug.Log("Recieved " + clicked.name + "!");
        passed = clicked;
        isAnimal = animal;
        needsUpdate = true;
    }

    public void InitPanel() {
        panel = GameObject.Find("EntityPanel");
        halo = GameObject.Find("Halo");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
               header = child;
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
            selectedAnimal = (Animal) selectedEntity;
            World.PrintStates(selectedAnimal.GetDriveSystem().GetStateDict());
            string objectName = (selectedAnimal.GetDriveSystem().GetStateLabels()[i]) + "Text";
            stateText[i] = GameObject.Find(objectName).GetComponent<Text>();
        }
    }

    public void HideDriveDisplays() {
        GameObject driveSprites = GameObject.Find("Sprites");
        driveSprites.SetActive(false);
    }

    public void InitButtons() {
        foreach (Transform child in buttonParent) {
            if (child.name == "GenoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassGenome);
            } else if (child.name == "BrainButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassNet);
            } else if (child.name == "InfoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassStream);
            } else if (child.name == "TrashButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(TrashEntity);
            } 
        }

        foreach (Transform child in header) {
            if (child.name == "AnimalNamer") {
                panelNamer = child;
            } else if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            }  else if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassCenter);
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
        GenomeUI.ReceiveClicked(selectedEntity.GetGameObject());
        Debug.Log("Tried to pass to genome");
    }

    public void PassNet() {
        NetUI.ReceiveClicked(selectedAnimal.GetGameObject());
        Debug.Log("Tried to pass to nets");
    }

    public void PassStream() {
        StreamUI.ReceiveClicked(selectedAnimal.GetGameObject());
        Debug.Log("Tried to pass to thought stream");
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

    // public void PassAction() {
        
    //     var command = actionDrop.value;
    //     int param = Int32.Parse(paramDrop.options[paramDrop.value].text);

    //     // Debug.Log("Passed command A " + command + " with parameter of " + param);
    //     Debug.Log("Got an action!" + command + " " + param);
    //     selectedAnimal.SetCommand(command, param);
    // }

    public static void Sleep() {
        toExit = true;
    }
}