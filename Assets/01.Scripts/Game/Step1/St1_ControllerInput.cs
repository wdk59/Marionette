using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class St1_ControllerInput : MonoBehaviour
{
    //[SerializeField] private PoseDetector poseDetector;
    [SerializeField] private GameObject poseDetector;
    [SerializeField] private string nowPoseName = "Idle";

    private Transform trans;
    private Rigidbody rigid;
    private PlayerMovement pMove;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        pMove = GetComponent<PlayerMovement>();

        nowPoseName = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Code --------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space) && pMove.isGrounded)
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(pMove.jumpForce * -Physics.gravity.y);

            rigid.AddForce(jumpVelocity, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.W))
        {
            pMove.MovePlayer(1);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            pMove.MovePlayer(0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            pMove.MovePlayer(-1);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            pMove.MovePlayer(0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            pMove.RotateHead(-1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            pMove.RotateHead(1);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            pMove.Dash(1);
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            pMove.Flash();
        }

        // Real Code --------------------------------------------------

        pMove.GroundCheck();

        this.nowPoseName = poseDetector.gameObject.GetComponent<St1_PoseDetector>().nowPoseName;

        if (this.nowPoseName == "Forward")
        {
            Debug.Log("Forward");
            pMove.MovePlayer(1);
        }
        if (this.nowPoseName == "Backward")
        {
            Debug.Log("Backward");
            pMove.MovePlayer(-1);
        }
        if (this.nowPoseName == "LeftTurn")
        {
            Debug.Log("LeftTurn");
            pMove.RotateHead(-1);
        }
        if (this.nowPoseName == "RightTurn")
        {
            Debug.Log("RightTurn");
            pMove.RotateHead(1);
        }
        if (this.nowPoseName == "Jump")
        {
            Debug.Log("Jump");
            pMove.Jump();
        }
        if (this.nowPoseName == "LeftForward")
        {
            Debug.Log("LeftForward");
            pMove.RotateHead(-1);
            pMove.MovePlayer(1);
        }
        if (this.nowPoseName == "RightForward")
        {
            Debug.Log("RightForward");
            pMove.RotateHead(1);
            pMove.MovePlayer(1);
        }
        if (this.nowPoseName == "LeftBackward")
        {
            Debug.Log("LeftBackward");
            pMove.RotateHead(-1);
            pMove.MovePlayer(-1);
        }
        if (this.nowPoseName == "RightBackward")
        {
            Debug.Log("RightBackward");
            pMove.RotateHead(1);
            pMove.MovePlayer(-1);
        }
        if (this.nowPoseName == "JumpForward")
        {
            Debug.Log("JumpForward");
            pMove.MovePlayer(1);
            pMove.Jump();
        }
        if (this.nowPoseName == "JumpBackorward")
        {
            Debug.Log("JumpBackorward");
            pMove.MovePlayer(-1);
            pMove.Jump();
        }
        if (this.nowPoseName == "DashSkill")
        {
            Debug.Log("DashSkill");
            pMove.Dash(1);
        }
        if (this.nowPoseName == "Flash")
        {
            Debug.Log("Flash");
            pMove.Flash();
        }
    }
}
