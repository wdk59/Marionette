using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private Transform fwdVec;
    [SerializeField] private GameObject gameOverScreen;
    public float playingTime;
    public float playStartTime;

    // Player State
    private int playerHP = 5;
    [SerializeField] private TMPro.TMP_Text hpTxt;
    private bool haveKey = false;
    private int coinCnt = 0;
    [SerializeField] private TMPro.TMP_Text coinCntTxt;
    private int flashCnt = 0;
    [SerializeField] private TMPro.TMP_Text flashCntTxt;

    public bool isImmortal = false;
    [SerializeField] private float immortalTime = 0f;

    // Move
    public int speedForward = 2;
    private float dirZ = 0f;
    private float rotY = 0f;

    // Jump
    [SerializeField] private bool canJump;
    [SerializeField] private float jumpCoolTime = 0f;
    public float jumpForce = 5f;
    private LayerMask groundLayer;
    private float groundCheckDistance = 10f;
    public bool isGrounded = true;

    // Dash
    [SerializeField] private bool canDash;
    [SerializeField] private float dashCoolTime = 0f;
    public int dashSpeed = 5;
    public float dashJumpForce = 0.2f;

    // Flash
    [SerializeField] private float flashCoolTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        playingTime = 0.0f;
        playStartTime = 0.0f;

        playerHP = 5;
        haveKey = false;
        coinCnt = 0;
        flashCnt = 0;

        isImmortal = false;
        float immortalTime = 0f;

        speedForward = 2;
        dirZ = 0f;
        rotY = 0f;

        canJump = true;
        jumpCoolTime = 0f;
        jumpForce = 5f;
        groundCheckDistance = 10f;

        canDash = true;
        dashCoolTime = 0f;
        dashSpeed = 5;
        dashJumpForce = 0.2f;
    }

    public void Damaged()
    {
        isImmortal = true;
        playerHP--;
        StartCoroutine("ImmortalTimer");

        Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(dashJumpForce * -Physics.gravity.y);
        Vector3 moveDir = new Vector3((fwdVec.transform.position - transform.position).normalized.x * -5f, 0, (fwdVec.transform.position - transform.position).normalized.z * -5f);
        jumpVelocity += moveDir;
        rigid.AddForce(jumpVelocity, ForceMode.Impulse);

        hpTxt.text = "X " + playerHP;
        Debug.Log("HP: " + playerHP);

        if (playerHP <= 0)
        {
            PlayerDead();
        }
    }
    IEnumerator ImmortalTimer()
    {
        float maxImmortalTime = 0.5f;
        immortalTime = 0.0f;
        while (immortalTime < maxImmortalTime)
        {
            immortalTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        isImmortal = false;
    }

    private void PlayerDead()
    {
        gameOverScreen.SetActive(true);

        StartCoroutine("DyingMessage");
    }
    IEnumerator DyingMessage()
    {
        float respawnCool = 3.0f;
        while (respawnCool > 0)
        {
            respawnCool -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        RestartGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GetKey()
    {
        Debug.Log("Get Key");
        haveKey = true;
        // 소리
        // UI
    }

    public void GetHP()
    {
        playerHP++;
        hpTxt.text = "X " + playerHP;
        Debug.Log("HP: " + playerHP);
    }

    public void GetCoin()
    {
        coinCnt++;
        coinCntTxt.text = string.Format("X {0:D2}", coinCnt);
    }

    public void GetFlash()
    {
        flashCnt++;
        flashCntTxt.text = "X " + flashCnt;
    }

    public bool WallCheck(RaycastHit hit)
    {
        bool hasWall = false;
        Debug.DrawRay(transform.position, (fwdVec.position - transform.position).normalized, Color.blue);
        if (hit.transform != null)
        {
            if (hit.transform.tag == "Wall")
            {
                hasWall = true;
            }
        }

        return hasWall;
    }
    public void Flash()
    {
        Debug.Log("Flash()");
        //Debug.Log("flash foward x y z: " + (fwdVec.position - transform.position).normalized);
        //Debug.Log("flash before x y z: " + transform.position);
        if (flashCnt > 0)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, (fwdVec.position - transform.position).normalized, out hit, 1.0f);
            if (WallCheck(hit))
            {
                flashCnt--;
                flashCntTxt.text = "X " + flashCnt;

                Vector3 newDir = (fwdVec.position - transform.position).normalized;
                //Debug.Log("flash dir x y z: " + newDir);
                transform.position = transform.position + newDir * 2;
                //transform.Translate(newDir * 2);
                //transform.position.Set(transform.position.x + newPos.x, 0, transform.position.z + newPos.z);

                StartCoroutine("FlashCoolTimer");
            }
        }
    }
    IEnumerator FlashCoolTimer()
    {
        float maxCoolTime = 0.5f;
        flashCoolTime = 0.0f;
        while (flashCoolTime < maxCoolTime)
        {
            flashCoolTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        canJump = true;
    }

    public void MovePlayer(int toward)
    {
        Debug.Log("Move " + toward);
        /*
        // Rotation handVisual_L.transform.rotation.x
        if (toward == 1)
        {
            if (0 <= poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y && poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y < 90)
            {
                transform.Rotate(0, -1, 0, Space.Self);
            }
            else if (90 < poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y && poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y <= 180)
            {
                transform.Rotate(0, 1, 0, Space.Self);
            }
        } else if (toward == 0)
        {
            if (0 <= poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y && poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y < 90)
            {
                transform.Rotate(0, 1, 0, Space.Self);
            }
            else if (90 < poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y && poseDetector.gameObject.GetComponent<PoseDetector>().handVisual_L.transform.rotation.y <= 180)
            {
                transform.Rotate(0, -1, 0, Space.Self);
            }
        }*/

        // Move
        dirZ = toward;  // Forward or Backward

        Vector3 moveDir = new Vector3(0, 0, dirZ * speedForward);

        transform.Translate(moveDir * Time.smoothDeltaTime);
        //rigid.velocity = moveDir;
    }

    public void RotateHead(int side)
    {
        Debug.Log("Rotate " + side);

        rotY = side;    // Left or Right

        transform.Rotate(0, rotY, 0, Space.Self);
    }

    public void GroundCheck()
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
    public void Jump()
    {
        Debug.Log("Jump()");

        if (isGrounded && canJump)
        {
            canJump = false;
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);

            rigid.AddForce(jumpVelocity, ForceMode.Impulse);
            StartCoroutine("JumpCoolTimer");
        }
    }
    IEnumerator JumpCoolTimer()
    {
        float maxCoolTime = 0.5f;
        jumpCoolTime = 0.0f;
        while (jumpCoolTime < maxCoolTime)
        {
            jumpCoolTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        canJump = true;
    }

    public void Dash(int toward)
    {
        Debug.Log("Dash()");

        if (canDash)
        {
            canDash = false;
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(dashJumpForce * -Physics.gravity.y);
            Vector3 moveDir = new Vector3((fwdVec.transform.position - transform.position).normalized.x * toward * dashSpeed, 0, (fwdVec.transform.position - transform.position).normalized.z * toward * dashSpeed);
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
