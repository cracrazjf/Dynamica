using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    private GameObject pause;
    private Button[] buttons;
    private bool showHelp;
    private GameObject help;
    
    void Start() {
        help = GameObject.Find("InfoPanel");
        help.SetActive(false);
    }
    
    void Update() { }
    public void ToggleHelp() {
        showHelp = !showHelp;
        help.SetActive(showHelp);
    }
}