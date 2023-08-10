using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamp : MonoBehaviour
{
    [HideInInspector] public Vector3 startPos;
    public float campRadius;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DebugUtil.DrawWireSphere(startPos, Color.red, campRadius);
    }
}
