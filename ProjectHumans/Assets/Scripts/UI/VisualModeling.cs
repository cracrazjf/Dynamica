using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualModeling : MonoBehaviour {
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
        //InitPanel();
        //InitButtons();
    }
    
    private void Update() {
        if (needsUpdate) { UpdatePanel(); }
    }

    public static void OnAwake() {
        needsUpdate = true;
        showPanel = true;
    }

    public void UpdatePanel() {
        if (showPanel) {
            panel.SetActive(true);
        } else { panel.SetActive(false); }
        
        needsUpdate = false;
        CreateNode(new Vector2(100, 100));
        CreateNode(new Vector2(50, 150));
    }

    public static void ReceiveClicked(Entity clicked) {
        selectedEntity = clicked;
        OnAwake();
    }

    public void InitPanel() {
        panel = MainUI.GetUXPos("GraphPanel").gameObject;

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

    private void DrawPoints(List<Vector2> values, bool connectValues) {
        float yMax = 100f;
        float xMax = 250f;

        GameObject lastNode = null;
        for (int i = 0; i < values.Count; i++) {
            float xPos = (values[i].x / xMax);
            float yPos = (values[i].y / yMax);

            GameObject newNode = CreateNode(new Vector2(xPos, yPos));
            if (connectValues) {
                if (lastNode != null) { ConnectNodes(lastNode.GetComponent<RectTransform>().anchoredPosition, newNode.GetComponent<RectTransform>().anchoredPosition); }
            }
            lastNode = newNode;
        }
    }

    private GameObject CreateNode(Vector2 anchoredPos) {
        GameObject activeNode = new GameObject("DrawnNode", typeof(Image));
        activeNode.transform.SetParent(graphSpace, false);
        activeNode.GetComponent<Image>().sprite = nodeSprite;

        RectTransform rectTransform = activeNode.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPos;
        rectTransform.sizeDelta = new Vector2(10, 10);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return activeNode;
    }

    private void ConnectNodes(Vector2 startPos, Vector2 endPos) {
        GameObject drawnLine = new GameObject("PointConnection", typeof(Image));
        drawnLine.transform.SetParent(graphSpace, false);
        drawnLine.GetComponent<Image>().color = new Color();

        RectTransform rectTransform = drawnLine.GetComponent<RectTransform>();
        Vector2 dir = (startPos - endPos).normalized;
        float distance = Vector2.Distance(startPos, endPos);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 2f);
        rectTransform.anchoredPosition = startPos + dir * distance * .5f;
        float normAngle = Mathf.Atan2(dir.x, dir.y);
        rectTransform.localEulerAngles = new Vector3(0, 0, normAngle);
    }
        
    public void ExitPanel() { showPanel = false; }

    public static void LayerFiller(GameObject displayBox, List<float> values, List<string> labels) {
        Color32 lowColor = Color.red;
        Color32 highColor = Color.green;

        // Place HorizontalLayoutGroup
        VerticalLayoutGroup newGroup = displayBox.AddComponent(typeof(VerticalLayoutGroup)) as VerticalLayoutGroup;

        // Make box for each
        for (int i = 0; i < values.Count; i++) {
            string activeLabel = labels[i];
            GameObject activeUnit = new GameObject(activeLabel, typeof(Image));
            activeUnit.transform.SetParent(displayBox.transform, false);
            string filePath = "Materials/UI/AppleIcon";
            activeUnit.GetComponent<Image>().sprite = Resources.Load(filePath, typeof(Sprite)) as Sprite;

            // Attach OnClick
            Button tempButton = activeUnit.AddComponent<Button>() as Button;
            tempButton.onClick.AddListener(delegate { ShowReducedWeights(activeLabel); });

            // color and number item
            Color32 lerpedColor =  Color.white;
            lerpedColor = Color32.Lerp(lowColor, highColor, values[i]);
        }
    }

    public void ShowFullNetwork() {
        // Attach boxes for all VBAD
        // Fill boxes with LayerFiller
        // Attach mouse-over that opens weights for component

        // Other side?
    }
    public static void ShowReducedWeights(string selectedLabel) {
        // Reduce to show based on clicked
    }
}

