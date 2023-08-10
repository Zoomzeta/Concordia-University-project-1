using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerbattle : MonoBehaviour
{
    public GameObject camoverview; //camera on the overview
    public GameObject cambattle; //battle camera
    public knightmovement km; //the knight movement script
    public BasicAnimationController bac; //animation script
    private BasicAnimationController enemyBac;
    public Transform enemy; //the position of the enemy
    public GameObject battleinfo; //gui gameobject for the knight
    public GameController gc; //get access to the gamecontroller
    private GameObject triggergameobject; //enemy triggered with
    public battleposition bp; //the team formation
    public GameObject battleseparator; //battle separator in the middle of the screen
    public CampWander cw;
    public Arrive arrive;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy").transform; //find the enemy to be triggered with
        //camoverview = GameObject.FindWithTag("overview");
        //camoverview = GameObject.FindWithTag("overview");
        //km = (knightmovement)GameObject.FindWithTag("Knight").GetComponent("knightmovement");
        bp = this.gameObject.GetComponent<battleposition>(); //find the battle position script
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>(); //find the gamecontroller
        km.direction *= 0; //prevent the knight from walking once in battle
    }

    //starting the battle and set up the formation
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") //check if the collider is with the enemy
        {
            bp.enemyCount = 0;
            triggergameobject = other.gameObject; //set the enemy gameobject

            Collider[] enemies = triggergameobject.transform.parent.GetComponentsInChildren<Collider>();

            foreach (var enemy in enemies)
            {
                if (enemy.tag == "Enemy")
                {
                    bp.enemyCount++;
                    ////set the enemy positions once entered in battle
                    if (bp.enemyCount == 1)
                    {
                        bp.curtriggerenemy = enemy.transform; 
                    }else if(bp.enemyCount == 2)
                    {
                        bp.curtEnemyTwo = enemy.transform;
                    }else if(bp.enemyCount == 3)
                    {
                        bp.curtEnemyThree = enemy.transform;
                    }else if(bp.enemyCount == 4)
                    {
                        bp.curtEnemyFour = enemy.transform;
                    }

                    //Disable enemy wander script
                    if (other.gameObject.layer != 6)
                    {
                        cw = enemy.GetComponent<CampWander>();
                        cw.enabled = false;
                        cw.wanderSpeed = 0;

                        arrive = enemy.GetComponent<Arrive>();
                        arrive.enabled = false;
                        arrive.seekSpeed = 0;
                    }

                    enemyBac = enemy.GetComponentInParent<BasicAnimationController>();
                    enemyBac.StopWalking();
                    enemyBac.StopRunning();

                    enemy.transform.LookAt(this.transform);
                }
            }

            //Stop playable character movement
            km.direction *= 0;

            //testing to fix wander with battle collider 
            //triggergameobject.transform.LookAt(this.gameObject.transform, Vector3.up);

            bp.setposition(); //set the team position

            //rotate to look at the enemy regadless of position
            transform.LookAt(other.gameObject.transform);

            camoverview.SetActive(false); //turn off the map camera
            //battleseparator.SetActive(true); //turn on the battle separator
            cambattle.SetActive(true); //turn on the battle camera
            bac.StopWalking(); //stop the team from walking
            bac.StopRunning(); //stop the team from running
            km.enabled = false; //stop the knight from walking during combat
            battleinfo.SetActive(true); //show the battle gui

            if (gc.GetBattleController() == null)
                gc.StartBattle(triggergameobject); //start the ai battle
        }
    }

    //once the battle is over, turn back to overview mode
    public void Onbattlewon()
    {
        cw.enabled = true; //allow the ai to wander again
        camoverview.SetActive(true); //turn on the map camera
        battleseparator.SetActive(false); //turn off the battle separator
        cambattle.SetActive(false); //turn off the battle camera  
        km.enabled = true; //allow the knight to walk after combat
        enemy = GameObject.FindWithTag("Enemy").transform; //find the enemy gameobject
        battleinfo.SetActive(false); //turn off the battle gui
    }
}
