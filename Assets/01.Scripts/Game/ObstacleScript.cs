using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerMovement>().isImmortal)
            {
                collision.gameObject.GetComponent<PlayerMovement>().Damaged();
            }
        }
    }
}
