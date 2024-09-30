using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public GameObject player;
    private Player playerScript;

    void Start() // gets player
    {
        playerScript = player.GetComponent<Player>();
    }


    // turns player to ragdoll when it enters the trigger
    public void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject == player)
        {
            playerScript.Ragdoll();
        }
    }
}
