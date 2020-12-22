using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rigidbody;
    Animator animator;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    float movingSpeed;
    float rotateAngle;
    void Update()
    {
        
        animator.SetBool("moving", true);
        animator.SetFloat("Velocity", 0);
        RaycastHit hit;
        Vector3 center = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z)
;        // Ray myRay = new Ray(transform.position, transform.forward);
        // Physics.Raycast(myRay, out hit, 10);
        Physics.SphereCast(center, 1, transform.forward, out hit, 10);
        //Debug.DrawRay(transform.position, transform.forward *10, Color.red);
        if (hit.transform != null) {
            if (Vector3.Distance(hit.transform.position, transform.position) <= 5) {
                movingSpeed = 0;
                animator.SetBool("moving", false);
                rotateAngle = 10;
                
            }
            else{
                rotateAngle = 0;
                movingSpeed = 1;
                
            }
        }
        else {
            rotateAngle = 0;
            movingSpeed = 1;
        }
        transform.Translate(transform.forward * movingSpeed * Time.deltaTime);
        transform.Rotate(0,rotateAngle * Time.deltaTime,0);
    }
    
}

