using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float mPace = 2;

    // TODO: Input Buffer

    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void LateUpdate()
    {
        Movement();
        // TODO: interact with objects on walls
        //       save and swap
        //       select which saved state to be selected as the one to swap

    }

    void Movement()
    {
        // TODO: Have the user input on press method, and update is used for lerp only

        // User movement
        if (Input.GetButtonDown("Right"))
        {
            transform.Rotate(0, 90, 0);
            player.currentFace = (player.currentFace + 1) % 4;
        }
        if (Input.GetButtonDown("Left"))
        {
            transform.Rotate(0, -90, 0);
            player.currentFace = (4 + player.currentFace - 1) % 4;
        }
        if (Input.GetButtonDown("Shift Right"))
        {
            Vector3 desiredPosition = transform.position + transform.right * mPace;
            transform.position = desiredPosition;
        }
        if (Input.GetButtonDown("Shift Left"))
        {
            Vector3 desiredPosition = transform.position - transform.right * mPace;
            transform.position = desiredPosition;
        }
        if (Input.GetButtonDown("Forward"))
        {
            Vector3 desiredPosition = transform.position + transform.forward * mPace;
            transform.position = desiredPosition;
        }
        if (Input.GetButtonDown("Backward"))
        {
            Vector3 desiredPosition = transform.position - transform.forward * mPace;
            transform.position = desiredPosition;
        }

        // Switch selected camera
        if (Input.GetButtonDown("Switch1"))
        {
            player.selectedCamera = 0;
        }
        if (Input.GetButtonDown("Switch2"))
        {
            player.selectedCamera = 1;
        }
        if (Input.GetButtonDown("Switch3"))
        {
            player.selectedCamera = 2;
        }
        if (Input.GetButtonDown("Switch4"))
        {
            player.selectedCamera = 3;
        }

        // swap
        if (Input.GetButtonDown("Swap"))
        {
            Swap();
        }
    }

    void Swap()
    {
        // Target (where the player is)
        GameObject target = new GameObject("container");
        target.transform.position = player.currentBlock.transform.position;
        target.transform.rotation = transform.rotation;
        // What is in front of player
        GameObject tempObj = player.currentBlock.GetComponent<Block>().walls[player.currentFace];
        tempObj.transform.parent = target.transform;

        // Destination (Where save state is)
        GameObject dest = new GameObject("disposable");
        dest.transform.position = player.saveStates[player.selectedCamera].transform.position;
        dest.transform.rotation = player.saveStates[player.selectedCamera].transform.rotation;
        // Save State (Container in save state)
        GameObject newTransform = player.saveStates[player.selectedCamera].GetComponent<SaveState>().saveState;
        newTransform.transform.parent = dest.transform;
        int tempOrigFace = player.saveStates[player.selectedCamera].GetComponent<SaveState>().origFace;
        if (tempObj.layer == LayerMask.NameToLayer("Blocks"))
        {
            // iterate and store rest of blocks
            List<GameObject> listBlocks = new List<GameObject>();
            LoopBlock(player.currentFace, player.currentBlock.GetComponent<Block>().walls[player.currentFace], ref listBlocks);

            // Old gameObject
            foreach (GameObject item in listBlocks)
            {
                item.transform.parent = target.transform;
            }
            player.saveStates[player.selectedCamera].GetComponent<SaveState>().origFace = player.currentFace;
        }

        // Swap
        Vector3 tempFacePos = dest.transform.position;
        Quaternion tempFaceQuat = dest.transform.rotation;
        Vector3 tempFaceScale = dest.transform.localScale;

        dest.transform.position = target.transform.position;
        dest.transform.rotation = target.transform.rotation;
        dest.transform.localScale = target.transform.localScale;

        target.transform.position = tempFacePos;
        target.transform.rotation = tempFaceQuat;
        target.transform.localScale = tempFaceScale;

        // Save
        if (newTransform.transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            List<GameObject> blocks = new List<GameObject>();
            for (int i = 0; i < newTransform.transform.childCount; i++)
            {
                blocks.Add(newTransform.transform.GetChild(i).gameObject);
            }

            player.currentBlock.GetComponent<Block>().walls[player.currentFace] = blocks[0];
            foreach (GameObject item in blocks)
            {
                item.transform.parent = player.currentRoom.transform;
                // update walls by rotating them
                int rotate;
                if (player.saveStates[player.selectedCamera].GetComponent<SaveState>().origFace == -1) // if it was a wall before
                    rotate = 0;
                else
                    rotate = tempOrigFace - player.currentFace;
                GameObject[] tempArr = new GameObject[4];
                for (int i = 0; i < 4; i++)
                {
                    tempArr[i] = item.GetComponent<Block>().walls[i];
                }
                for (int i = 0; i < 4; i++)
                {
                    item.GetComponent<Block>().walls[i] = tempArr[(4 + i + rotate) % 4];
                }
                blocks[0].GetComponent<Block>().walls[(player.currentFace + 2) % 4] = player.currentBlock;
            }
        }
        else
        {
            player.currentBlock.GetComponent<Block>().walls[player.currentFace] = newTransform.transform.GetChild(0).gameObject;
            newTransform.transform.GetChild(0).parent = player.currentBlock.transform;
        }
        player.saveStates[player.selectedCamera].GetComponent<SaveState>().saveState = target;
        target.transform.parent = GameObject.Find("Extra " + player.selectedCamera).transform;
        Destroy(newTransform);
        Destroy(dest);
    }

    void LoopBlock(int previousFace, GameObject wall, ref List<GameObject> listBlocks)
    {
        if (wall.layer == LayerMask.NameToLayer("Blocks"))
        {
            listBlocks.Add(wall);

            for (int i = 0; i < 4; i++)
            {
                // not same as where we came from
                if ((i + 2) % 4 != previousFace)
                {
                    LoopBlock(i, wall.GetComponent<Block>().walls[i], ref listBlocks);
                }
            }
        }
    }
}
