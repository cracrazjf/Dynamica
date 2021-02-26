// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class PanelUI : MonoBehaviour {

//     protected CountableObject selectedObject = null;
//     protected static GameObject passed;

//     protected static bool needsUpdate = false;
//     protected bool togglePanel = false;

//     public Button tempButton;
//     protected Button closePanelButton;
//     protected GameObject panel;
//     protected GameObject mainCam;
//     protected GameObject halo;

//     protected Text originalName;
//     protected Text inputName;
//     protected InputField panelNamer;

//     private void Start() {
//         InitPanel();
//         InitNamer();
//     }
    
//     private void Update() {
//         if(togglePanel) {
//             UpdatePanel();
//         }
//     }

//     public void InitXButton(){
//         foreach (Transform child in panel.transform) {
//             if (child.name == "ClosePanelButton") {
//                 closePanelButton = child.gameObject.GetComponent<Button>();
//             } 
//             closePanelButton.onClick.AddListener(ExitPanel);
//         }
//     }

//     public static void ReceiveClicked(GameObject clicked) {
//         // selectedObject = World.GetObject(clicked.name);
//         passed = clicked;
//         needsUpdate = true;
//     }
    
//     public virtual void UpdatePanel(){}

//     public void TogglePanel() {
//         togglePanel = !togglePanel;
//         panel.SetActive(togglePanel);
//         UpdatePanel();
//     }

//     public void CenterObject() {
//        Transform toSend =  selectedObject.gameObject.transform;
//        MainUI.CenterObject(toSend);
//     }

//     public void ExitPanel() {
//         panel.SetActive(false);
//         halo.SetActive(false);
//     }

//     public void Rename() {
//         selectedObject.SetDisplayName(inputName.text);
//         panelNamer.text = "";
//     }
// }