using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public GameObject player;
    private Player playerScript;

    void Start()
    {
        playerScript = player.GetComponent<Player>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerScript.Ragdoll();
        }
    }
}
