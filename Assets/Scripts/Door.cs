using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 force = new Vector3(0, 0, -100);
            rb.AddForce(force);
        }
    }
}
