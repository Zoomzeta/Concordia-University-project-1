using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleposition : MonoBehaviour
{
    public Transform curarcherpos; //get the current position of the archer
    public Transform curcleopos; //get the current position of the cleopatra
    public Transform curoldpos; //get the current position of the scientist

    public Transform archerpos; //set the current position of the archer
    public Transform cleopos; //set the current position of cleopatra
    public Transform oldpos; //set the current position of the scientist

    public Transform curtriggerenemy; //get the current position of the enemy triggered
    public Transform triggerenemy; //set the current position of the enemy triggered

    [HideInInspector] public Transform curtEnemyTwo;
    public Transform enemyTwoBattle;
    
    [HideInInspector] public Transform curtEnemyThree;
    public Transform enemyThreeBattle;
    
    [HideInInspector] public Transform curtEnemyFour;
    public Transform enemyFourBattle;

    [HideInInspector] public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        //find the archer postition at start
        curarcherpos = GameObject.Find("Archer").transform;
        enemyCount = 0;
    }

    //set the team position during combat
    public void setposition()
    {
        curarcherpos.position = archerpos.position; //get the current position of the archer and place it in the correct position
        int i = 1;

        while(i <= enemyCount) {

            //get the current position of the triggered enemy and place it in the correct position
            if (i == 1)
            {
                curtriggerenemy.position = triggerenemy.position;
            }else if(i == 2)
            {
                curtEnemyTwo.position = enemyTwoBattle.position;
            }else if (i == 3)
            {
                curtEnemyThree.position = enemyThreeBattle.position;
            }else if (i == 4)
            {
                curtEnemyFour.position = enemyFourBattle.position;
            }
            i++;
        }

        //if cleopatra is not in the scene let her position be null
        if (cleopos == null)
        {
            curcleopos = null;
        }
        else
        {
            //if cleopatra is in the scene, set the battle position
            curcleopos.position = cleopos.position;
        }
        //if the scientist is not in the scene let her position be null
        if (curoldpos == null)
        {
            curoldpos = null;
        }
        else
        {
            //if the scientist is in the scene, set the battle position
            curoldpos.position = oldpos.position;
        }
    }
}
