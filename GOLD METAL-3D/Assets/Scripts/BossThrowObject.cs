using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrowObject : Bullet
{
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //rigid.AddTorque(transform.right * Time.deltaTime, ForceMode.Acceleration);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor" || other.tag == "Player")
        {

        }
    }
}
