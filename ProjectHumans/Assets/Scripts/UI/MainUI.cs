using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    // Movement content
    private float baseClimbSpeed = 8;
    private float baseMoveSpeed = 20;
    private float baseRotateSpeed = 120;
    private static float eyeLevel = 2.55f;

    private float climbAdjustment = 0.5f;
    private float moveAdjustment = 0.5f;
    private float rotateAdjustment = 0.5f;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private static float xTemp, yTemp, zTemp;
    
    // Camera content
    private static Vector3 toSet;
    private static GameObject mainCam;
    private static Transform centeredTransform;
    
    // UI content
    private static Dictionary<string, GameObject> UXDict = new Dictionary<string, GameObject>();
    private static Dictionary<string, bool> toggleDict = new Dictionary<string, bool>();

    public GameObject movePub;
    public GameObject climbPub;
    public GameObject rotatePub;
    protected Button tempButton;

    public static void CheckStart() {
        if (World.initWorld == false) {
            IntroUI.ToggleUpdate();
        }
    }
    
    void Start() {
        toggleDict.Add("isFF", false);
        toggleDict.Add("isAwake", false);
        toggleDict.Add("isPaused", false);
        toggleDict.Add("showInfo", false);
        toggleDict.Add("showOptions", false);
        toggleDict.Add("isFollowing", false);
        toggleDict.Add("isCentered", false);
        toggleDict.Add("canRotate", false);
        toggleDict.Add("canMove", false);
        toggleDict.Add("canFly", true);
        InitPanels();
        InitButtons();
        mainCam = GameObject.Find("Main Camera");
    }

    public static void WakeUp() { 
        SetToggle("isAwake", true);
        ToggleUX("AlwaysPanel", true);
        Toggle("canMove");
    }

    // Called once a frame
    void Update() {
        CheckClick();
        MovePlayer();

        if (Check("isCentered")) { mainCam.transform.position = toSet; }
    }

    public void InitPanels() {
        AddChildrenUX("RightOverlay");
        AddChildrenUX("LeftOverlay");
        AddChildrenUX("CenterOverlay");
        AddChildrenUX("FreeOverlay");
        
        UXDict["IntroPanel"].SetActive(true);
    }

    public void AddChildrenUX(string parent){
        Transform temp = GameObject.Find(parent).transform;

        foreach (Transform child in temp) {
            UXDict.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    public void InitButtons() {
        foreach (Transform child in UXDict["AlwaysPanel"].transform) {
            if (child.name == "PauseButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(TogglePause);
            } else if (child.name == "FFButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleFF);
            } else if (child.name == "InfoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleInfo);
            } else if (child.name == "AddButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleAdd);
            }
        }

        // Init info panel buttons
        foreach (Transform child in UXDict["InfoPanel"].transform) {
            if (child.name == "Header") {
                foreach (Transform grandchild in child) {
                    if (grandchild.name == "SettingsButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(ToggleOptions);
                    }
                }
            } else if (child.name == "Footer") {
                foreach (Transform grandchild in child) {
                    if (grandchild.name == "CloseInfoButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(ToggleInfo);
                    } else if (grandchild.name == "ExitButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(QuitPlay);
                    }
                }
            }
        }

        // Init option panel buttons
        foreach (Transform child in UXDict["OptionsPanel"].transform) {
            if (child.name == "Header") {
                foreach (Transform grandchild in child) {
                    if (grandchild.name == "InfoButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(ToggleInfo);
                    }
                }
            } else if (child.name == "Footer") {
                foreach (Transform grandchild in child) {
                    if (grandchild.name == "CloseOptionsButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(ToggleOptions);
                        tempButton.onClick.AddListener(CheckStart);
                    } else if (grandchild.name == "ExitButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(QuitPlay);
                    }
                }
            }
        }
    }

    private void MovePlayer() {
        // Always called in case player goes under plane
        ResolveAltitude();
        if (Check("canMove")) {

            if (Check("isFollowing")) {
                ToggleUX("HumanText", true);
                CenterObject(centeredTransform);
            }

            MoveNormally(baseMoveSpeed * moveAdjustment);
            // Fly if legal
            if (Check("canFly")) { MoveAirborne(baseClimbSpeed * climbAdjustment); }
            // Hold R Click to rotate
            if (Input.GetMouseButtonDown(1)) {
                     
                SetToggle("canRotate", true); 
            } else if (Input.GetMouseButtonUp(1)) {
                    
                SetToggle("canRotate", false);
            }
            //Actually call the function
            if (Check("canRotate")) { RotateCamera(baseRotateSpeed * rotateAdjustment); }

            
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { CheckReset(); }
    }

    public static void CenterObject(Transform passed) {
        Debug.Log("Centering on object");
        centeredTransform = passed;
        xTemp = passed.position.x;
        zTemp = passed.position.z;

        //https://forum.unity.com/threads/moving-the-camera-to-center-an-object-in-the-screen.219813/
        toSet = new Vector3(xTemp, eyeLevel, zTemp);
        SetToggle("isFollowing", true);
        SetToggle("isCentered", true); 
    }

    public void MoveNormally(float passedSpeed) {
        // Vertical update
        xTemp = transform.forward.x * passedSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        zTemp = transform.forward.z * passedSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += new Vector3(xTemp, 0, zTemp);

        // Horizontal update
        xTemp = transform.right.x * passedSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        zTemp = transform.right.z * passedSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.position += new Vector3(xTemp, 0, zTemp);
        if (xTemp != 0 || zTemp!= 0) {
            SetToggle("isCentered", false);
            SetToggle("isFollowing", false);
        }
    }

    public void MoveAirborne(float passedSpeed) {
        yTemp = 0f;
        // Climb update
        if (Input.GetKey(KeyCode.Space)) {
            yTemp = transform.position.y * passedSpeed * Time.deltaTime;
            transform.position += new Vector3(0, yTemp, 0);
        }

        // Descend update
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (transform.position.y > 1) {
                yTemp = transform.position.y * passedSpeed * Time.deltaTime;
                transform.position -= new Vector3(0, yTemp, 0);
            }
        }
    }

    public void RotateCamera(float passedSpeed) {
        xRotation += Input.GetAxis("Mouse X") * passedSpeed  * Time.deltaTime;
        yRotation += Input.GetAxis("Mouse Y") * passedSpeed  * Time.deltaTime;

        transform.rotation = Quaternion.AngleAxis(xRotation, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(yRotation, Vector3.left);
    }

    public void ResolveAltitude() {
        yTemp = transform.position.y;
        // Checks for underground
        if (yTemp < 0) { VerticalBump(yTemp * -1); }
    }

    public void VerticalBump(float height) { transform.position += new Vector3(0, height, 0); }

    // Listens for clicks; if there's a click this function checks whether it hit something and ensures relevant info shows
    public void CheckClick() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)) {
                // Gets the clicked object and identifies its type from the tag before waking up the relevant panel
                GameObject countable = hit.transform.root.gameObject;
                string objTag = countable.tag;

                if (objTag == "Human" || objTag == "Animal") {
                    EntityUI.ReceiveClicked(countable, true);
                } else { EntityUI.ReceiveClicked(countable, false); }
            }
        }
    }

    public void UpdateRotateSensitivity() {
        float toUpdate = rotatePub.GetComponent<Slider>().value;
        rotateAdjustment = toUpdate;
    }

    public void UpdateMoveSensitivity() {
        float toUpdate = movePub.GetComponent<Slider>().value;
        moveAdjustment = toUpdate;
    }

    public void UpdateClimbSensitivity() {
        float toUpdate = climbPub.GetComponent<Slider>().value;
        climbAdjustment = toUpdate;
    }

    // Followed by the escape key
    public void CheckReset() {
        SetToggle("canRotate", false);

        if (!Check("isAwake")) { QuitPlay(); }
        
        if (Check("isFollowing")) {
            ToggleUX("BrainText", false);
            Toggle("isFollowing");
            VerticalBump(2f);
            
        } else { ToggleInfo(); }
        
    }

    public void QuitPlay() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public static Transform GetUXPos(string name) {
        Transform toSend = null;
        if (UXDict.ContainsKey(name)) {
            toSend = UXDict[name].transform;
        }
        return toSend;
    }

    // Toggles

    public void ToggleFF() { Toggle("isFF"); }

    public void TogglePause() {
        Toggle("isPaused");
        ToggleUX("PauseText", Check("isPaused"));
    }


    public void ToggleInfo() {
        Toggle("showInfo");
        ToggleUX("InfoPanel", Check("showInfo"));
        ToggleUX("OptionsPanel", false);
    }

    public void ToggleAdd() {
        LeftUI.WakeUp();
    }

    public void ToggleOptions() {
        Toggle("showOptions");
        ToggleUX("OptionsPanel", Check("showOptions"));
        ToggleUX("InfoPanel", false);
    }

    public static void ToggleUX(string name, bool toSet) {
        if (UXDict.ContainsKey(name)) {
            UXDict[name].SetActive(toSet);
        }
    }

    public static void SetToggle(string name, bool toSet) {
        if (toggleDict.ContainsKey(name)) {
            toggleDict[name] = toSet;
        }
    }

    public static void Toggle(string name) {
        if (toggleDict.ContainsKey(name)) {
            bool antithesis = !toggleDict[name];
            toggleDict[name] = antithesis;
        }
    }

    public static bool Check(string toggle) {
        if (toggleDict.ContainsKey(toggle)) {
            return toggleDict[toggle];
        } else {
            Debug.Log("Key does not exist: " + toggle);
            return false;
        }
    }
}