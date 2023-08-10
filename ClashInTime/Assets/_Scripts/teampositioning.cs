using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teampositioning : MonoBehaviour
{
    public Transform knight;
    public Transform formation;
    public GameObject enemy;
    public int controlformation;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
        knight = GameObject.FindWithTag("Knight").transform;
        formation = GameObject.FindWithTag("Formation").transform;
        controlformation = 0;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knight")
        {
            if(controlformation == 0)
            {
                controlformation++;
            }
        }
    }
}
