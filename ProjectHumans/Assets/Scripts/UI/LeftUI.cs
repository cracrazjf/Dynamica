using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LeftUI : MonoBehaviour {

    protected static bool showSearchPanel = false;
    protected static bool showSpawnPanel = false;
    protected static bool updateSearch = false;
    protected static bool updateSpawn = false;
    protected static bool notInit = true;

    protected Button tempButton;
    protected GameObject spawnPanel;
    protected GameObject searchPanel;

    protected Dropdown locationSpawn;
    protected Dropdown speciesSpawn;

    protected Dropdown nameSearch;
    protected Dropdown speciesSearch;
    
   private void Update() {
       
        if (notInit) {
            InitPanels();
            notInit = false;
        } else if (showSearchPanel) { 
            if(updateSearch) {
                UpdateSpeciesNames();
                UpdateSearchNames();
                updateSearch = false;
            }
            searchPanel.SetActive(true);
        }
        if (showSpawnPanel)  { 
            if (updateSpawn) {
                UpdateSpeciesNames();
                updateSpawn = false;
            }
            spawnPanel.SetActive(true);
        }
    }

    public static void WakeSpawn() { 
        showSpawnPanel = true;
        updateSpawn = true;
    }

    public static void WakeSearch() { 
        showSearchPanel = true; 
        updateSearch = true;
    }


    public void InitPanels(){
        spawnPanel = MainUI.GetUXPos("SpawnPanel").gameObject;
        spawnPanel.SetActive(false);
        InitHeaderFooter(spawnPanel.transform, true);

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
                if (isSpawner) { 
                   speciesSpawn = child.gameObject.GetComponent<Dropdown>(); 
                } else { 
                    speciesSearch = child.gameObject.GetComponent<Dropdown>(); 
                    speciesSearch.onValueChanged.AddListener(delegate{ UpdateSearchNames(); });
                    }
            } else if (child.name == "NameDropdown") {
                nameSearch = child.gameObject.GetComponent<Dropdown>();
            } else if (child.name == "LocationDropdown") {
                locationSpawn = child.gameObject.GetComponent<Dropdown>();
            }
        }

        foreach (Transform child in header.transform) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                if (isSpawner) { 
                    tempButton.onClick.AddListener(ExitSpawnPanel); 
                } else { tempButton.onClick.AddListener(ExitSearchPanel); }
            } else if (child.name == "RefreshButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                if (isSpawner) { 
                    tempButton.onClick.AddListener(delegate { updateSpawn = true; }); ; 
                } else { tempButton.onClick.AddListener(delegate { updateSearch = true; });  }
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
        string species = speciesSearch.options[type].text;
        if (location == 1) {
            Debug.Log("Spawning a " + species + " at this location");
            Vector3 toSend = MainUI.GetCamPos();
            World.AddEntity(species, toSend);
        } else {
            Debug.Log("Spawning a " + species + " at a random location");
            World.AddEntity(species, null);
        }
    }

    // This method is only used for the search panel
    public void UpdateSearchNames() {
        
        int value = speciesSearch.value;
        string species = speciesSearch.options[value].text;
        Population tempPop = World.GetPopulation(species);
        List<string> names = tempPop.GetEntityNames();

        nameSearch.ClearOptions();
        nameSearch.AddOptions(names);
    }

    // This method is used by both panels
    public void UpdateSpeciesNames() {
        List<string> populationNames = World.GetPopulationNames();
        
        if (showSearchPanel) {
            speciesSearch.ClearOptions();
            speciesSearch.AddOptions(populationNames);
        }

        if (showSpawnPanel) {
            speciesSpawn.ClearOptions();
            speciesSpawn.AddOptions(populationNames);
        }
    }

    public void SearchInput() {
        int value = nameSearch.value;
        string name = nameSearch.options[value].text;

        Entity toPass = World.GetEntity(name);
        Debug.Log("Centering on " + name);
        EntityUI.ReceiveClicked(toPass);
        MainUI.CenterObject(toPass.GetGameObject().transform);

    }
}