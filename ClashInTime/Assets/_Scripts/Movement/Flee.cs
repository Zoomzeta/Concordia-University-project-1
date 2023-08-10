using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : MonoBehaviour
{
    public Transform target;
    public float fleeSpeed;
    private Vector3 movement;
    public float stopRadius;
    private Rigidbody rb;

    void Awake()
    {
        fleeSpeed = 5f;
        stopRadius = 10f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FleeFrom()
    {
        Vector3 desiredVelocity = transform.position - target.position;
        float distance = desiredVelocity.magnitude;
        desiredVelocity = desiredVelocity.normalized * fleeSpeed;
        desiredVelocity = desiredVelocity - rb.velocity;

        if (distance >= stopRadius)
        {
            desiredVelocity *= 0;
        }

        movement = desiredVelocity;
    }

    public void FleeScript()
    {
        FleeFrom();

        movement = Vector3.ClampMagnitude(movement, fleeSpeed);
        transform.position += movement * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 70 * Time.deltaTime);
        }
    }
}
