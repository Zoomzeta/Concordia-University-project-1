//https://www.youtube.com/watch?v=_QajrabyTJc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensibility = 100f;
    //public Transform playerBody;

    // Update is called once per frame
    void Update()
    {
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;

        //playerBody.Rotate(Vector3.up * mouseX);
        print(Input.GetAxis("Mouse X"));
        if (Input.GetAxis("Mouse X") < 0)
        {
            //Code for action on mouse moving left
            print("Mouse moved left");
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            //Code for action on mouse moving right
            print("Mouse moved right");
        }
    }
}
