using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    public bool debug;
    //public Transform target;
    public float neighborRadius = 5;
    public float avoidanceRadius = 3.5f;
    public float cohesionFactor = 1.5f;
    public float avoidanceFactor = 2f;
    public float seekSpeed = 3f;

    public Vector3 movement = new Vector3(0f, 0f, 0f);

    private knightmovement km;
    // Start is called before the first frame update
    void Start()
    {
        km = GameObject.Find("Knight").GetComponent<knightmovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Collider[] GetNeighborContext()
    {
        int layerMask = LayerMask.GetMask("Party");
        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius, layerMask);

        if (debug)
            DebugUtil.DrawWireSphere(transform.position, Color.Lerp(Color.white, Color.red, neighbors.Length), neighborRadius);

        return neighbors;
    }

    // This is the force that keeps the swarm together.
    void Cohesion(Collider[] neighbors)
    {
        Vector3 cohesiveMovement = Vector3.zero;
        foreach (Collider neighbor in neighbors)
        {
            if (neighbor.transform == this.transform) continue;

            cohesiveMovement += neighbor.transform.position;
        }

        if (neighbors.Length > 0)
        {
            cohesiveMovement -= transform.position;
            cohesiveMovement /= neighbors.Length;
        }

        movement += cohesiveMovement.normalized * cohesionFactor;
    }

    // This is the force that dictates the spacing of the swarm.
    void Avoidance(Collider[] neighbors)
    {
        Vector3 avoidanceMovement = Vector3.zero;
        float avoidanceRadSqrd = avoidanceRadius * avoidanceRadius;
        foreach (Collider neighbor in neighbors)
        {
            if (neighbor.transform == this.transform) continue;

            Vector3 neighborToAgent = transform.position - neighbor.transform.position;
            if (Vector3.SqrMagnitude(neighborToAgent) <= avoidanceRadSqrd)
            {
                if (neighborToAgent == Vector3.zero)
                {
                    neighborToAgent = Random.insideUnitSphere * 0.1f;
                }

                avoidanceMovement += neighborToAgent;
            }
        }

        if (neighbors.Length > 0)
        {
            avoidanceMovement = avoidanceMovement.normalized / neighbors.Length;
        }

        movement += avoidanceMovement.normalized * avoidanceFactor;
    }

    // This "force" has each of the agents try to synch their orientation.
    void Alignment(Collider[] neighbors)
    {
        Vector3 alignedMovement = Vector3.zero;
        foreach (Collider neighbor in neighbors)
        {
            if (neighbor.transform == this.transform) continue;

            alignedMovement += neighbor.GetComponent<Formation>().movement;
        }

        if (neighbors.Length > 0)
        {
            alignedMovement = alignedMovement.normalized / neighbors.Length;
        }

        movement += alignedMovement.normalized;
    }

    public void StayInFormation(BasicAnimationController anim)
    {
        if (km.direction != Vector3.zero)
        {
            Collider[] neighbors = GetNeighborContext();
            Cohesion(neighbors);
            Avoidance(neighbors);
            //Alignment(neighbors);

            // TODO : steer and align the boid in the direction of the movement
            movement = Vector3.ClampMagnitude(movement, seekSpeed);
            transform.position += movement * Time.deltaTime;
            anim.StartWalking();
            /*  if (movement != Vector3.zero)
              {
                  transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 70 * Time.deltaTime);
              }*/
        }else {
            anim.StopWalking();
        }
    }
}
