using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject player;
    public bool move = false;

    public Vector3 target;
    private Vector3 startPos;

    private void Start() // sets the point to return to
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // moves towards the target position when the player is on the platform
        if (move) 
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
        }
        else // moves back when the player is off the platform
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) // checks when the player gets on the platform
    {
        if(other.gameObject == player)
        {
            player.transform.parent = this.transform; // sets the platform as the player's parent object


            move = true; // gets the platform to move
        }
    }

    private void OnTriggerExit(Collider other) // checks when the player gets off the platform
    {
        if(other.gameObject == player)
        {
            player.transform.parent = null; // removes the platform as the player's parent


            move = false;
        }
    }
}
