using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 0.5f)
        {
            Vector3 dir = ((-transform.up - transform.forward) / 2).normalized;
            transform.Translate(dir * 1 * Time.deltaTime, Space.World);

        }
        
    }
}
