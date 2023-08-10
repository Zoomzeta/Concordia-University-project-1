using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    // Start is called before the first frame update
    BasicAnimationController test;
    void Start()
    {
        test = this.GetComponent<BasicAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown("w"))
        {
            test.StartWalking();
        }

        if (Input.GetKeyDown("s"))
        {
            test.StopWalking();
        }

        if (Input.GetKeyDown("a"))
        {
            test.Attack();
        }

        if (Input.GetKeyDown("d"))
        {
            test.Die();
        }

        if (Input.GetKeyDown("h"))
        {
            test.Heal();
        }

        if (Input.GetKeyDown("q"))
        {
            test.StartRunning();
        }

        if (Input.GetKeyDown("e"))
        {
            test.StopRunning();
        }
    }
}
