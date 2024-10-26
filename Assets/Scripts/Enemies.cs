using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    private Rigidbody[] ragdoll;
    private void Start()
    {
        ragdoll = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = true;
        }

    }



    public void Ragdoll()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;

        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Ragdoll(); 
        }
    }
}
