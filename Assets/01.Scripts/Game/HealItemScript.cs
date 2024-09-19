using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().GetHP();
            Destroy(this.gameObject);
        }
    }
}
