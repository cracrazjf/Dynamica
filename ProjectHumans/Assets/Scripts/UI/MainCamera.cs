// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class MainCamera : MonoBehaviour
// {
//     private Vector3 startPosition = new Vector3(-1, 3, 0);
//     private Vector3 startRotation = new Vector3(0, 90, 0);

//     private float baseClimbSpeed = 4;
//     private float baseMoveSpeed = 10;
//     private float baseRotateSpeed = 100;
//     private float genericHop = 2;
//     private float eyeLevel = 2.55f;

//     private float climbAdjustment = 0f;
//     private float moveAdjustment = 0f;
//     private float rotateAdjustment = 0f;

//     private float xRotation = 0.0f;
//     private float yRotation = 0.0f;
//     private float xTemp, yTemp, zTemp;

//     private bool toggleFlight = true;
//     private bool toggleRotate = false;

//     private bool thirdPerson = true;
//     private bool firstPerson = false;
//     private bool followingAnimal = false;
//     private Camera followedCam;

//     void Start() { transform = new Quaternion(startPosition, startRotation); }

//     Called once a frame
//     void Update() {
//         // Always called in case player goes under plane
//         ResolveClipping();

//         if (followingAnimal) {
//             transform = followedCam.transform;

//         } else if (thirdPerson) { 
//             MoveNormally(baseMoveSpeed);

//             // Fly if legal
//             if (toggleFlight) { MoveAirborne(baseClimbSpeed); }

//             // Hold L Ctrl to rotate
//             if (Input.GetKeyDown(KeyCode.LeftControl)) { RotateCamera(baseRotateSpeed); } 

//         } else if (firstPerson) {
//             MoveNormally(baseMoveSpeed);
//             RotateCamera(baseRotateSpeed);
//         }
        
//         // Checks if reset is needed
//         if (Input.GetKey(KeyCode.Escape)) { CheckReset(); }
//     }
    
//     Flips from first to third person or vice versa
//     public void ToggleView() {
//         firstPerson = !firstPerson;
//         thirdPerson = !thirdPerson;
//         toggleFlight =! toggleFlight;
//     }

//     public void CheckReset() {
//         if (firstPerson) {
//             ToggleView();
//             VerticalBump(genericHop);
//         } else if (followingAnimal) {
//             followingAnimal = false;
//             VerticalBump(genericHop);
//         } else { MainUI.ToggleHelp(); }
//     }

//     public static void CenterObject(Transform passed) {
//         Debug.Log("Centering on object");
//         xTemp = passed.position.x;
//         yTemp = transform.position.y;
//         zTemp = passed.position.z;

//         transform.position = new Vector3(xTemp, yTemp, zTemp);
//     }

//     public static void EnterAnimalCam(Camera passed) {
//         followCam = passed.transform;
//         followingAnimal = true;
//     }

//     public void MoveNormally(float passedSpeed) {
//         // Vertical update
//         xTemp = transform.forward.x * passedSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
//         zTemp = transform.forward.z * passedSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
//         transform.position += new Vector3(xTemp, 0, zTemp);

//         // Horizontal update
//         xTemp = transform.right.x * passedSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
//         zTemp = transform.right.z * passedSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
//         transform.position += new Vector3(xTemp, 0, zTemp);
//     }

//     public void MoveAirborne(float passedSpeed) {
//         // Climb update
//         yTemp = 0f;
//         if (Input.GetKey(KeyCode.Space)) {
//             yTemp = transform.position.y * passedSpeed * Time.deltaTime;
//             transform.position += new Vector3(0, yTemp, 0);
//         }

//         // Descend update
//         if (Input.GetKey(KeyCode.LeftShift)) {
//             if (transform.position.y > 1) {
//                 yTemp = yTemp * passedSpeed * Time.deltaTime;
//                 transform.position -= new Vector3(0, yTemp, 0);
//             }
//         }
//     }

//     public void RotateCamera(float passedSpeed) {
//         xRotation += Input.GetAxis("Mouse X") * passedSpeed  * Time.deltaTime;
//         yRotation += Input.GetAxis("Mouse Y") * passedSpeed  * Time.deltaTime;

//         transform.localRotation = Quaternion.AngleAxis(xRotation, Vector3.up);
//         transform.localRotation *= Quaternion.AngleAxis(yRotation, Vector3.left);
//     }

//     public void ResolveAltitude() {
//         yTemp = transform.position.y;
//         // Checks for underground
//         if (yTemp < 0) { VerticalBump(yTemp * -1); }

//         // Checks for eye level
//         if (firstPerson && yTemp != eyeLevel) { VerticalBump(-yTemp + eyeLevel); }
//     }

//     public void VerticalBump(float height) {
//         transform.position += new Vector3(0, height, 0);
//     }
// }