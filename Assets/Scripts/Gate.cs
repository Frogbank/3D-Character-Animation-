using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;

    private Enemies enemy1Script;
    private Enemies enemy2Script;

    private void Start()
    {
        enemy1Script = enemy1.GetComponent<Enemies>();
        enemy2Script = enemy2.GetComponent<Enemies>();

    }

    void Update()
    {
        if (!enemy1Script.alive && !enemy2Script.alive)
        {
            Destroy(gameObject);
        }
    }
}
