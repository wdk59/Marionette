using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnterScript : MonoBehaviour
{
    [SerializeField] GameObject startGateObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            startGateObj.SetActive(true);
            other.gameObject.GetComponent<PlayerMovement>().playStartTime = Time.deltaTime;
            Destroy(this.gameObject);
        }
    }
}
