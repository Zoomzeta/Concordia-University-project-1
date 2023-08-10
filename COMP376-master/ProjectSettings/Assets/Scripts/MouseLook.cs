//https://www.youtube.com/watch?v=_QajrabyTJc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensibility = 100f;
    public Transform playerBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);
        Debug.Log(Input.GetAxis("Mouse X"));
        // Get Axis returns 0????
    }
}
