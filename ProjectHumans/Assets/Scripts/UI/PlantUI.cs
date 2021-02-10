using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : MonoBehaviour
{
    public Plant selectedPlant = null;

    bool togglePanel = false;
    bool toggleGenePanel = false;
    GameObject panel;
    GameObject genePanel;
    GameObject mainCam;

    public GameObject halo;

    public Text OGName;
    public Text inputName;

    bool showMutable = true;

    public Text displayText;
    public Text muteText;

    Genome displayGenome;
    Phenotype displayPhenotype;

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        panel = GameObject.Find("PlantPanel");
        genePanel = GameObject.Find("GenomePanel");
    }
    
    private void Update(){
        if(toggleGenePanel) {
            DisplayGenome();
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)) {
                halo.SetActive(true);
                Vector3 haloPos = new Vector3 (hit.transform.position.x, 0.1f, hit.transform.position.z);
                halo.transform.position = haloPos;

               ReceivePlant(hit.transform.root.gameObject); 
            }
        }

        if(!togglePanel){
            halo.SetActive(false);
        }
    }

    public void ReceivePlant(GameObject clicked) {
        if(clicked.tag == "Plant") {
            Debug.Log ("Got a plant!");

            selectedPlant = World.GetPlant(clicked.name);
            if (selectedPlant != null) {
                DisplayInfo();
            }

            if(selectedPlant != null) {
                if(Input.GetKeyDown(KeyCode.Return)) {
                    selectedPlant.SetDisplayName(inputName.text);
                }
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

    public void PassPlantTransform() {
       Transform to_send =  selectedPlant.gameObject.transform;
       MainCamera.CenterObject(to_send);
    }

    public void DisplayInfo(){
        panel.SetActive(true);
        OGName.text = selectedPlant.GetName(); //breaks here... prefabs not being added to dict/given identitites -jc
    }

    public void ExitPanel() {
        panel.SetActive(false);
    }

    public void DisplayGenome(){
        displayGenome = selectedPlant.GetGenome();
        displayPhenotype = selectedPlant.GetPhenotype();

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