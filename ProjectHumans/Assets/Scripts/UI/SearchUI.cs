using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SearchUI : MonoBehaviour {

    protected static bool showPanel = false;
    protected static bool needsUpdate = false;
    protected Button tempButton;
    protected GameObject panel;
    protected Dropdown nameSearch;
    protected Dropdown speciesSearch;
    

    private void Start() { 
        InitPanel(); 
        showPanel = false;
    }
    
    private void Update() {
        if (showPanel) {
            panel.SetActive(true);
        } else { panel.SetActive(false); }

        if (needsUpdate) { UpdatePanel(); }
    }

    private void UpdatePanel() {
        UpdateSpeciesNames();
        UpdateSearchNames();

        needsUpdate = false;
    }

    public static void WakeSearch() { 
        showPanel = true; 
        needsUpdate = true;
    }


    public void InitPanel(){
        panel = GameObject.Find("SearchPanel");
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
                speciesSearch = child.gameObject.GetComponent<Dropdown>(); 
                speciesSearch.onValueChanged.AddListener(delegate{ UpdateSearchNames(); });
                
            } else if (child.name == "NameDropdown") {
                nameSearch = child.gameObject.GetComponent<Dropdown>();
            }
        }

        foreach (Transform child in header.transform) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitSearchPanel);
            } else if (child.name == "RefreshButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(delegate { needsUpdate = true; }); 
            }
        }

        foreach (Transform child in footer.transform) {
            if (child.name == "SubmitButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(SearchInput);
            } 
        }
    }
        
    public void ExitSearchPanel() {
        panel.SetActive(false);
        showPanel = false;
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
        
        if (showPanel) {
            speciesSearch.ClearOptions();
            speciesSearch.AddOptions(populationNames);
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