using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public List<GameObject> walls;    // Element 0 : UP
                                      // Element 1: Right
                                      // Element 2: Down
                                      // Element 3: Left
    public double priority = 0;
    public List<GameObject> collisionList;

    public Transform god;
    public bool special = false;
    void Awake()
    {
        collisionList = new List<GameObject>();
        god = transform.parent.parent;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            if (!collisionList.Contains(col.gameObject))
                collisionList.Add(col.gameObject);
            if (priority < col.gameObject.GetComponent<Block>().priority)
            {
                //gameObject.SetActive(false);
                Stack<Transform> children = new Stack<Transform>();
                children.Push(this.transform);
                while (children.Count > 0)
                {
                    Transform current = children.Pop();
                    Renderer renderer = current.GetComponent<Renderer>();
                    Collider collider = current.GetComponent<Collider>();
                    if (renderer != null)
                    {
                        renderer.enabled = false;
                    }
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                    foreach (Transform child in current.transform)
                    {
                        children.Push(child);
                    }
                }
            }
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            // keep track of location
            // characters: player and monsters
            Player player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.currentBlock = gameObject;
                player.currentRoom = gameObject.transform.parent.gameObject;

                // check if player can switch in this room
                player.mSwitch = player.currentRoom.GetComponent<Room>().mSwitch;

                // block where player is always render
                priority = 20;


                if (special)
                {
                    god.GetComponent<EmptyRoomObjectScript>().setText("This looks like a special place...");
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        //if (col.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        //{
        //    //collisionList.Remove(col.gameObject);
        //    //if (priority < col.gameObject.GetComponent<Block>().priority)
        //    //    gameObject.SetActive(true);

        //}
        if (col.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            // keep track of location
            // characters: player and monsters
            Player player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
            }
        }
    }

    public Vector3 getDoorDestination()
    {
        return gameObject.GetComponentInChildren<WallDoorScript>().getC();
    }

    //Only called when player is in the block
    public bool hasDoor(GameObject player)
    {
        bool has = false;
        foreach (Transform child in transform)
        {
            if (child.tag == "Door")
            {
                has = true;
                if (child.GetComponent<WallDoorScript>().locked)
                {
                    god.GetComponent<EmptyRoomObjectScript>().setText("Door Locked");
                    if (player.GetComponent<PlayerController>().checkKeys(child.gameObject))
                    {
                        god.GetComponent<EmptyRoomObjectScript>().setText("Door Unlocked");
                        child.GetComponent<WallDoorScript>().locked = false;
                    }
                }
                player.GetComponent<PlayerController>().setDestinationRoom(child.GetComponent<WallDoorScript>().getDestinationRoom());
            }

        }
        return has;

    }

    public Vector3 decidePlayerPosition()
    {
        //Parent (room) global position + this block local position
        return transform.parent.GetComponent<Transform>().position + gameObject.GetComponent<Transform>().localPosition;
    }

    // fade everything in block



    // Checks the status of the door (locked or not)
    public bool checkDoorLocked()
    {
        return gameObject.GetComponentInChildren<WallDoorScript>().locked;
    }

    //Check if the block is special (Doors here can react differently)
    public bool isSpecial()
    {
        return special;
    }
}
