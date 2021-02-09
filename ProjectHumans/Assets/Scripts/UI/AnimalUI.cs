using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUI : MonoBehaviour
{
    public Animal selectedAnimal = null;

    bool togglePanel = false;
    bool toggleGenePanel = false;
    GameObject panel;
    GameObject genePanel;
    GameObject mainCam;

    public GameObject halo;

    public Text hunger;
    public Text thirst;
    public Text sleep;
    public Text stamina;
    public Text health;
    public Text OGName;

    bool showMutable = true;

    public Text displayText;
    public Text muteText;

    Genome displayGenome;
    Phenotype displayPhenotype;

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        panel = GameObject.Find("AnimalPanel");
        genePanel = GameObject.Find("GenomePanel");
    }
    
    private void Update(){
        if(toggleGenePanel) {
            DisplayGenome();
        }

        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Logged a click!");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)) {
                halo.SetActive(true);
                Vector3 haloPos = new Vector3 (hit.transform.position.x, 0.1f, hit.transform.position.z);
                halo.transform.position = haloPos;

               ReceiveAnimal(hit.transform.root.gameObject); 
            }
        }
    }

    public void ReceiveAnimal(GameObject clicked) {
        
        if(clicked.tag == "Human") {
            Debug.Log ("Got a human!");
            panel.SetActive(true);

            selectedAnimal = World.GetAnimal(clicked.name);
            DisplayDrives();
        }

        if(clicked.tag == "Animal") {
            Debug.Log ("He wants fish!");
            panel.SetActive(true);

            selectedAnimal = World.GetAnimal(clicked.name);
            if(selectedAnimal != null) {
                DisplayDrives();
            }
        }
    }

    public void TogglePanel() {
        togglePanel = !togglePanel;
        panel.SetActive(togglePanel);
    }

    public void ToggleGenePanel() {
        toggleGenePanel = !toggleGenePanel;
        genePanel.SetActive(toggleGenePanel);
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
        DriveSystem passed = selectedAnimal.GetDriveSystem();
        float[] passedDrives = passed.GetDriveStateArray(); //breaks here bc no drives lol - jc

        Debug.Log(passedDrives.ToString());

        // hunger.text = passedDrives["hunger"].ToString();
        // thirst.text = passedDrives["thirst"].ToString();
        // sleep.text = passedDrives["sleepiness"].ToString();
        // stamina.text = passedDrives["fatigue"].ToString();
        // health.text = passedDrives["health"].ToString();

        OGName.text = selectedAnimal.GetDisplayName();
    }

    public void ExitPanel() {
        panel.SetActive(false);
        halo.SetActive(false);
    }

    public void DisplayGenome(){
        displayGenome = selectedAnimal.GetGenome();
        displayPhenotype = selectedAnimal.GetPhenotype();

        string toDisplay = "";

        if(showMutable){
            toDisplay = displayPhenotype.GetDisplayInfo();
        } else {
            toDisplay = displayGenome.GetConstantInfo();
        }
        Debug.Log(toDisplay);
        displayText.text = toDisplay;
    }

    public void ToggleMutable() {
        showMutable = !showMutable;
        DisplayGenome();

        if(showMutable) {
            muteText.text = "Mutable";
        } else {
            muteText.text = "Immutable";
        }
    }
}