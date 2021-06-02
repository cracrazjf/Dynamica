using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour {

    public GameObject humanPub;
    protected static bool needsUpdate = false;
    protected bool showPanel = false;

    protected Button tempButton;
    protected GameObject panel;
    protected GameObject optionPanel;
    protected GameObject header;
    protected GameObject body;
    protected GameObject footer;

    protected Dropdown humanDrop;
    protected Dropdown bodyDrop;
    protected Dropdown biomeDrop;
    

    void Start() { InitPanel(); }
    private void Update() {
        if (needsUpdate) { OnAwake(); }
        if (showPanel) { UpdatePanel(); }
    }

    public void OnAwake() {
        panel.SetActive(true);

        showPanel = true;
        needsUpdate = false;
    }

    public void UpdatePanel(){}

    public void InitPanel(){
        panel = MainUI.GetUXPos("IntroPanel").gameObject;
        optionPanel = MainUI.GetUXPos("OptionsPanel").gameObject;
        panel.SetActive(true);

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
            if (child.name == "HumanDropdown") {
                humanDrop = child.gameObject.GetComponent<Dropdown>();
            } else if (child.name == "HumanBodyDropdown") {
                bodyDrop = child.gameObject.GetComponent<Dropdown>();
            } else if (child.name == "BiomeDropdown") {
                biomeDrop = child.gameObject.GetComponent<Dropdown>();
            }
        }

        foreach (Transform child in header.transform) {
            
            if (child.name == "OptionsButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(OpenOptions);
            } 
        }

        foreach (Transform child in footer.transform) {
            if (child.name == "StartButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(StartWorld);
            } else if (child.name == "ExitButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(QuitPlay);
            }
        }
    }

    public void QuitPlay() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

     public void OpenOptions() {
        optionPanel.SetActive(true);
        ExitPanel();
    }
        
    public void ExitPanel() {
        panel.SetActive(false);
        showPanel = false;
    }

    public void StartWorld() {
        UpdateNumHumans();
        UpdateBiome();
        UpdateAI();
        UpdateHumanBody();
        World.initWorld = true;
        ExitPanel();
    }

    public void UpdateAI() {
        int type = humanDrop.value;
        World.SetHumanAI(type);
    }

    public void UpdateHumanBody() {
        int index = bodyDrop.value;
        string type = bodyDrop.options[index].text;
        World.SetHumanBodies(type);
    }

    public void UpdateBiome() {
        int index = biomeDrop.value;
        string type = biomeDrop.options[index].text;
        World.SetBiome(type);
    }

    public void UpdateNumHumans() {
        Slider humanSlider = humanPub.GetComponent<Slider>();
        int toUpdate = (int) humanSlider.value;
        World.InitNumSpecies("Human", toUpdate);
    }

    public static void ToggleUpdate() {
        needsUpdate = !needsUpdate;
    }
}