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

    protected static Entity selectedEntity;
    protected static bool needsUpdate = false;
    protected static bool showPanel = false;
    protected static bool isAnimal;
    protected GameObject mainCam;
    protected Transform header;
    protected GameObject panel;
    protected GameObject halo;

    protected Button tempButton;
    protected Text inputName;
    protected Text originalName;
    protected Transform panelNamer;
    protected Transform buttonParent;
    protected Transform spriteParent;
    
    private void Start() { 
        InitPanel(); 
        InitButtons();
        InitNamer();
        showPanel = false;
    }
    
    private void Update() {
        if (showPanel) {
            panel.SetActive(true);
            halo.SetActive(true);

            if (isAnimal) {
                InitDriveDisplays();
            } else { HideDriveDisplays(); }

        } else { 
            panel.SetActive(false); 
            halo.SetActive(false);
        }

        if (needsUpdate) { UpdatePanel(); }

    }

    public static void OnAwake() {
        isAnimal = selectedEntity.CheckAnimal();
        if (!isAnimal) { 
            Debug.Log("It seems this entity is not an animal"); 
        } else {
            Debug.Log("A-okay!");
        }
        
        showPanel = true;
        needsUpdate = true;
    }

    public void UpdatePanel() {

        originalName.text = selectedEntity.GetDisplayName();
        halo.transform.position = selectedEntity.GetBody().GetXZPosition() + new Vector3(0, 0.75f, 0);
        //goalText.text = selectedAnimal.GetAction();
        if (isAnimal) { UpdateDriveDisplays(); }
    }

    private void UpdateDriveDisplays() {
        selectedAnimal = (Animal) selectedEntity;
        Vector<float> passedDrives = selectedAnimal.GetDriveSystem().GetStates(); 
        for(int i = 0; i < 5; i++) {

            float toDisplay = (passedDrives[i] * 100f);
            stateText[i].text = ((int)toDisplay).ToString();
        }
    }

    public static void ReceiveClicked(Entity clicked) {
        selectedEntity = clicked;
        OnAwake();
    }

    public void InitPanel() {
        panel = GameObject.Find("EntityPanel");
        halo = GameObject.Find("Halo");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
               header = child;
            } else if (child.name == "EntityButtons") {
                buttonParent = child;
            } else if (child.name == "Sprites") {
                spriteParent = child;
            } 
        }
    }

    public void InitDriveDisplays() {
        stateText = new Text[5];
        for (int i = 0; i < 5; i++) {
            selectedAnimal = (Animal) selectedEntity;
            World.PrintStates(selectedAnimal.GetDriveSystem().GetStateDict());
            string objectName = (selectedAnimal.GetDriveSystem().GetStateLabels()[i]) + "Text";
            // state text is null for some
            stateText[i] = GameObject.Find(objectName).GetComponent<Text>();
        }
        spriteParent.gameObject.SetActive(true);
    }

    public void HideDriveDisplays() { spriteParent.gameObject.SetActive(false); }

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
            } else if (child.name == "ViewButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(PassView);
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
        GenomeUI.ReceiveClicked(selectedEntity);
        Debug.Log("Tried to pass to genome");
    }

    public void PassView() {
        if (isAnimal) {
            Animal toSend = (Animal) selectedEntity;
            ViewUI.ReceiveClicked(toSend);
        } else { World.DisplayError(); }
        Debug.Log("Tried to pass to camera");
    }

    public void PassNet() {
        if (isAnimal) {
            Animal toSend = (Animal) selectedEntity;
            NetUI.ReceiveClicked(toSend);
        } else { World.DisplayError(); }
        Debug.Log("Tried to pass to nets");
    }

    public void PassStream() {
        if (isAnimal) {
            Animal toSend = (Animal) selectedEntity;
            StreamUI.ReceiveClicked(toSend);
        } else { World.DisplayError(); }
        Debug.Log("Tried to pass to thought stream");
    }

    public void PassCenter() {
        MainUI.CenterObject(selectedEntity.GetGameObject().transform);
    }

    public void ExitPanel() {
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
}