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
    private static bool followingAnimal = false;
    private static bool centeredOn = false;
    private static Camera followedCam;
    private static GameObject mainCam;
    private static Vector3 toSet;
    

    // UI content
    private Dictionary<string, GameObject> UXDict = new Dictionary<string, GameObject>();
    private static Dictionary<string, bool> toggleDict = new Dictionary<string, bool>();

    public GameObject rotatePub;
    public GameObject movePub;
    public GameObject climbPub;
    protected Button tempButton;

    protected static bool needsUpdate = false;
    private static bool isPaused = false;


    public static void CheckStart() {
        if (World.initWorld == false) {
            IntroUI.ToggleUpdate();
        }
    }
    
    void Start() {
        toggleDict.Add("isFF", false);
        toggleDict.Add("isPaused", false);
        toggleDict.Add("isAwake", false);
        toggleDict.Add("showInfo", false);
        toggleDict.Add("showOptions", false);
        toggleDict.Add("canRotate", false);
        toggleDict.Add("canMove", false);
        toggleDict.Add("canFly", true);
        toggleDict.Add("view", true);
        InitPanels();
        InitButtons();
        mainCam = GameObject.Find("Main Camera");
    }

    public void OnAwake() {
        Toggle("isAwake");
        ToggleUX("AlwaysPanel", true);
        Toggle("canMove");
    }

    public static void ToggleUpdate() { needsUpdate = !needsUpdate; }

    // Called once a frame
    void Update() {
        if (needsUpdate) { OnAwake(); }
        CheckClick();
        MovePlayer();

        if (centeredOn) { mainCam.transform.position = toSet; }
    }

    public void InitPanels() {
        Transform temp = GameObject.Find("RightOverlay").transform;

        foreach (Transform child in temp) {
            UXDict.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }

        temp = GameObject.Find("CenterOverlay").transform;
        foreach (Transform child in temp) {
            UXDict.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }
        
        temp = GameObject.Find("FreeOverlay").transform;
        foreach (Transform child in temp) {
            UXDict.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }
        
        UXDict["IntroPanel"].SetActive(true);
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
            } else if (child.name == "WalkButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleView);
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

            if (followingAnimal) {
                ToggleUX("HumanText", true);
                transform.position = followedCam.transform.position;
                transform.rotation = followedCam.transform.rotation;

            } else if (!Check("view")) { 
                MoveNormally(baseMoveSpeed * moveAdjustment);
                // Fly if legal
                if (Check("canFly")) { MoveAirborne(baseClimbSpeed * climbAdjustment); }
                // Hold L Ctrl to rotate
                if (Input.GetKeyDown(KeyCode.LeftControl)) { Toggle("canRotate"); } 
                //Actually call the function
                if(Check("canRotate")) { RotateCamera(baseRotateSpeed * rotateAdjustment); }

            } else if (Check("view")) {
                MoveNormally(baseMoveSpeed);
                RotateCamera(baseRotateSpeed);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { CheckReset(); }
    }

    public static void CenterObject(Transform passed) {
        Debug.Log("Centering on object");
        xTemp = passed.position.x;
        zTemp = passed.position.z;

        //https://forum.unity.com/threads/moving-the-camera-to-center-an-object-in-the-screen.219813/
        toSet = new Vector3(xTemp, eyeLevel, zTemp);
        centeredOn = true;
    }

    public static void EnterAnimalCam(Camera passed) {
        followedCam = passed;
        followingAnimal = true;
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
        if (Check("isAwake")) {
            if (centeredOn) { centeredOn = false; }
            if (!Check("view")) {
                ToggleView();
                VerticalBump(2f);
            } else if (followingAnimal) {
                ToggleUX("BrainText", false);
                followingAnimal = false;
                VerticalBump(2f);
            } else { ToggleInfo(); }
        }
    }

    public void QuitPlay() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // Toggles

    public void ToggleView() {
        Toggle("view");
        Toggle("canFly");
    }

    public void ToggleFF() { Toggle("view"); }

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
        AddUI.OnAwake();
    }

    public void ToggleOptions() {
        Toggle("showOptions");
        ToggleUX("OptionsPanel", Check("showOptions"));
        ToggleUX("InfoPanel", false);
    }

    void ToggleUX(string name, bool toSet) {
        if (UXDict.ContainsKey(name)) {
            UXDict[name].SetActive(toSet);
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