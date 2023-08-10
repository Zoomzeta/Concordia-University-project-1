using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimationController : MonoBehaviour
{
    private Animator controller;
    public GameObject weapontrail; //vfx of each of the character during combat
    //public GameController.BattleController gcb;
    public GameObject healall; //the heal magic for cleaopatra and summon for the boss fight

    void Start()
    {
        controller = GetComponent<Animator>(); //get the animator
        //gcb = GameObject.FindWithTag("GameController").GetComponent<GameController.BattleController>();
    }
    
    //let the characters walk
    public void StartWalking()
    {
        controller.SetBool("walk", true);
    }
    //prevent the characters from walking
    public void StopWalking()
    {
        controller.SetBool("walk", false);
    }

    //called by all the characters to attack
    public void Attack()
    {
        weapontrail.SetActive(true); //play the weapon trail when the character attack
        controller.SetBool("attack", true); //allow the attack animation to play
        controller.Play("Attack", 0, 0f); //play the attack animation
        StartCoroutine("OnCompleteAttack"); //call the attack coroutine
    }

    //finish the attack 
    IEnumerator OnCompleteAttack()
    {
        while (controller.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        controller.SetBool("attack", false); //stop the attack animation
        weapontrail.SetActive(false); //stop the weapon trail when the character isnt attacking
        //gcb.TurnDone(); //tell the game controller that the turn is over
    }

    //play dying animation when the character die
    public void Die()
    {
        controller.SetBool("isDead", true);
    }

    //heal the targeted team member
    public void Heal()
    {
        controller.SetBool("heal", true); //allow the heal animation to play
        controller.Play("Heal", 0, 0f); //play the heal animation
        healall.SetActive(true); //turn on the vfx heal
        StartCoroutine("OnCompleteHeal"); //call the heal coroutine
    }

    //finish the heal
    IEnumerator OnCompleteHeal()
    {
        while (controller.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        controller.SetBool("heal", false); //prevent the heal animation
        healall.SetActive(false); //turn off the vfx heal
    }

    //let the character run
    public void StartRunning()
    {
        controller.SetBool("run", true);
    }
    //prevent the characters from running
    public void StopRunning()
    {
        controller.SetBool("run", false);
    }

    //summon the enemy once the previous undead are dead
    public void SummonEnemy()
    {
        controller.SetBool("summon", true);
        controller.Play("Summon", 0, 0f); //play the summon animation
        healall.SetActive(true); //turn on the vfx summon
        StartCoroutine("OnSummonEnemy"); //call the summon coroutine
    }

    //finish the summon
    IEnumerator OnSummonEnemy()
    {
        while (controller.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
        healall.SetActive(false); //turn off the vfx summon
        controller.SetBool("summon", false);
    }

}
