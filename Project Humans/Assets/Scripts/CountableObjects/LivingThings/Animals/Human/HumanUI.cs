using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanUI : MonoBehaviour
{
    //FEATURES
    //WASD/Arrows:   Movement
    // Space:        Climb
    // Shift:        Drop
    // End:          Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
    // L Click:      Show stats
    // R Click:      Hide stats
    // Q click:      Enter camera
    // G:            Exit human cam

    /// <value>Object to toggle for first person view</value>   
    public Camera firstPersonCamera;
    /// <value>Object to toggle for third person view</value>
    public Camera thirdPersonCamera;
    /// <value>True when camera is set to third person</value>
    public bool thirdPersonToggle = true;
    /// <value>Used for camera transformations</value>
    public Transform temp = null;
    /// <value>Used for camera transformations</value>
    private Transform _selection;
    [SerializeField] private string selectableTag1 = "human";

    GameObject newCanvas;
    Canvas objectStats;
    GameObject panel;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        thirdPersonCamera = Camera.main;
        thirdPersonCamera.enabled = thirdPersonToggle;

        newCanvas = new GameObject("Canvas");
        objectStats = newCanvas.AddComponent<Canvas>();
        objectStats.renderMode = RenderMode.ScreenSpaceOverlay;
        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GraphicRaycaster>();

        panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = Color.white;
    }
    
    /// <summary>
    /// Update is called once per frame and awaits input to call UI functions
    /// </summary>
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (temp != null && thirdPersonToggle == false) {
                firstPersonCamera.transform.position = temp.position;
                thirdPersonToggle = true;
                thirdPersonCamera.enabled = true;
            }    
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKeyDown(KeyCode.Q)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selected = hit.transform;
                temp = firstPersonCamera.transform;
                firstPersonCamera = selected.GetComponentInChildren<Camera>(); // get the camera in that person
                thirdPersonCamera.enabled = false;

            }
        }

        if (Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                var ob_selected = hit.transform.gameObject;
                panel.gameObject.SetActive(true);
                Text toDisplay = panel.AddComponent<Text>();

                if (ob_selected is Human) {
                    Human selected = ob_selected.GetComponent<Human>();
                    string concat = "";
                    for(int i = 0; i < 5; i++) {
                         if(selected./*state_display[i] == 1;*/ StateDisplayList[i] == true) { // I don't know what are we doing here 
                             concat += selected.StateLabelList[i] + ": " + 
                             ((selected.StateValueList[i]).ToString("#.00"));
                            concat += "\n";
                         }
                    }
                    toDisplay.text = concat;
                }
                else if (ob_selected is Apple) {
                    toDisplay.text = "Apple info";
                }
                else if (ob_selected is Water) {
                    toDisplay.text = "Water info";
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            panel.gameObject.SetActive(false);
        }
    }
}