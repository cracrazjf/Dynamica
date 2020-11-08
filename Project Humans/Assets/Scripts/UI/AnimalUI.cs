using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUI : MonoBehaviour
{
    public Animal selectedAnimal = null;

    /// <value>Used for camera transformations</value>
    Transform temp = null;
    bool togglePanel = true;
    GameObject panel;
    GameObject mainCam;

    public Text hunger;
    public Text thirst;
    public Text sleep;
    public Text stamina;
    public Text health;
    public Text OGName;

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        panel = GameObject.Find("ObjectPanel");
    }
    
    private void Update(){
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Logged a click!");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)) {
               ReceiveAnimal(hit.transform.gameObject); 
            }
        }
    }

    public void ReceiveAnimal(GameObject clicked) {

        if(clicked.tag == "Human") {
            Debug.Log ("Got a human!");

            string process = clicked.name;
            string[] splitName = process.Split(' ');
            int originalIndex = int.Parse(splitName[1]);

            selectedAnimal = World.GetHuman(originalIndex);
        }

        if(selectedAnimal != null) {
            DisplayDrives();
        }
    }

    public void TogglePanel() {
        togglePanel = !togglePanel;
        panel.SetActive(togglePanel);
    }

    public void PassAnimalTransform() {
       Transform to_send =  selectedAnimal.gameObject.transform;
       MainCamera.CenterObject(to_send);
    }

    public void PassAnimalCam() {
       Camera to_send =  selectedAnimal.gameObject.GetComponent<Camera>();
       MainCamera.EnterAnimalCam(to_send);
    }

    public void DisplayDrives(){

        float[] to_display = new float[5];
        DriveSystem passed = selectedAnimal.GetDrive();
        Dictionary<string, float> passedDrives = passed.GetStateValues();

        Debug.Log(passedDrives.ToString());

        hunger.text = passedDrives["hunger"].ToString();
        thirst.text = passedDrives["thirst"].ToString();
        sleep.text = passedDrives["sleepiness"].ToString();
        stamina.text = passedDrives["fatigue"].ToString();
        health.text = passedDrives["health"].ToString();

        OGName.text = selectedAnimal.GetDisplayName();
    }
}