using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    public List<GameObject> inSight = new List<GameObject>();
    public TestMotor motor;
    public bool facingTarget;
    public float thirst = 10;
    public float rotatedAngle = 0.0f;
    public Vector3 randomPosition = Vector3.negativeInfinity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFOV(transform, 45, 10);
        DecreaseThirst();
        if (Input.GetKey(KeyCode.Space))
        {
            motor.PutDownHand(-1);
        }
        if (Input.GetKey(KeyCode.Return))
        {
            motor.UseHand(-1, -0.6f, 0, -0.3f);
            motor.Consume(-1);
        }
    }

    void DecreaseThirst()
    {
        if (inSight.Count > 0)
        {
            rotatedAngle = 0;
            ReachAndGrab();
        }
        else
        {
            if (rotatedAngle <= 360)
            {
                motor.Rotate(10);
                rotatedAngle += 10 * Time.deltaTime;
            }
            else
            {
                Explore();
            }
        }
    }
    void ReachAndGrab()
    {
        foreach (GameObject x in inSight)
        {
            if (IsReachable(x) || motor.reached)
            {

                if (motor.isCrouching)
                {
                    if (motor.leftHand.childCount > 1)
                    {
                        motor.Stand();
                        if (thirst > 9)
                        {
                            motor.UseHand(-1, -0.5f, 0.6f, -0.3f);
                            motor.Consume(-1);
                            thirst = 0;
                        }
                    }
                    else
                    {
                        motor.UseHand(-1, -1f, 0, -0.5f);
                    }
                }
                else
                {
                    motor.Crouch();
                }
            }
            else
            {
                if (x.CompareTag("Water"))
                {
                    FacePosition(x.transform.position);
                    if (facingTarget)
                    {
                        motor.TakingSteps();
                    }
                }
            }
        }
    }
    void Explore()
    {
        if (randomPosition.Equals(Vector3.negativeInfinity))
        {
            randomPosition = GenerateRandomPos();
            Debug.Log("picked a new position");
            Debug.Log(randomPosition);
        }
        if (Vector3.Distance(transform.position, randomPosition) < 1)
        {
            randomPosition = Vector3.negativeInfinity;
        }
        else
        {
            FacePosition(randomPosition);
            if (IsFacing(randomPosition))
            {
                motor.TakingSteps();
            }
        }

    }
    public void Sleep()
    {
        motor.Lay();
        motor.Sleep();
    }
    public void FacePosition(Vector3 targetPos)
    {
        if (!IsFacing(targetPos))
        {
            if (GetRelativePosition(targetPos) == -1)
            {
                motor.Rotate(-10);
            }
            else
            {
                motor.Rotate(10);
            }
        }
    }
    public int GetRelativePosition(Vector3 targetPos)
    {
        Vector3 relativePosition = transform.InverseTransformPoint(targetPos);
        if (relativePosition.x < 0)
        {
            return -1;
        }
        else if (relativePosition.x > 0)
        {
            return 1;
        }
        return 0;
    }
    public bool IsFacing(Vector3 targetPos)
    {
        float angle = Vector3.Angle(transform.forward, targetPos - transform.position);
        if (angle <= 13f)
        {
            facingTarget = true;
            return true;
        }
        facingTarget = false;
        return false;
    }
    public bool IsReachable(GameObject target)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 1f)
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10);

        Gizmos.color = Color.blue;
    }

    public Vector3 GenerateRandomPos()
    {
        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(-10, 10);
        return new Vector3(transform.position.x + randomX, 0.6f, transform.position.z + randomZ);
    }

    public void UpdateFOV(Transform checkingObject, float maxAngle, float maxRadius)
    {
        LayerMask layermask = ~(1 << 9 | 1 << 8);
        Collider[] overlaps = new Collider[60];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps, layermask);
        inSight.Clear();
        for (int i = 0; i < count + 1; i++)
        {
            if (overlaps[i] != null)
            {
                Vector3 directionBetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                directionBetween.y *= 0;
                float angle = Vector3.Angle(checkingObject.forward, directionBetween);
                if (angle <= maxAngle)
                {
                    inSight.Add(overlaps[i].gameObject);
                    //Ray ray = new Ray(checkingObject.position, overlaps[i].transform.position - checkingObject.position);
                    //RaycastHit hit;
                    //if (Physics.Raycast(ray, out hit, maxRadius))
                    //{

                    //    if (hit.transform == overlaps[i].transform)
                    //    {
                    //        
                    //    }
                //}
                }
            }
        }
    }
}
