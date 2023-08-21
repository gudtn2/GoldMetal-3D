using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrowObject : Bullet
{
    Rigidbody rigid;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor" || other.tag == "Player")
        {

        }
    }
}
