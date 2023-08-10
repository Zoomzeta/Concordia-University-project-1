using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{

    [SerializeField]
    float mMouseSensitivity = 0;

    Player player;
    public Material emptyMat;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.GetComponent<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        // alternative
        //transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Vector3.up);

        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");

        if (!Mathf.Approximately(rotationX, 0.0f))
        {
            Quaternion quaternionX = Quaternion.AngleAxis(rotationX * mMouseSensitivity, Vector3.up);
            transform.localRotation = transform.localRotation * quaternionX;
        }

        if (!Mathf.Approximately(rotationY, 0.0f))
        {
            Quaternion quaternionY = Quaternion.AngleAxis(rotationY * mMouseSensitivity, -Vector3.right);
            transform.localRotation = transform.localRotation * quaternionY;
        }
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(rayOrigin, transform.forward * 2, Color.green);
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, transform.forward, out hit, 2.0f))
            {
                Object obj = hit.collider.GetComponent<Object>();
                if (obj != null)
                {
                    obj.execute();
                }
            }
        }
        if (Input.GetButtonDown("Fire2"))   // swap object
        {
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, transform.forward, out hit, 2.0f))
            {
                Object obj = hit.collider.GetComponent<Object>();
                if (obj != null)
                {
                    Transform savedObj = player.saveStates[player.selectedCamera].GetComponent<SaveState>().saveState.transform.GetChild(0);
                    if (savedObj.gameObject.CompareTag("EmptyObject") && obj.CompareTag("MissingObject"))
                    {

                    }
                    else
                    {
                        if (obj.objType == Object.Type.Square && player.selectedCamera == 2)
                        {
                            SwapObj(obj);
                            if (obj.gameObject.CompareTag("MissingObject"))
                            {
                                obj.gameObject.GetComponent<MeshRenderer>().material = emptyMat;
                                obj.gameObject.tag = "EmptyObject";
                            }
                        }
                        else if (obj.objType == Object.Type.Circle && player.selectedCamera == 3)
                        {
                            SwapObj(obj);
                            if (obj.gameObject.CompareTag("MissingObject"))
                            {
                                obj.gameObject.GetComponent<MeshRenderer>().material = emptyMat;
                                obj.gameObject.tag = "EmptyObject";
                            }
                        }
                        else
                            print("Wrong camera selected");
                    }                                   
                }
            }
        }
    }

    private void SwapObj(Object obj)
    {
        GameObject temp = new GameObject();
        temp.transform.parent = obj.transform.parent;
        temp.transform.position = obj.transform.position;
        temp.transform.rotation = obj.transform.rotation;
        temp.transform.localScale = obj.transform.localScale;
        Transform savedObj = player.saveStates[player.selectedCamera].GetComponent<SaveState>().saveState.transform.GetChild(0);
        obj.transform.parent = player.saveStates[player.selectedCamera].GetComponent<SaveState>().saveState.transform;
        obj.transform.position = savedObj.position;
        obj.transform.rotation = savedObj.rotation;
        obj.transform.localScale = savedObj.localScale;

        savedObj.parent = temp.transform.parent;
        savedObj.position = temp.transform.position;
        savedObj.rotation = temp.transform.rotation;
        savedObj.localScale = temp.transform.localScale;

        Destroy(temp);
    }
}
