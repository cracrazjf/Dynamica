using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void CloseAndOpenCanvas(string name)
    {
        Transform targetTransform = GameObject.Find(name).transform;
        if (targetTransform.localScale.y == 1)
        {
            GameObject.Find(name).transform.localScale = new Vector3(1, 0, 1);
        }
        else
        {
            GameObject.Find(name).transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
