using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Stats health;
    public static int enemyScore = 0;
    public static bool enemyHit = false;
    public AudioSource soundOfDeath;
    private bool hasDied = false;
    private object collision;
   
    float elapsed = 0f;
    
    public void Awake()
    {
        health.InitializeMaxValue();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //


    void FixedUpdate()
    {
 
        elapsed += Time.deltaTime;
        if (elapsed >= 1f )
        {
            elapsed = elapsed % 6f;
            if(enemyHit == true)
            {
                UpdatePlayerHealth();
                if (health.CurrentValue == 0 && !hasDied)
                {
                    hasDied = true;
                    soundOfDeath.Play();
                }
            }
            
        }
        
         
    }

    


    void UpdatePlayerHealth()
    {
        health.CurrentValue -= 1f;
        if (health.CurrentValue <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    /*
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "enemy")
        {
            Debug.Log("Hit");
        }
    }
    */
    
}
