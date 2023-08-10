using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnFloor : MonoBehaviour
{
    public float yValue;
    // Start is called before the first frame update
    void Start()
    {
        yValue = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y != yValue)
        {
            transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
        }
    }
}
