using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeExitScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("도챡쿠");
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().playingTime = Time.deltaTime - other.gameObject.GetComponent<PlayerMovement>().playStartTime;
            Debug.Log("playingTime: "+other.gameObject.GetComponent<PlayerMovement>().playingTime);
            Destroy(this.gameObject);
        }
    }
}
