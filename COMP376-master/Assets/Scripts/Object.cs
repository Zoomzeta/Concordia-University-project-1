using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public enum Type { Normal, Square, Circle };
    public Type objType;
    public GameObject lockedDoor;
    public GameObject player;
    public GameObject designatedWall;
    Transform god;

    void Start()
    {
        god = getRoomsEmptyObject();
    }
    //Called on left click on the object
    public void execute()
    {
        if (objType == Object.Type.Normal)
        {
            god.GetComponent<EmptyRoomObjectScript>().setText("I already got the key");
            gameObject.tag = "Untagged";
        }
        if (gameObject.CompareTag("Key"))
        {
            //If it has to be on a specific wall, check for it
            if (designatedWall != null)
            {
                //Check if the block of this object equals the block of the designated wall
                if (getBlock().Equals(designatedWall.GetComponent<Transform>().parent.GetComponent<Transform>()))
                {
                    god.GetComponent<EmptyRoomObjectScript>().setText("You found a key!");
                    player.GetComponent<PlayerController>().addKey(lockedDoor);
                    objType = Object.Type.Normal;
                    gameObject.GetComponent<MeshRenderer>().material = god.GetComponent<EmptyRoomObjectScript>().GetMaterial();
                }
                else
                {
                    god.GetComponent<EmptyRoomObjectScript>().setText("Not on the correct wall");
                }
            }
            else
            {
                god.GetComponent<EmptyRoomObjectScript>().setText("It's a key!");
                player.GetComponent<PlayerController>().addKey(lockedDoor);
                objType = Object.Type.Normal;
                gameObject.GetComponent<MeshRenderer>().material = god.GetComponent<EmptyRoomObjectScript>().GetMaterial();
            }

        }
        else
        {
            if (gameObject.CompareTag("MissingObject"))
            {
                god.GetComponent<EmptyRoomObjectScript>().setText("I could put an object in here...");
            }
            
        }        
    }

    public Transform getBlock()
    {
        return transform.parent.parent;
    }
    //Returns the empty object rooms
    public Transform getRoomsEmptyObject()
    {
        return transform.parent.parent.parent.parent;
    }

}
