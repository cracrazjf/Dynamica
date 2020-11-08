using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class testScript : MonoBehaviour
{
    public Animator animator;
    public Vector3 righthandPosition;
    public Vector3 righthandRoatation;
    public Transform righthand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "apple")
        {
            other.transform.parent = righthand;
            other.transform.localPosition = righthandPosition;
            other.transform.localEulerAngles = righthandRoatation;
            pickup();   
            other.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("pickup", false);
    }
    public void pickup() {
        if (righthand.childCount > 0) 
        {
            animator.SetBool("pickup", true);
            animator.SetFloat("pickuporsetdown", 0);
        }

    }


}
