using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {}

    public void Init(float worldSize)
    {
        gameObject.GetComponent<Renderer> ().material.color = new Color(20f/255f, 150f/255f, 50f/255f);
        gameObject.transform.localScale = new Vector3(worldSize, 1.0f, worldSize);
        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update() {}
}
