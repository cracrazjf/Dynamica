using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnimalUI : PanelUI {

    protected Text hunger;
    protected Text thirst;
    protected Text sleep;
    protected Text stamina;
    protected Text health;
    protected Text displayText;
    protected Animal selectedAnimal;

    Button centerObjectButton;
    Button genomeButton;
    Button brainButton;

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

    public void InitPanel() {
        panel = GameObject.Find("AnimalPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "CenterObjectButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(MainUI.CenterObject(passed.transform));
            } else if (child.name == "GenoPanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(GenomeUI.ReceiveClicked(selectedAnimal.gameObject));
            }
        }
    }
    public void InitNamer(){}
}