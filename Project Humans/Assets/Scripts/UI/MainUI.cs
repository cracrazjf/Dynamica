using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    private GameObject objectPanel;
    private GameObject alwaysPanel;
    private GameObject nonPanel;
    private GameObject animalPanel;
    private GameObject genomePanel;
    private GameObject brainPanel;
    private GameObject help;
    private GameObject pause;
    private Button[] buttons;
    private bool show_help;
    private bool is_paused = false;
    private bool is_FF = false;
    

    /// <summary>
    /// Start is called before the first frame update and sets the starting camera position and angle
    /// </summary>
    void Start() {
        alwaysPanel = GameObject.Find("AlwaysPanel");
        help = GameObject.Find("InfoPanel");
        pause = GameObject.Find("PauseText");
        nonPanel = GameObject.Find("NonlivingPanel");
        animalPanel = GameObject.Find("AnimalPanel");
        genomePanel = GameObject.Find("GenomePanel");
        brainPanel = GameObject.Find("BrainPanel");
        help.SetActive(false);
        pause.SetActive(false);
        nonPanel.SetActive(false);
        animalPanel.SetActive(false);
        genomePanel.SetActive(false);
        brainPanel.SetActive(false);

    }

    /// <summary>
    /// Update is called once per frame and adjusts the camera position and angle from mouse input
    /// </summary>
    void Update() { }

    

    public void ToggleFF() {
        buttons = alwaysPanel.GetComponentsInChildren<Button>();
        Button usedButton = buttons[1];
        is_FF= !is_FF;
        if(is_FF) {
            usedButton.GetComponentInChildren<Text>().text = ">";
            
        } else {
            usedButton.GetComponentInChildren<Text>().text = "> >";
        }
    }

    public void TogglePause() {
        buttons = alwaysPanel.GetComponentsInChildren<Button>();
        Button usedButton = buttons[0];
        is_paused = !is_paused;
        if(is_paused) {
            usedButton.GetComponentInChildren<Text>().text = ">";
            usedButton.GetComponent<SpriteRenderer>().color = Color.green;
            pause.SetActive(true);
            
        } else {
            usedButton.GetComponentInChildren<Text>().text = "| |";
            usedButton.GetComponent<SpriteRenderer>().color = Color.red;
            pause.SetActive(false);
        }
        
    }

    public void ToggleHelp() {
        show_help = !show_help;
        help.SetActive(show_help);
    }


}