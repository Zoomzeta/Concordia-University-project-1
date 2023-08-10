using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knightmovement : MonoBehaviour
{
    private float moveVert; //the vertical movement of the knight
    private float moveHorz; //the horizontal movement of the knight

    public Animator animator; //the animator component of the knight

    public CharacterController controller; //the character controller of the knight
    public Transform cam; //the camera position

    public float speed = 5.0f; //how fast the knight move
    private float turnSpeed = 0.8f; //how fast the knight turn
    private float turnVelocity; //amount of change in turn

    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //get the animator component of the knight
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //allow the knight to move on player input w,a,s,d
        moveHorz = Input.GetAxis("Horizontal");
        moveVert = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorz, 0, moveVert).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("walk", true);
            //if (animator.GetBool("scared"))
            //{
            //    speed = 5.0f;
            //}

            //allow the knight to run instead of walk once press on q
            if (Input.GetKey("q"))
            {
                animator.SetBool("run", true);
                animator.SetBool("walk", false);
                speed = 10.0f;
            }
            else
            {
                //else prevent the knight from running
                animator.SetBool("run", true);
                animator.SetBool("run", false);
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            //else prevent the knight from walking
            animator.SetBool("walk", false);
        }
    }
}
