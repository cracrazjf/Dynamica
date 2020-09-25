using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FOVDetection : MonoBehaviour
{

    //basic stat
    

    public List<GameObject> targets = new List<GameObject>();
        // rename this a little more specific
    public List<GameObject> objects_in_vision = new List<GameObject>();

    public void inFov(Transform checkingObject, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        //Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        objects_in_vision.Clear();
        
        for (int i = 0; i < count +1 ; i++)
        {
            
            if (overlaps[i] != null)
            {
                Debug.Log(count);
                


                    // eliminate this loop

                    Vector3 directionBetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxAngle)
                {
                    Ray ray = new Ray(checkingObject.position, overlaps[i].transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
                        Debug.Log("hit");
                        if (hit.transform == overlaps[i].transform)
                        {

                            if (!objects_in_vision.Contains(overlaps[i].gameObject))
                            {
                                objects_in_vision.Add(overlaps[i].gameObject);

                            }
                        }

                    }

                }






                
            }

        }
    }



}