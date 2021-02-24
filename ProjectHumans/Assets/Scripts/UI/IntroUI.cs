using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    private GameObject objectPanel;
    private GameObject alwaysPanel;
    private GameObject nonPanel;
    private GameObject animalPanel;
    private GameObject plantPanel;
    private GameObject genomePanel;
    private GameObject brainPanel;
    private GameObject help;
    private GameObject pause;
    private Button[] buttons;
    private bool show_help;
    private static bool is_paused = false;
    private bool is_FF = false;
    

    /// <summary>
    /// Start is called before the first frame update and sets the starting camera position and angle
    /// </summary>
    void Start() {
        help = GameObject.Find("InfoPanel");
        help.SetActive(false);
    }

    /// <summary>
    /// Update is called once per frame and adjusts the camera position and angle from mouse input
    /// </summary>
    void Update() { }
    public void ToggleHelp() {
        show_help = !show_help;
        help.SetActive(show_help);
    }
}