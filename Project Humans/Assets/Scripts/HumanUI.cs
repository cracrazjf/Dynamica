using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanUI : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    bool thirdPersonView = true;
    public Transform temp = null;
    public Vector3 CameraOffset;
    private Transform _selection;
    [SerializeField] private string selectableTag1 = "human";

    private void Start()
    {
        //GameObject theWorld = GameObject.Find("World");
        //World worldScript = theWorld.GetComponent<worldScript>();
        thirdPersonCamera = Camera.main;
        CameraOffset = new Vector3(0, 5, -6);
        thirdPersonCamera.enabled = thirdPersonView;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Tps.transform.position = temp.position + CameraOffset;
            thirdPersonView = !thirdPersonView;
            thirdPersonCamera.enabled = thirdPersonView;
            firstPersonCamera.enabled = !thirdPersonView;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var sel = hit.transform;
                firstPersonCamera = sel.GetComponentInChildren<Camera>();// get the camera in that person
                if (sel.CompareTag(selectableTag1))
                {
                    temp = hit.transform;
                    //worldScript.HumanInfoInstance.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    _selection = sel;
                }
            }
        }
    }
}