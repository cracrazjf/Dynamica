using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    Text textField;

    // Start is called before the first frame update
    void Start() {
        textField = gameObject.GetComponentInParent<Text>();
    }

    // Update is called once per frame
    void Update() {
            string toUpdate = DateTime.Now.ToString("HH:mm:ss");
            textField.text = toUpdate;
    }
}
