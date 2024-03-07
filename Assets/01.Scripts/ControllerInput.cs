using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{

    public int speedForward = 12;
    public int speedSide = 6;

    private Transform tr;
    private float dirX = 0;
    private float dirZ = 0;
    private float dirY = 0;
    private float rotY = 0;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotateHead();
    }

    void MovePlayer()
    {
        dirX = 0;
        dirZ = 0;

        Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        var absX = Mathf.Abs(coord.x);
        var absY = Mathf.Abs(coord.y);

        if (absX > absY)
        {
            if (coord.x > 0) dirX = +1;
            else dirX = -1;
        }
        else
        {
            if (coord.y > 0) dirZ = +1;
            else dirZ = -1;
        }

        Vector3 moveDir = new Vector3(dirX * speedSide, 0, dirZ * speedForward);

        transform.Translate(moveDir * Time.smoothDeltaTime);
    }

    void RotateHead()
    {
        rotY = 0;

        Vector2 coord = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        rotY = coord.x;

        transform.Rotate(0, rotY, 0, Space.Self);
    }

    void Jump()
    {

    }
}
