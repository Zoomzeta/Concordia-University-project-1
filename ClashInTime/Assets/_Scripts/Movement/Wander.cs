using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float wanderDegreesDelta = 180;
    public float wanderInterval = 0.5f;
    protected float wanderTimer = 0;

    private Vector3 lastWanderDirection;
    private Vector3 lastDisplacement;
    private float wanderSpeed;
    private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        wanderSpeed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        WanderAround();

        movement = Vector3.ClampMagnitude(movement, wanderSpeed);
        transform.position += movement * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 70 * Time.deltaTime);
        }
    }

    void WanderAround()
    {
        wanderTimer += Time.deltaTime;

        //Check to see if it's the first wander
        if (lastWanderDirection == Vector3.zero)
        {
            lastWanderDirection = transform.forward.normalized * wanderSpeed;
        }

        if (lastDisplacement == Vector3.zero)
        {
            lastDisplacement = transform.forward;
        }

        Vector3 desiredVelocity = lastDisplacement;

        //If wander timer passes the interval then change wander direction
        if (wanderTimer > wanderInterval)
        {
            float angle = (Random.value - Random.value) * wanderDegreesDelta;
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * lastWanderDirection.normalized;
            Vector3 circleCenter = transform.position + lastDisplacement;
            Vector3 destination = circleCenter + direction.normalized;
            desiredVelocity = destination - transform.position;
            desiredVelocity = desiredVelocity.normalized * wanderSpeed;

            lastDisplacement = desiredVelocity;
            lastWanderDirection = direction;
            wanderTimer = 0;
        }

        movement = desiredVelocity;
    }
}
