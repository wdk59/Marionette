using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroughItemScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().GetFlash();
            Destroy(this.gameObject);
        }
    }
}
