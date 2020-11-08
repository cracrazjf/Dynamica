using UnityEngine;
using System.Collections;

public class FootController : MonoBehaviour{
    public Transform leftFoot;
    public Transform rightFoot;

    public Vector3 leftStartPos;

    public Vector3 rightStartPos;

    public Vector3 leftEndPos;

    public Vector3 rightEndPos;

    public int step;

    void Start() {
        step = 2;
        leftFoot = transform.GetChild(0);
        rightFoot = transform.GetChild(1);
        leftStartPos = leftFoot.localPosition;
        rightStartPos = rightFoot.localPosition;
        leftEndPos = new Vector3 (leftStartPos.x, leftStartPos.y, leftStartPos.z + step);
        rightEndPos = new Vector3 (rightStartPos.x, rightStartPos.y, rightStartPos.z + step);
        
    }

    void Update() {
        moveLeftFoot();
        moveRightFoot();
        
    }

    void moveLeftFoot() {
        
        if (Input.GetKeyDown(KeyCode.A)) {
            
            leftFoot.localPosition = leftEndPos;
            if (rightFoot.localPosition != rightStartPos) {
                rightFoot.localPosition = rightStartPos;
            }
            // leftStartPos = leftEndPos;
            // leftEndPos = new Vector3 (leftStartPos.x, leftStartPos.y, leftStartPos.z + step);
        }
        
    }

    void moveRightFoot() {
        if (Input.GetKeyDown(KeyCode.D)) {
            
            rightFoot.localPosition = rightEndPos;
            if (leftFoot.localPosition != leftStartPos) {
                leftFoot.localPosition = leftStartPos;
            }
            // rightStartPos = rightEndPos;
            // rightEndPos = new Vector3 (rightStartPos.x, rightStartPos.y, rightStartPos.z + step);
        }
    }
}