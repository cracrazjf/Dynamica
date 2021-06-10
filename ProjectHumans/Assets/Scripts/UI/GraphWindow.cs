using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphWindow : MonoBehaviour {
    [SerializeField] private Sprite nodeSprite;
    private RectTransform graphSpace;
    protected static Entity selectedEntity;
    protected static bool showPanel = false;
    protected static bool needsUpdate = false;
    protected Button tempButton;
    protected GameObject panel;
    protected GameObject header;
    protected GameObject body;

    private void Start() { 
        InitPanel();
        InitButtons();
    }
    
    private void Update() {
        if (needsUpdate) { UpdatePanel(); }
        if (showPanel) {
            panel.SetActive(true);
        } else { panel.SetActive(false); }
    }

    public static void OnAwake() {
        needsUpdate = true;
        showPanel = true;
    }

    public void UpdatePanel(){
        needsUpdate = false;
        CreateNode(new Vector2(100, 100));
    }

    public static void ReceiveClicked(Entity clicked) {
        selectedEntity = clicked;
        OnAwake();
    }

    public void InitPanel() {
        panel = GameObject.Find("GraphPanel");

        foreach (Transform child in panel.transform) {
            if (child.name == "Header") {
                header = child.gameObject;
            } else if (child.name == "Body") {
                body = child.gameObject;
            }
        }

        foreach (Transform child in body.transform) {
            if (child.name == "GraphSpace") {
                graphSpace = child.GetComponent<RectTransform>();
            } 
        }
    }

    public void InitButtons() {
        foreach (Transform child in header.transform) {
            if (child.name == "ClosePanelButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ExitPanel);
            } else if (child.name == "RefreshButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(UpdatePanel);
            }
        }
    }

    private void CreateNode(Vector2 anchoredPos) {
        GameObject activeNode = new GameObject("rectangle", typeof(Image));
        activeNode.transform.SetParent(graphSpace, false);
        activeNode.GetComponent<Image>().sprite = nodeSprite;

        RectTransform rectTransform = activeNode.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPos;
        rectTransform.sizeDelta = new Vector2(10, 10);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }
        
    public void ExitPanel() { showPanel = false; }
}
