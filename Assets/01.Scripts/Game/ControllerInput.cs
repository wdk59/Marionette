using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{

    //[SerializeField] private PoseDetector poseDetector;
    [SerializeField] private GameObject poseDetector;
    [SerializeField] private string nowPoseName = "Idle";

    public int speedForward = 5;

    private Transform trans;
    private Rigidbody rigid;
    private float dirZ = 0;
    private float rotY = 0;

    // jump
    private float jumpForce = 5f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        this.nowPoseName = poseDetector.gameObject.GetComponent<PoseDetector>().currentPoseName();

        if (this.nowPoseName == "Forward")
        {
            Debug.Log("if forward");
            MovePlayer(1);
        }

        switch(this.nowPoseName)
        {
            case "Forward":
                Debug.Log("Forward");
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
                isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
                if (isGrounded)
                {
                    Jump();
                }
                break;

            case "LeftForward":
                Debug.Log("LeftForward");
                RotateHead(-1);
                MovePlayer(1);
                break;

            case "RightForward":
                Debug.Log("RightForward");
                RotateHead(1);
                MovePlayer(1);
                break;

            case "LeftBackward":
                Debug.Log("LeftBackward");
                RotateHead(-1);
                MovePlayer(-1);
                break;

            case "RightBackward":
                Debug.Log("RightBackward");
                RotateHead(1);
                MovePlayer(-1);
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

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Dash()
    {
        Debug.Log("Dash()");
    }
}
