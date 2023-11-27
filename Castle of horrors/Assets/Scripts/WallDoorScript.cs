using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDoorScript : MonoBehaviour
{
    public GameObject destinationDoor;
    Vector3 TPCoords;
    public bool locked = false;
    //SpecialDoors can only be opened in special blocks
    public bool specialDoor = false;
    public bool finalDoor = false;
    Transform god;
    public static bool end = false;

    private void Start()
    {
        god = transform.parent.parent.parent;
    }

    //Takes coordinates of the destination door
    public Vector3 getC()
    {
        if (specialDoor && transform.parent.GetComponent<Block>().isSpecial())
        {
            if (locked)
            {
                god.GetComponent<EmptyRoomObjectScript>().setText("The door is in the right place but it is still locked...");
                TPCoords = getCoord();
                end = false;
            }
            else
            {
                TPCoords = destinationDoor.GetComponent<WallDoorScript>().getCoord();
                if (finalDoor)
                    end = true;
            }
        }
        else
        {
            if (specialDoor)
            {
                god.GetComponent<EmptyRoomObjectScript>().setText("This door looks special, maybe I should put it somewhere...");
                TPCoords = getCoord();
            }
            else
            {
                if (locked)
                {
                    TPCoords = getCoord();
                }
                else
                {
                    TPCoords = destinationDoor.GetComponent<WallDoorScript>().getCoord();
                }
            }
            end = false;
        }   
        return TPCoords;
    }
    //returns true (acts as a check)
    public bool isDoor()
    {
        return true;
    }
    //Gives coordinates of this door
    public Vector3 getCoord()
    {
        return gameObject.GetComponentInParent<Block>().decidePlayerPosition();
    }

    public Transform getDestinationRoom()
    {
        return destinationDoor.transform.parent.parent;
    }
}
