using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    // Movement content
    private Vector3 startPosition = new Vector3(-1, 3, 0);
    private Quaternion startRotation = new Quaternion(0, 90, 0, 0);

    private float baseClimbSpeed = 4;
    private float baseMoveSpeed = 10;
    private float baseRotateSpeed = 100;
    private float genericHop = 2;
    private static float eyeLevel = 2.55f;

    private float climbAdjustment = 0f;
    private float moveAdjustment = 0f;
    private float rotateAdjustment = 0f;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private static float xTemp, yTemp, zTemp;

    private bool toggleFlight = true;
    private bool toggleRotate = false;

    // Camera content
    private bool thirdPerson = true;
    private bool firstPerson = false;
    private static bool followingAnimal = false;
    private static Camera followedCam;
    private static GameObject mainCam;

    // UI content 
    private GameObject alwaysPanel;
    private GameObject helpObj;
    private GameObject pauseObj;

    private static bool isPaused = false;
    private bool isFF = false;
    private bool toggleHelp;
    
    void Start() {
        InitPanels();
        transform.position = startPosition;
        transform.rotation = startRotation;
        mainCam = GameObject.Find("Main Camera");
    }

    // Called once a frame
    void Update() {
        CheckClick();
        MovePlayer();
    }

    public void InitPanels() {
        alwaysPanel = GameObject.Find("AlwaysPanel");
        helpObj = GameObject.Find("InfoPanel");
        pauseObj = GameObject.Find("PauseText");
        
        helpObj.SetActive(false);
        pauseObj.SetActive(false);
    }

    private void MovePlayer() {
        // Always called in case player goes under plane
        ResolveAltitude();
        if (followingAnimal) {
            transform.position = followedCam.transform.position;
            transform.rotation = followedCam.transform.rotation;

        } else if (thirdPerson) { 
            MoveNormally(baseMoveSpeed);
            // Fly if legal
            if (toggleFlight) { MoveAirborne(baseClimbSpeed); }
            // Hold L Ctrl to rotate
            if (Input.GetKeyDown(KeyCode.LeftControl)) { RotateCamera(baseRotateSpeed); } 

        } else if (firstPerson) {
            MoveNormally(baseMoveSpeed);
            RotateCamera(baseRotateSpeed);
        }
        if (Input.GetKey(KeyCode.Escape)) { CheckReset(); }
    }

    public static void CenterObject(Transform passed) {
        Debug.Log("Centering on object");
        xTemp = passed.position.x;
        zTemp = passed.position.z;

        //https://forum.unity.com/threads/moving-the-camera-to-center-an-object-in-the-screen.219813/
        SetPosition(new Vector3(xTemp, eyeLevel, zTemp));
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
                yTemp = yTemp * passedSpeed * Time.deltaTime;
                transform.position -= new Vector3(0, yTemp, 0);
            }
        }
    }

    public void RotateCamera(float passedSpeed) {
        xRotation += Input.GetAxis("Mouse X") * passedSpeed  * Time.deltaTime;
        yRotation += Input.GetAxis("Mouse Y") * passedSpeed  * Time.deltaTime;

        transform.localRotation = Quaternion.AngleAxis(xRotation, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(yRotation, Vector3.left);
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
                    AnimalUI.ReceiveClicked(countable);
                } else if (objTag == "Plant") {
                    PlantUI.ReceiveClicked(countable);
                } else if (objTag == "Object") {
                    NonlivingUI.ReceiveClicked(countable);
                }
            }
        }
    }

    // Followed by the escape key
    public void CheckReset() {
        if (firstPerson) {
            ToggleView();
            VerticalBump(genericHop);
        } else if (followingAnimal) {
            followingAnimal = false;
            VerticalBump(genericHop);
        } else { ToggleHelp(); }
    }

    // Toggles

    // Flips from first to third person or vice versa
    public void ToggleView() {
        firstPerson = !firstPerson;
        thirdPerson = !thirdPerson;
        toggleFlight =! toggleFlight;
    }

    public void ToggleFF(Button usedButton) {
        isFF= !isFF;
        if (isFF) { usedButton.GetComponentInChildren<Text>().text = ">"; 
        } else { usedButton.GetComponentInChildren<Text>().text = "> >"; }
    }

    public void TogglePause(Button usedButton) {
        isPaused = !isPaused;
        if (isPaused) { pauseObj.SetActive(true);
        } else { pauseObj.SetActive(false); }
    }

    public void ToggleHelp() {
        toggleHelp = !toggleHelp;
        helpObj.SetActive(toggleHelp);
    }

    // Getters and Setters

    public static bool GetPause() { return isPaused; }
    public static void SetPosition(Vector3 passedPos) {mainCam.transform.position = passedPos;}
}