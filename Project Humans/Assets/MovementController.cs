using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetPosition;
    public GameObject testObject;
    Rigidbody rigidbody;
    Animator animator;
    bool IsFacingToObject;
    List<GameObject> objects_in_vision = new List<GameObject>();
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    float movingSpeed;
    float rotateAngle;

    private void Update()
    {
        Debug.DrawRay(transform.position,Vector3.forward *10, Color.green);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        InFov(transform, 45, 10);
        if (objects_in_vision.Count > 0)
        {
            GameObject target = objects_in_vision[0];
            targetPosition = target.transform.position;
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= 1)
            {
                animator.SetTrigger("pickUp");
                animator.SetFloat("left/rightHand", 0);
            }
            else
            {
                IsFacingTowardObejct(target.transform.position);
                if (IsFacingToObject)
                {
                    Debug.Log("translating");
                    rigidbody.MovePosition(transform.forward * 0.01f);
                }
                else
                {
                    FacingTowardObejct(target.transform.position);
                }
            }
            
        }
       
    }
    void OnAnimatorIK()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Picking Up"))
        {

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPosition);

        }
    }

    public float CalculateRotationAngle(Vector3 passedPosition)
    {
        Vector3 targetDirection = passedPosition - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);
        return angle;
    }
    public int CalculateRelativePosition(Vector3 passedPosition)
    {
        Vector3 relativePosition = transform.InverseTransformPoint(passedPosition);
        if (relativePosition.x < 0)
        {
            return -1;
        }
        else if (relativePosition.x > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public void IsFacingTowardObejct(Vector3 passedPosition)
    {
        float angle = CalculateRotationAngle(passedPosition);
        if (angle < 0.1f)
        {
            angle = 0;
        }
        if (angle == 0)
        {
            IsFacingToObject = true;
        }
        else
        {
            IsFacingToObject = false;
        }
    }
    public void FacingTowardObejct(Vector3 passedPosition)
    {
        if (CalculateRelativePosition(passedPosition) == -1)
        {
            transform.Rotate(0.0f,-0.05f,0.0f);
        }
        else
        {
            transform.Rotate(0.0f, 0.05f, 0.0f);
        }
    }

    public void InFov(Transform checkingObject, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        objects_in_vision.Clear();

        for (int i = 0; i < count + 1; i++)
        {

            if (overlaps[i] != null)
            {
                Vector3 directionBetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                directionBetween.y *= 0;

                float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxAngle)
                {
                    Ray ray = new Ray(checkingObject.position, overlaps[i].transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
                        if (hit.transform == overlaps[i].transform)
                        {
                            if (!objects_in_vision.Contains(overlaps[i].gameObject) && overlaps[i].tag != "ground")
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

