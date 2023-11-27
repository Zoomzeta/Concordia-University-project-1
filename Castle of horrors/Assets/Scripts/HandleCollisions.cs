using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCollisions : MonoBehaviour
{
    [SerializeField]
    private float distanceToGround;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private float enemySpeed;
    [SerializeField]
    private bool stopRight;
    [SerializeField]
    private bool stopLeft;
    [SerializeField]
    private bool stopUp;
    [SerializeField]
    private bool stopDown;
    [SerializeField]
    private bool stopForward;
    [SerializeField]
    private bool stopBackward;


    //Use Raycast to detect collisions

    public void FixedUpdate()
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, distanceToGround+0.1f))
        {
            isGrounded = false;
            //Debug.Log("Is not grounded");
        }
        else
        {
            isGrounded = true;
            //Debug.Log("IEnemy is grounded");
        }
         
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
