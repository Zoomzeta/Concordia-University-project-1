using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    public int health;
    public int healthMax;
    public Slider healthBar;
    private Animator anim;

    [SerializeField]
    private Text valueText;

    public float Value
    {
        set
        {
            string[] tmp = valueText.text.Split(':');
            valueText.text = tmp[0] + ": " + value;
            //fillAmount = Map(value, 0, healthMax, 0, 1);
        }
    }

    private void Start()
    {
        
        health = healthMax;
        healthBar.value = health;
        healthBar.maxValue = healthMax;

        anim = GetComponent<Animator>();
    }
   

   void Update()
    {

        float hit = anim.GetFloat("hit");

        if(hit > 0)
        {
            hit -= Time.deltaTime * 3; //To give animator time to go back to 0
            anim.SetFloat("hit", hit);
        }

        //Set condition for player death
        if (health < 1)
        {
            anim.SetBool("death", true);
        }


        //Generate random numbers to test health system
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Damage(Random.Range(10, 20));
        }
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.value = health;
        anim.SetFloat("hit", 1);
    }
}
