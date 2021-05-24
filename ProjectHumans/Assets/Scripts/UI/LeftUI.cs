using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LeftUI : MonoBehaviour {

    protected static bool isAwake = false;
    protected static bool showSearchPanel = false;
    protected static bool showSpawnPanel = false;

    protected Button tempButton;
    protected GameObject spawnPanel;
    protected GameObject searchPanel;

    protected Dropdown locationSpawn;
    protected Dropdown speciesSpawn;

    protected Dropdown nameSearch;
    protected Dropdown speciesSearch;

    private void Start() {
        InitPanels();
    }
    
   private void Update() {
        if (isAwake) {

        }
    }

    public static void WakeUp() { isAwake = true; }


    public void InitPanels(){
        spawnPanel = MainUI.GetUXPos("AddPanel").gameObject;
        spawnPanel.SetActive(false);
        InitHeaderFooter(spawnPanel.transform, false);

        searchPanel = MainUI.GetUXPos("SearchPanel").gameObject;
        searchPanel.SetActive(false);
        InitHeaderFooter(searchPanel.transform, false);
    }

    public void InitHeaderFooter(Transform panel, bool isSpawner) {
        Transform header = null, body = null, footer = null;
        Dropdown firstReturn = null, secondReturn = null;

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
                firstReturn = child.gameObject.GetComponent<Dropdown>();
            } else if (child.name == "OtherDropdown") {
                secondReturn = child.gameObject.GetComponent<Dropdown>();
            }
        }

        foreach (Transform child in header.transform) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                if (isSpawner) { 
                    tempButton.onClick.AddListener(ExitSpawnPanel); 
                } else { tempButton.onClick.AddListener(ExitSearchPanel); }
            } 
        }

        foreach (Transform child in footer.transform) {
            if (child.name == "SubmitButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                if (isSpawner) { 
                    tempButton.onClick.AddListener(SpawnInput); 
                } else { tempButton.onClick.AddListener(SearchInput); }
            } 
        }

        speciesSearch = firstReturn;
        nameSearch = secondReturn;

        if (isSpawner) {
            speciesSpawn = firstReturn;
            locationSpawn = secondReturn;
        }
    }
        
    public void ExitSpawnPanel() {
        spawnPanel.SetActive(false);
        showSpawnPanel = false;
    }

    public void ExitSearchPanel() {
        searchPanel.SetActive(false);
        showSearchPanel = false;
    }

    public void SpawnInput() {
        int location = speciesSpawn.value;
        int type = locationSpawn.value;

        if (type == 0) {
            Debug.Log("Spawning a human");
            World.AddEntity("Human", null);
        } else {
            Debug.Log("Spawning a tree");
            World.AddEntity("TreeBumpy", null);
        }
    }

    public void SearchInput() {
        int species = speciesSearch.value;
        int type = nameSearch.value;

    }
}