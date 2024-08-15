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

    public int speedForward = 2;

    private Transform trans;
    private Rigidbody rigid;
    private float dirZ = 0f;
    private float rotY = 0f;

    // Jump
    private float jumpForce = 5f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 10f;
    private bool isGrounded = true;

    // Dash
    [SerializeField] private bool canDash;
    [SerializeField] private float dashCoolTime = 0f;
    public int dashSpeed = 5;
    public float dashJumpForce = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();

        nowPoseName = new PoseName();
        nowPoseName.left = "Idle";
        nowPoseName.right = "Idle";

        speedForward = 2;

        dirZ = 0f;
        rotY = 0f;

        jumpForce = 5f;
        groundCheckDistance = 10f;

        canDash = true;
        dashCoolTime = 0f;
        dashSpeed = 5;
        dashJumpForce = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Code --------------------------------------------------

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);

            rigid.AddForce(jumpVelocity, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.W))
        {
            MovePlayer(1);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            MovePlayer(0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Dash(1);
        }

        // Real Code --------------------------------------------------

        GroundCheck();

        //this.nowPoseName = poseDetector.gameObject.GetComponent<PoseDetector>().currentPoseName();
        this.nowPoseName.left = poseDetector.gameObject.GetComponent<PoseDetector>().currentLeftPoseName();
        this.nowPoseName.right = poseDetector.gameObject.GetComponent<PoseDetector>().currentRightPoseName();

        if (this.nowPoseName.left == "Forward")
        {
            Debug.Log("Forward");
            MovePlayer(1);
        }
       if (this.nowPoseName.left == "Backward")
        {
            Debug.Log("Backward");
            MovePlayer(-1);
        }
        if (this.nowPoseName.left == "LeftTurn")
        {
            Debug.Log("LeftTurn");
            RotateHead(-1);
        }
        if (this.nowPoseName.left == "RightTurn")
        {
            Debug.Log("RightTurn");
            RotateHead(1);
        }
        if (this.nowPoseName.right == "Jump")
        {
            Debug.Log("Jump");
            Jump();
        }
        if (this.nowPoseName.left == "LeftForward")
        {
            Debug.Log("LeftForward");
            RotateHead(-1);
            MovePlayer(1);
        }
        if (this.nowPoseName.left == "RightForward")
        {
            Debug.Log("RightForward");
            RotateHead(1);
            MovePlayer(1);
        }
        if (this.nowPoseName.left == "LeftBackward")
        {
            Debug.Log("LeftBackward");
            RotateHead(-1);
            MovePlayer(-1);
        }
        if (this.nowPoseName.left == "RightBackward")
        {
            Debug.Log("RightBackward");
            RotateHead(1);
            MovePlayer(-1);
        }
        if (this.nowPoseName.right == "JumpForward")
        {
            Debug.Log("JumpForward");
            MovePlayer(1);
            Jump();
        }
        if (this.nowPoseName.right == "JumpBackorward")
        {
            Debug.Log("JumpBackorward");
            MovePlayer(-1);
            Jump();
        }
        if (this.nowPoseName.right == "ForwardDash")
        {
            Debug.Log("ForwardDash");
            Dash(1);
        }
        /*
        switch (this.nowPoseName)
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
                Jump();
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

            case "ForwardDashSkill":
                Debug.Log("DashSkill");
                Dash(1);
                break;

            default:
                Debug.Log("Idle");
                nowPoseName = "Idle";
                break;
        }*/
    }

    void MovePlayer(int toward)
    {
        Debug.Log("Move " + toward);

        dirZ = toward;  // Forward or Backward

        Vector3 moveDir = new Vector3(0, 0, dirZ * speedForward);

        transform.Translate(moveDir * Time.smoothDeltaTime);
        //rigid.velocity = moveDir;
    }

    void RotateHead(int side)
    {
        Debug.Log("Rotate " + side);

        rotY = side;    // Left or Right

        transform.Rotate(0, rotY, 0, Space.Self);
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f))
        {
            //Debug.Log("ray tag: " + hit.transform.tag);
            if (hit.transform.tag != null)
            {
                isGrounded = true;
                //Debug.Log("ray: ground true");
            }
        }
        else
        {
            isGrounded = false;
            //Debug.Log("ray: ground false");
        }
    }
    void Jump()
    {
        Debug.Log("Jump()");

        if (isGrounded)
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);
            Debug.Log("start jump");
            rigid.AddForce(jumpVelocity, ForceMode.Impulse);
        }
    }

    void Dash(int toward)
    {
        Debug.Log("Dash()");

        if (canDash)
        {
            canDash = false;
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(dashJumpForce * -Physics.gravity.y);
            Vector3 moveDir = new Vector3(0, 0, toward * dashSpeed);
            jumpVelocity += moveDir;

            rigid.AddForce(jumpVelocity, ForceMode.Impulse);
            StartCoroutine("DashCoolTimer");
        }
    }
    IEnumerator DashCoolTimer()
    {
        float maxCoolTime = 3.0f;
        dashCoolTime = 0.0f;
        while (dashCoolTime < maxCoolTime)
        {
            dashCoolTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        canDash = true;
    }
}
