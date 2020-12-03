using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;


public class FOVDetection : MonoBehaviour
{

    //basic stat
     


    // public void OnDrawGizmos() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, 10);
    //     Vector3 fovLine1 = Quaternion.AngleAxis(45,transform.up)* transform.forward * 10;
    //     Vector3 fovLine2 = Quaternion.AngleAxis(-45,transform.up)* transform.forward * 10;

    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawRay(transform.position, fovLine1);
    //     Gizmos.DrawRay(transform.position, fovLine2);

    //     for (int i = 0; i < objects_in_vision.Count; i++) {
    //         Gizmos.color= Color.green;
    //         Gizmos.DrawRay(transform.position, (objects_in_vision[i].transform.position- transform.position).normalized * 10);
    //     }
    // }

    // public RenderTexture currrentRT;

    // public void CamCapture() {
        
    //     Camera cam = GetComponent<Camera>();
    //     currrentRT = RenderTexture.active;
    //     RenderTexture.active = cam.targetTexture;
        

    // }

    // private void Update() {
    //     CamCapture();
    // }

    

    
}
    

