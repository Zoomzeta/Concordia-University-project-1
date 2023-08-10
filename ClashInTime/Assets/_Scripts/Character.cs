using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All entities could use this class (players/allies/enemies)
public class Character: MonoBehaviour
{
    // VALUES
  
    [SerializeField]
    private string characterName;
 
    private int level;

    

    public int currentHealth;
    private int maxHealth;

    private int strength; // effects physical damage + health
    private int intelligence; // effects magic damage
    private int agility; // effects speed(turn order)

    private float walkingSpeed;

    //attack values based on STR or INT
    private int attackValue;
    private int magicAttackValue;

    private int enemyAgression; // lower value mean less likely to be targeted

    //Personalities Range from 1-3
    //Note: Should change them only through inc and dec functions
    private int anger;
    private int bravery;

    void Start()
    {
        //build class base on their class name
        BuildCharacter(gameObject.name);
    }

    public enum CharacterClass
    {
        Warrior,
        Archer,
        Mage
    }
    
    [SerializeField]
    private CharacterClass charClass;



    // CONTRUCTOR
    public void BuildCharacter(string name)
    { 
        this.characterName = name;
        this.level = 1;
        int cStrength=strength;
        int cIntelligence=intelligence;
        int cAgility = 1;
        
        int cAnger;
        int cBravery;
        
        //Warrior
        if(name == "Warrior" || name == "Knight")
        {
            cStrength = 10;
            cIntelligence = 5;
            cAgility = 5;

            cAnger = 2;
            cBravery = 3;
            
        }
        //Archer
        else if (name == "Archer")
        {
            cStrength = 5;
            cIntelligence = 5;
            cAgility = 10;

            cAnger = 1;
            cBravery = 2;
            
        }
        //Mage
        else if (name == "Mage")
        {
            cStrength = 5;
            cIntelligence = 10;
            cAgility = 5;

            cAnger = 0;
            cBravery = 1;
           
        }
        //Warrior again
        else {
            cStrength = 10;
            cIntelligence = 5;
            cAgility = 5;

            cAnger = 2;
            cBravery = 3;
            
        }

        strength = cStrength;
        intelligence = cIntelligence;
        agility = cAgility;

        maxHealth = cStrength * 10;
        currentHealth = cStrength * 10;

        attackValue = cStrength;
        MagicAttackValue = cIntelligence;

        anger = cAnger;
        bravery = cBravery;

        
    }

    /// <summary>
    ///     Returns attack value (int)
    /// </summary>
    public int attack()
    {
        return attackValue;
    }

    public int magicAttack()
    {
        return magicAttackValue;
    }

    /// <summary>
    ///     reduces current health based on value (dmg)
    ///     Sets health to 0 if damage were to set it below 0
    /// </summary>
    public void receiveDamage(int dmg)
    { 
        if((currentHealth - dmg) < 0)
            currentHealth = 0;
        else
            currentHealth -= dmg;
    }

    /// <summary>
    ///     increases current health based on value 
    ///     Sets health to maxHealth if value were to set it above maxHealth
    /// </summary>
    public void heal(int value)
    {
        if ((currentHealth + value) > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += value;
    }

    // functions to change personality levels

    public void incBravery()
    {
        if (bravery == 3)
        {
            //at max bravery, do nothing
            return;
        }
        else
        { 
            bravery += 1;
        }
            
    }

    public void decBravery()
    {
        if (bravery == 0)
        {
            //at min bravery, do nothing
            return;
        }
        else
        {
            bravery -= 1;
        }
    }

    public void incAnger()
    {
        if (anger == 3)
        {
            //at max anger, do nothing
            return;
        }
        else
        {
            anger += 1;
        }
    }

    public void decAnger()
    {
        if (anger == 0)
        {
            //at min anger, do nothing
            return;
        }
        else
        {
            anger -= 1;
        }
    }

    /// Getter and Setters
    public string PlayerName
    {
        get { return characterName; }
        set { characterName = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public string Class
    {
        get { return charClass.ToString(); }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int Health
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    public int Intelligence
    {
        get { return intelligence; }
        set { intelligence = value; }
    }

    public int Agility
    {
        get { return agility; }
        set { agility = value; }
    }

    public float WalkingSpeed
    {
        get { return walkingSpeed; }
        set { walkingSpeed = value; }
    }

    public int AttackValue
    {
        get { return attackValue; }
        set { attackValue = value; }
    }

    private int MagicAttackValue
    {
        get { return magicAttackValue; }
        set { magicAttackValue = value; }
    }

    private int EnemyAgressionValue
    { 
        get { return enemyAgression; }
        set { enemyAgression = value;}
    }

    public int Bravery
    {
        get { return bravery; }
    }

    public int Anger
    {
        get { return anger; }
    }

}