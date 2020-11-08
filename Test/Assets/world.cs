using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class world : MonoBehaviour
{
    public GameObject gameobject;
    public GameObject prefab;
    public Transform mesh;
    public Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    { 
        startPosition =new Vector3(2.53f, 0.61f,-9.74f);
        prefab = Resources.Load("man_astronaut", typeof(GameObject)) as GameObject;
        gameobject = Instantiate(prefab,startPosition,Quaternion.identity);
        

    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            mesh = gameObject.transform.GetChild(0).GetChild(0);
        }
    }
}
