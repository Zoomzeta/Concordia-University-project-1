using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject currentRoom;    // Current room location
    public GameObject currentBlock;   // Current block location
    public int currentFace;           // Current face rotation: 0 = up, 1 = right, 2 = down, 3 = left

    public int selectedCamera;         // Current state selected
    public List<SaveState> saveStates; // List of the four cameras to be switched

    void Start()
    {
        // Start at specified location to spawn
        transform.position = currentBlock.transform.position;
        // Determine which direction to rotate towards
        Vector3 desiredPostion = currentBlock.GetComponent<Block>().walls[currentFace].transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(desiredPostion, Vector3.up);

        // TODO: create in script the save states 

        //// Re-position cams
        //Vector3 desiredPostion2;
        //for (int i = 1; i <= 4; i++)
        //{
        //    saveStates[i - 1].transform.position = GameObject.Find("Extra (" + i + ")").transform.position;
        //    desiredPostion2 = saveStates[i - 1].saveState.transform.position - saveStates[i - 1].transform.position;
        //    transform.rotation = Quaternion.LookRotation(desiredPostion, Vector3.up);
        //}
    }

    void Update()
    {

    }
}
