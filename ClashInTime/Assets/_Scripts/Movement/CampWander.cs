using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampWander : MonoBehaviour
{
    public EnemyCamp camp;
    private BasicAnimationController bac;

    [HideInInspector] public float wanderSpeed;
    private Vector3 movement;
    private Vector3 target;
    private Vector3 desiredVelocity;
    private float distance;
   
    private float timer;
    private float waitTime;
    
    private bool first;

    void Awake()
    {
        first = true;
        wanderSpeed = 1.5f;
        waitTime = Random.Range(1, 5);
        target = Vector3.zero;
        bac = GetComponent<BasicAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void Wander()
    {
        //Check to see if enemy is at destination or if it's his first target area
        if ((distance >= 0 && distance <= 0.1) || target == Vector3.zero)
        {
            //If it's the first target set initial target
            if (first)
            {
                target = new Vector3(Random.Range(camp.startPos.x - camp.campRadius, camp.startPos.x + camp.campRadius), camp.startPos.y, Random.Range(camp.startPos.z - camp.campRadius, camp.startPos.z + camp.campRadius));
                first = false;
            }

            //If reached target set velocity to 0
            desiredVelocity *= 0;

            //Start a count of how long the enemy is at target position
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                //When timer passes certain time limit choose new target
                target = new Vector3(Random.Range(camp.startPos.x - camp.campRadius, camp.startPos.x + camp.campRadius), camp.startPos.y, Random.Range(camp.startPos.z - camp.campRadius, camp.startPos.z + camp.campRadius));
                
                //Set new wait timer
                waitTime = Random.Range(1, 5);

                //Reset timer
                timer = 0f;
            }
        }

        //Calculate velocity
        desiredVelocity = target - transform.position;
        distance = desiredVelocity.magnitude;
        desiredVelocity = desiredVelocity.normalized * wanderSpeed;

        //Apply it to movement
        movement = desiredVelocity;
    }

    public void WanderInCamp()
    {
        //Call wander
        Wander();

        //Check to see if enemy is not at destination
        if (!(distance >= 0 && distance <= 0.1))
        {
            //Move the enemy
            movement = Vector3.ClampMagnitude(movement, wanderSpeed);
            transform.position += movement * Time.deltaTime;

            if (movement != Vector3.zero)
            {
                bac.StartWalking();
                //Make the enemy look where they're going
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 70 * Time.deltaTime);
            }
        }else {
            bac.StopWalking();
        }
    }
}
