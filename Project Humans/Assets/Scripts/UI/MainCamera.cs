using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    //FEATURES
    //WASD/Arrows:   Movement
    // Space:        Climb
    // Shift:        Drop
    // End:          Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).

    public float cameraSensitivity = 200;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    public bool toggle_check = false;


    /// <summary>
    /// Start is called before the first frame update and sets the starting camera position and angle
    /// </summary>
    void Start() {
        // GameObject mainCam = GameObject.Find("MainCamera");
        // HumanUI accessUI = mainCam.GetComponent<HumanUI>();
        // toggle_check = accessUI.thirdPersonToggle;

        Screen.lockCursor = false;
        transform.position = new Vector3(1, 1, 1);
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Update is called once per frame and adjusts the camera position and angle from mouse input
    /// </summary>
    void Update() {

        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

        //rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        if (toggle_check != true)
        {
            var isolated_value_x = transform.forward.x * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            var isolated_value_z = transform.forward.z * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += new Vector3(isolated_value_x, 0, isolated_value_z);
            isolated_value_x = transform.right.x * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            isolated_value_z = transform.right.z * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            transform.position += new Vector3(isolated_value_x, 0, isolated_value_z);

            var isolated_value_y = 0f;
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

            if (Input.GetKeyDown(KeyCode.End))
            {
                Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
            }
        }
    }

    public void setToggle(bool passed){
        toggle_check = passed;
    }

}