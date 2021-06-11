using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpawnUI : MonoBehaviour {

    protected static bool showPanel = false;
    protected static bool needsUpdate = false;
    protected Button tempButton;
    protected GameObject panel;
    protected Dropdown locationSpawn;
    protected Dropdown speciesSpawn;

       private void Start() { 
        InitPanel(); 
        showPanel = false;
    }
    
    private void Update() {
        if (needsUpdate) { UpdatePanel(); }
    }

    private void UpdatePanel() {
        if (showPanel) {
            panel.SetActive(true);
        } else { panel.SetActive(false); }


        UpdateSpeciesNames();

        needsUpdate = false;
    }

    public static void WakeSpawn() { 
        showPanel = true; 
        needsUpdate = true;
    }

    public void InitPanel(){
        panel = GameObject.Find("SpawnPanel");
        panel.SetActive(false);
        InitHeaderFooter(panel.transform);
    }

    public void InitHeaderFooter(Transform panel) {
        Transform header = null, body = null, footer = null;

        foreach (Transform child in panel) {
            if (child.name == "Header") {
                header = child;
            } else if (child.name == "Body") {
                body = child;
            } else if (child.name == "Footer") {
                footer = child;
            }
        }

        foreach (Transform child in body.transform) {
            if (child.name == "SpeciesDropdown") {
                speciesSpawn = child.gameObject.GetComponent<Dropdown>(); 
                
            } else if (child.name == "LocationDropdown") {
                locationSpawn = child.gameObject.GetComponent<Dropdown>();
            }
        }

        foreach (Transform child in header.transform) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitSpawnPanel); 
                
            } else if (child.name == "RefreshButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(delegate { needsUpdate = true; }); ; 
                
            }
        }

        foreach (Transform child in footer.transform) {
            if (child.name == "SubmitButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(SpawnInput); 
            } 
        }
    }
        
    public void ExitSpawnPanel() {
        panel.SetActive(false);
        showPanel = false;
    }

    public void SpawnInput() {
        int type = speciesSpawn.value;
        int location = locationSpawn.value;
        string species = speciesSpawn.options[type].text;
        if (location == 1) {
            Debug.Log("Spawning a " + species + " at this location");
            Vector3 toSend = MainUI.GetCamPos();
            World.AddEntity(species, toSend);
        } else {
            Debug.Log("Spawning a " + species + " at a random location");
            World.AddEntity(species, null);
        }
    }

    // This method is used by both panels
    public void UpdateSpeciesNames() {
        List<string> populationNames = World.GetPopulationNames();

        if (showPanel) {
            speciesSpawn.ClearOptions();
            speciesSpawn.AddOptions(populationNames);
        }
    }
}