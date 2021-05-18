using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AddUI : MonoBehaviour {

    protected static bool needsUpdate = false;
    protected static bool showPanel = false;

    protected Button tempButton;
    protected Button closePanelButton;
    protected GameObject panel;
    protected GameObject header;
    protected GameObject body;
    protected GameObject footer;

    protected Dropdown locationDrop;
    protected Dropdown entityDrop;

    private void Start() {
        InitPanel();
    }
    
   private void Update() {
        if (needsUpdate) { OnAwake(); }
        if (showPanel) { UpdatePanel(); }
    }

    public static void OnAwake() {
        showPanel = true;
    }

    public void UpdatePanel() {
        panel.SetActive(true);
        needsUpdate = false;
    }

    public void InitPanel(){
        panel = GameObject.Find("AddPanel");
        panel.SetActive(false);

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "Body") {
                body = child.gameObject;
            } else if (child.name == "Footer") {
                footer = child.gameObject;
            }
        }
        
        foreach (Transform child in body.transform) {
            if (child.name == "EntityDropdown") {
                entityDrop = child.gameObject.GetComponent<Dropdown>();
            } else if (child.name == "LocationDropdown") {
                locationDrop = child.gameObject.GetComponent<Dropdown>();
            }
        }

        foreach (Transform child in header.transform) {
            
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            } 
        }
    }
        
    public void ExitPanel() {
        panel.SetActive(false);
        showPanel = false;
    }

    public void SpawnInput() {
        int location = locationDrop.value;
        int type = entityDrop.value;

        if (type == 0) {
            Debug.Log("Spawning a human");
            World.AddEntity("Human", null);
        } else {
            Debug.Log("Spawning a tree");
            World.AddEntity("TreeBumpy", null);
        }
    }
}