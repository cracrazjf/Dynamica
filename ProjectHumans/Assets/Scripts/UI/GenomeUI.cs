using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GenomeUI : PanelUI {

    bool showMutable = true;
    public Text displayText;
    public Text muteText;

    protected Genome displayGenome;
    protected Phenotype displayPhenotype;
    protected Animal selectedAnimal;

    public void UpdatePanel(){
        string toDisplay = "";
        displayGenome = selectedAnimal.GetGenome();
        displayPhenotype = selectedAnimal.GetPhenotype();
        displayText.text = toDisplay;
        
        if (showMutable) {
            toDisplay = displayPhenotype.GetDisplayInfo();
        } else { toDisplay = displayGenome.GetConstantInfo(); }
        displayText.text = toDisplay;
    }

    public void ToggleMutable() {
        showMutable = !showMutable;
        UpdatePanel();

        muteText.text = "Immutable";
        if (showMutable) { muteText.text = "Mutable"; } 
    }

    public void InitPanel(){
        panel = GameObject.Find("GenomePanel");
        panel.SetActive(false);
    }
    public void InitNamer(){}
}