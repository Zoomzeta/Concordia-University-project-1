using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour
{
    [HideInInspector] public float seekSpeed;
    private Vector3 movement;
    public float stopRadius;
    private BasicAnimationController bac;
    [HideInInspector] public Vector3 desiredVelocity;
   // private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        bac = GetComponent<BasicAnimationController>();
        seekSpeed = 5f;
        stopRadius = 2f;
       // rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ArriveTo(Transform target)
    {
        desiredVelocity = target.position - transform.position;
        float distance = desiredVelocity.magnitude;
        desiredVelocity = desiredVelocity.normalized * seekSpeed;
        //desiredVelocity = desiredVelocity - rb.velocity;

        if (distance <= stopRadius)
        {
            desiredVelocity *= 0;
        }



        movement = desiredVelocity;
    }

    public void ArriveScript(Transform target)
    {
        ArriveTo(target);

        movement = Vector3.ClampMagnitude(movement, seekSpeed);
        transform.position += movement * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            bac.StartRunning();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 70 * Time.deltaTime);
        }
        else
        {
            bac.StopRunning();
        }
    }
}
