using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUI : PanelUI {

    protected Text hunger;
    protected Text thirst;
    protected Text sleep;
    protected Text stamina;
    protected Text health;
    protected Text displayText;
    protected Animal selectedAnimal;
    
    public void ReceiveClicked(GameObject clicked) {
        selectedAnimal = World.GetAnimal(clicked.name);
        UpdatePanel();
    }

    public void PassAnimalCam() {
       Camera toSend =  selectedAnimal.gameObject.GetComponent<Camera>();
       MainUI.EnterAnimalCam(toSend);
    }

    public void UpdatePanel(){

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

    public void InitPanel(){}
    public void InitNamer(){}
}