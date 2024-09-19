using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    //[SerializeField] private PoseDetector poseDetector;
    [SerializeField] private GameObject poseDetector;
    //[SerializeField] private string nowPoseName = "Idle";

    [System.Serializable]
    public struct PoseName
    {
        public string left;
        public string right;
    }
    private PoseName nowPoseName;

    private Transform trans;
    private Rigidbody rigid;
    private PlayerMovement pMove;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        pMove = GetComponent<PlayerMovement>();

        nowPoseName = new PoseName();
        nowPoseName.left = "Idle";
        nowPoseName.right = "Idle";
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

        //this.nowPoseName = poseDetector.gameObject.GetComponent<PoseDetector>().currentPoseName();
        this.nowPoseName.left = poseDetector.gameObject.GetComponent<PoseDetector>().currentLeftPoseName();
        this.nowPoseName.right = poseDetector.gameObject.GetComponent<PoseDetector>().currentRightPoseName();

        if (this.nowPoseName.left == "Forward")
        {
            Debug.Log("Forward");
            pMove.MovePlayer(1);
        }
        if (this.nowPoseName.left == "Backward")
        {
            Debug.Log("Backward");
            pMove.MovePlayer(-1);
        }
        if (this.nowPoseName.left == "LeftTurn")
        {
            Debug.Log("LeftTurn");
            pMove.RotateHead(-1);
        }
        if (this.nowPoseName.left == "RightTurn")
        {
            Debug.Log("RightTurn");
            pMove.RotateHead(1);
        }
        if (this.nowPoseName.right == "Jump")
        {
            Debug.Log("Jump");
            pMove.Jump();
        }
        if (this.nowPoseName.left == "LeftForward")
        {
            Debug.Log("LeftForward");
            pMove.RotateHead(-1);
            pMove.MovePlayer(1);
        }
        if (this.nowPoseName.left == "RightForward")
        {
            Debug.Log("RightForward");
            pMove.RotateHead(1);
            pMove.MovePlayer(1);
        }
        if (this.nowPoseName.left == "LeftBackward")
        {
            Debug.Log("LeftBackward");
            pMove.RotateHead(-1);
            pMove.MovePlayer(-1);
        }
        if (this.nowPoseName.left == "RightBackward")
        {
            Debug.Log("RightBackward");
            pMove.RotateHead(1);
            pMove.MovePlayer(-1);
        }
        if (this.nowPoseName.right == "JumpForward")
        {
            Debug.Log("JumpForward");
            pMove.MovePlayer(1);
            pMove.Jump();
        }
        if (this.nowPoseName.right == "JumpBackorward")
        {
            Debug.Log("JumpBackorward");
            pMove.MovePlayer(-1);
            pMove.Jump();
        }
        if (this.nowPoseName.right == "ForwardDash")
        {
            Debug.Log("ForwardDash");
            pMove.Dash(1);
        }
        if (this.nowPoseName.right == "Flash")
        {
            Debug.Log("ForwardDash");
            pMove.Flash();
        }
    }
}
