using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /*
     * TODO: Create a SaveStateManager that stores the save states
     *       Use Observer pattern to manage user inputs
     *       
     *       Create a Character class which is a parent for both monsters and player and keeps track of location
     *       Create a Monster class for every different monsters
     *       
     *       Create a Map class that stores the rooms and links the doors together. Have a 3x3 grid for the location.
     *       
     *       
     *       
     */
    public GameObject currentRoom;    // Current room location
    public GameObject currentBlock;   // Current block location
    public int currentFace;           // Current face rotation: 0 = up, 1 = right, 2 = down, 3 = left

    public int selectedCamera;         // Current state selected
    public List<SaveState> saveStates; // List of the four cameras to be switched
    public bool mSwitch;               // Allows player to save and switch if true
    void Awake()
    {
        // Start at specified location to spawn
        transform.position = currentBlock.transform.position;
        // Determine which direction to rotate towards
        Vector3 desiredPostion = currentBlock.GetComponent<Block>().walls[currentFace].transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(desiredPostion, Vector3.up);
        // TODO: create in script the save states 
    }

    void Update()
    {

    }
}
