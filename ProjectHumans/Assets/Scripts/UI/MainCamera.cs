using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : MonoBehaviour
{
    // FEATURES
    // WASD/Arrows:   Movement
    // Space:         Climb
    // Shift:         Drop
    // G:             Toggle bird's eye view
    public float cameraSensitivity = 100;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private bool birdView = true;
    private bool toggleRotate = false;

    private static bool brainMode = false;

    private static Transform toSwitch;
    private static bool resetPos = false;
    private static bool switchCenter = false;


    /// <summary>
    /// Start is called before the first frame update and sets the starting camera position and angle
    /// </summary>
    void Start() {
        transform.position = new Vector3(-1, 3, 0);
        transform.localEulerAngles = new Vector3(0, 90, 0);
    }

    /// <summary>
    /// Update is called once per frame and adjusts the camera position and angle from mouse input
    /// </summary>
    void Update() {


        //check for clipping
        var isolated_value_y = transform.position.y;
        if(isolated_value_y < 0) {
            transform.position += new Vector3(0, -1f * isolated_value_y, 0);
        }


        //normal movement
        if(!brainMode){
            var isolated_value_x = transform.forward.x * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            var isolated_value_z = transform.forward.z * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += new Vector3(isolated_value_x, 0, isolated_value_z);
            isolated_value_x = transform.right.x * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            isolated_value_z = transform.right.z * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            transform.position += new Vector3(isolated_value_x, 0, isolated_value_z);
        }

        if(Input.GetKey(KeyCode.Escape)){
            ToggleReset();
        }

        //flying
        if(birdView){
            if(resetPos){
                ResetPos(switchCenter);
            }

            isolated_value_y = 0f;
            if (Input.GetKey(KeyCode.Space))
            {
                isolated_value_y = transform.position.y * climbSpeed * Time.deltaTime;
                transform.position += new Vector3(0, isolated_value_y, 0);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (transform.position.y > 1) {
                    isolated_value_y = transform.position.y * climbSpeed * Time.deltaTime;
                    transform.position -= new Vector3(0, isolated_value_y, 0);
                }
            }

            //hold L Ctrl to rotate in sky mode
            if(Input.GetKeyDown(KeyCode.LeftControl)) {
                toggleRotate = !toggleRotate;
            }


            if (toggleRotate) {
                rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * 10 * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * 10 * Time.deltaTime;

                transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
                transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
            }
        }

        //walking
        if (!birdView)
        {
            var tempx = transform.position.x;
            var tempz = transform.position.z;
            transform.position = new Vector3(tempx, 1.55f, tempz);

            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        }
    }

    public void ToggleWalk() {
        Debug.Log("Walk toggle");
        birdView = !birdView;
        toSwitch = transform;
        resetPos = true;
    }

    public static void ToggleBrain() {
        Debug.Log("Brain toggle");
        brainMode = !brainMode;
        
    }

    public void ToggleReset() {
        if(!birdView) {
            resetPos = true;
            toSwitch = transform;
            ToggleWalk();
        }
    }

    public static void CenterObject(Transform passed) {
        Debug.Log("Centering on object");
        toSwitch = passed;
        resetPos = true;
        switchCenter = true;
    }

    public static void EnterAnimalCam(Camera passed) {
        Debug.Log("Entering animal cam");

        
        //ToggleBrain();
        
    }

    public void ResetPos(bool isIndependent) {
        if(isIndependent) {
            var tempx = transform.position.x;
            var tempz = transform.position.z;
            transform.position = new Vector3(tempx, 3, tempz);
            resetPos = false;

        } else {
            var tempx = toSwitch.position.x;
            var tempz = toSwitch.position.z;
            transform.position = new Vector3(tempx, 3, tempz);

            resetPos = false;
            isIndependent = true;
        }
    }
}