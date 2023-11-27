using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour

{

    public Transform player;
    //public float speed;
    public AudioSource monsterWalkSound;
    //This is the script for the skeleton Monster
    // Start is called before the first frame update

    private bool walkAlreadyPlaying = false;
    private bool idleOrAttacking = false;

    [SerializeField]
    private float monsterSpeed1;

    [SerializeField]
    private float monsterSpeed2;

    private Animator anim;
    Vector3 previousPos;

    private bool failSafeVar = true;
    GameObject Character;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        previousPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        PlayerHealth.enemyHit = false;
        previousPos = transform.position;
        //if (Vector3.Distance(player.position, this.transform.position) < 10 && angle < 25)

        if (Vector3.Distance(player.position, this.transform.position) < 10)
        {

            PlayerHealth.enemyHit = false;

            //float angle = Vector3.Angle(direction, this.transform.forward);
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            Debug.Log("Setting isIdle to false");
            anim.SetBool("isIdle", false);
            Debug.Log("Setting isWalking to true");
            anim.SetBool("isWalking", true);
            if (!walkAlreadyPlaying && !idleOrAttacking)
            {
                walkAlreadyPlaying = true;
                monsterWalkSound.Play(); // The monster has started to follow the player, so the walking sound is audible.
            }
            //  anim.SetTrigger("isWalking");
            if (direction.magnitude > 8f)
            {
                this.transform.Translate(0, 0, monsterSpeed1);


            }

            else
            {
                this.transform.Translate(0, 0, monsterSpeed2);

                int numberOfHits = 0;

                if (direction.magnitude < 3f)

                {
                    Debug.Log("Player hit!!!!!!!!!!!!!!" + numberOfHits++);

                    anim.SetBool("isWalking", false);
                    Debug.Log("Setting isAttacking to true");
                    anim.SetBool("isAttacking", true);
                    idleOrAttacking = true; // Causes the walking sound to stop when the monster is attacking the player.
                    walkAlreadyPlaying = false;

                    if (direction.magnitude < 1.5f)
                    {
                        PlayerHealth.enemyHit = true;
                    }

                    else
                    {
                        PlayerHealth.enemyHit = false;
                    }

                }
                else
                {
                    idleOrAttacking = false;
                }

            }


        }

        else
        {
            anim.SetBool("isIdle", true);
            idleOrAttacking = true; // Ensure the walking sound is not played when the monster is idle.
            walkAlreadyPlaying = false;
        }



    }


    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        { 
            Debug.Log("Player hit ............");

        }
    }
    */

}