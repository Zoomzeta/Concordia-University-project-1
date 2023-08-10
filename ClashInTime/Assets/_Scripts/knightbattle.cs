using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knightbattle : MonoBehaviour
{
    public Animator anim;
    public GameObject vfx;

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */

    public void attack()
    {
        anim.Play("Attack");
        vfx.SetActive(true);
    }
}
