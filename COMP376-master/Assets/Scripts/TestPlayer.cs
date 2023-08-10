using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestPlayer : MonoBehaviour
{
    [SerializeField]
    private Stats health;

    public void Awake()
    {
        health.InitializeMaxValue();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Update health here
        if (Input.GetKeyDown(KeyCode.Q)  )
        {
            health.CurrentValue -= 10;
        }

        if (Input.GetKeyDown(KeyCode.I)  )
        {
            health.CurrentValue += 10;
        }
    }

   
}
