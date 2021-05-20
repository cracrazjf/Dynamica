using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    // Movement content
    private Vector3 startPosition = new Vector3(-1, 3, 0);
    private Quaternion startRotation = new Quaternion(0, 90, 0, 0);

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

    private bool toggleFlight = true;
    private bool toggleRotate = false;
    private bool toggleMovement = false;

    // Camera content
    private bool thirdPerson = true;
    private bool firstPerson = false;
    private static bool followingAnimal = false;
    private static Camera followedCam;
    private static GameObject mainCam;
    private static Vector3 toSet;
    private static bool centeredOn = false;

    // UI content 
    private GameObject alwaysPanel;
    private GameObject infoPanel;
    private GameObject optionPanel;
    private GameObject startPanel;

    private GameObject pauseObj;
    private GameObject bannerObj;
    private Slider rotationSlider;
    public GameObject rotatePub;
    private Slider movementSlider;
    public GameObject movePub;
    private Slider climbSlider;
    public GameObject climbPub;
    protected Button tempButton;

    protected static bool needsUpdate = false;
    private static bool isPaused = false;
    private bool isFF = false;
    private bool isAwake = false;
    private bool toggleHelp = false;
    private bool toggleOptions = false;
    
    void Start() {
        InitPanels();
        transform.position = startPosition;
        mainCam = GameObject.Find("Main Camera");
    }

    public void OnAwake() {
        isAwake = true;
        alwaysPanel.SetActive(true);
        toggleMovement = true;
    }

    public static void ToggleUpdate() {
        needsUpdate = !needsUpdate;
    }

    // Called once a frame
    void Update() {
        if (needsUpdate) {
            OnAwake();
        }
        CheckClick();
        MovePlayer();

        if (centeredOn) { SetPosition(toSet); }

        if(toggleHelp && toggleRotate) { ToggleRotate(); }

        if (toggleOptions && toggleRotate) { ToggleRotate(); }
    }

    public void InitPanels() {
        alwaysPanel = GameObject.Find("AlwaysPanel");
        infoPanel = GameObject.Find("InfoPanel");
        optionPanel = GameObject.Find("OptionsPanel");
        startPanel = GameObject.Find("StartPanel");
        pauseObj = GameObject.Find("PauseText");
        bannerObj = GameObject.Find("BrainText");
        
        alwaysPanel.SetActive(false);
        infoPanel.SetActive(false);
        optionPanel.SetActive(false);
        pauseObj.SetActive(false);
        bannerObj.SetActive(false);

        InitButtons();
    }

    public void InitButtons() {
        foreach (Transform child in alwaysPanel.transform) {
            if (child.name == "PauseButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(TogglePause);
            } else if (child.name == "FFButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleFF);
            } else if (child.name == "InfoButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleHelp);
            } else if (child.name == "WalkButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleView);
            } else if (child.name == "AddButton") {
                tempButton = child.gameObject.GetComponent<Button>();
                tempButton.onClick.AddListener(ToggleAdd);
            }
        }

        foreach (Transform child in infoPanel.transform) {
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
                        tempButton.onClick.AddListener(ToggleHelp);
                    } else if (grandchild.name == "ExitButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(QuitPlay);
                    }
                }
            }
        }

        foreach (Transform child in optionPanel.transform) {
            if (child.name == "Header") {
                foreach (Transform grandchild in child) {
                    if (grandchild.name == "InfoButton") {
                        tempButton = grandchild.gameObject.GetComponent<Button>();
                        tempButton.onClick.AddListener(ToggleHelp);
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
        if (toggleMovement) {

            if (followingAnimal) {
                bannerObj.SetActive(true);
                transform.position = followedCam.transform.position;
                transform.rotation = followedCam.transform.rotation;

            } else if (thirdPerson) { 
                MoveNormally(baseMoveSpeed * moveAdjustment);
                // Fly if legal
                if (toggleFlight) { MoveAirborne(baseClimbSpeed * climbAdjustment); }
                // Hold L Ctrl to rotate
                if (Input.GetKeyDown(KeyCode.LeftControl)) { ToggleRotate(); } 
                //Actually call the function
                if(toggleRotate) { RotateCamera(baseRotateSpeed * rotateAdjustment); }

            } else if (firstPerson) {
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
        // Climb update
        yTemp = 0f;
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

        // Checks for eye level
        if (firstPerson && yTemp != eyeLevel) { VerticalBump(-yTemp + eyeLevel); }
    }

    public void VerticalBump(float height) {
        transform.position += new Vector3(0, height, 0);
    }

    // Listens for clicks; if there's a click this function checks whether it hit something and ensures relevant info shows
    public void CheckClick() {

        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Logged a click!");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)) {
                // Gets the clicked object and identifies its type from the tag before waking up the relevant panel
                GameObject countable = hit.transform.root.gameObject;
                string objTag = countable.tag;

                if (objTag == "Human" || objTag == "Animal") {
                    EntityUI.ReceiveClicked(countable, true);
                } else {
                    EntityUI.ReceiveClicked(countable, false);
                }
            }
        }
    }

    public void UpdateRotateSensitivity() {
        rotationSlider = rotatePub.GetComponent<Slider>();
        float toUpdate = rotationSlider.value;
        rotateAdjustment = toUpdate;
    }

    public static void CheckStart() {
        if (World.initWorld == false) {
            IntroUI.ToggleUpdate();
        }
    }

    public void UpdateMoveSensitivity() {
        movementSlider = movePub.GetComponent<Slider>();
        float toUpdate = movementSlider.value;
        moveAdjustment = toUpdate;
    }

    public void UpdateClimbSensitivity() {
        climbSlider = climbPub.GetComponent<Slider>();
        float toUpdate = climbSlider.value;
        climbAdjustment = toUpdate;
    }

    // Followed by the escape key
    public void CheckReset() {
        if(isAwake) {
            if(centeredOn) {
                centeredOn = false;
            }
            if (firstPerson) {
                ToggleView();
                VerticalBump(2f);
            } else if (followingAnimal) {
                bannerObj.SetActive(false);
                followingAnimal = false;
                VerticalBump(2f);
            } else { ToggleHelp(); }
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

    // Flips from first to third person or vice versa
    public void ToggleView() {
        firstPerson = !firstPerson;
        thirdPerson = !thirdPerson;
        toggleFlight =! toggleFlight;
    }

    public void ToggleFF() {
        isFF= !isFF;
    }

    public void TogglePause() {
        isPaused = !isPaused;
        if (isPaused) { pauseObj.SetActive(true);
        } else { pauseObj.SetActive(false); }
    }

    public void ToggleHelp() {
        toggleHelp = !toggleHelp;
        infoPanel.SetActive(toggleHelp);

        optionPanel.SetActive(false);
        toggleOptions = false;
    }

    public void ToggleAdd() {
        AddUI.OnAwake();
    }

    public void ToggleOptions() {
        toggleOptions = !toggleOptions;
        optionPanel.SetActive(toggleOptions);

        infoPanel.SetActive(false);
        toggleHelp = false;
    }

    public void ToggleRotate() {
        toggleRotate = !toggleRotate;
    }

    // Getters and Setters

    public static bool GetPause() { return isPaused; }
    public void SetPosition(Vector3 passedPos) {transform.position = passedPos;}
}