using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{

    [SerializeField] private PoseDetector poseDetector;
    [SerializeField] private string nowPoseName = "Idle";

    public int speedForward = 12;

    private Transform tr;
    private float dirZ = 0;
    private float rotY = 0;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        this.nowPoseName = poseDetector.gameObject.GetComponent<PoseDetector>().nowPoseName;
    }

    // Update is called once per frame
    void Update()
    {
        switch(nowPoseName)
        {
            case "Forward":
                Debug.Log("Forward");
                MovePlayer(1);
                break;

            case "Bunny":
                Debug.Log("Test");
                MovePlayer(1);
                break;

            case "Backward":
                Debug.Log("Backward");
                MovePlayer(-1);
                break;

            case "LeftTurn":
                Debug.Log("LeftTurn");
                RotateHead(-1);
                break;

            case "RightTurn":
                Debug.Log("RightTurn");
                RotateHead(1);
                break;

            case "Jump":
                Debug.Log("Jump");
                Jump();
                break;

            case "LeftForward":
                Debug.Log("LeftForward");
                MovePlayer(1);
                RotateHead(-1);
                break;

            case "RightForward":
                Debug.Log("RightForward");
                MovePlayer(1);
                RotateHead(1);
                break;

            case "LeftBackward":
                Debug.Log("LeftBackward");
                MovePlayer(-1);
                RotateHead(-1);
                break;

            case "RightBackward":
                Debug.Log("RightBackward");
                MovePlayer(-1);
                RotateHead(1);
                break;

            case "JumpForward":
                Debug.Log("JumpForward");
                MovePlayer(1);
                Jump();
                break;

            case "JumpBackorward":
                Debug.Log("JumpBackorward");
                MovePlayer(-1);
                Jump();
                break;

            case "DashSkill":
                Debug.Log("DashSkill");
                Dash();
                break;

            default:
                Debug.Log("Idle");
                nowPoseName = "Idle";
                break;
        }
    }

    void MovePlayer(int toward)
    {
        Debug.Log("Move " + toward);

        dirZ = toward;  // Forward or Backward

        Vector3 moveDir = new Vector3(0, 0, dirZ * speedForward);

        transform.Translate(moveDir * Time.smoothDeltaTime);
    }

    void RotateHead(int side)
    {
        Debug.Log("Rotate " + side);

        rotY = side;    // Left or Right

        transform.Rotate(0, rotY, 0, Space.Self);
    }

    void Jump()
    {
        Debug.Log("Jump()");
    }

    void Dash()
    {
        Debug.Log("Dash()");
    }
}
