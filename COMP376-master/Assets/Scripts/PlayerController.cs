using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float mPace = 2;
    [SerializeField]
    float smoothness = 0.25f;
    [SerializeField]
    float mSideContinue = 15f;
    [SerializeField]
    float mFrontContinue = 0.5f;
    [SerializeField]
    float mSpeedUp = 2;

    // Audio sources
    public AudioSource openDoorSound;
    public AudioSource closeDoorSound;

    public bool isLerpingTurn = false;
    public bool isLerpingFront = false;
    public bool right = false;
    public bool left = false;
    public bool forward = false;
    public bool backward = false;

    private bool reset = false;
    private bool mRunning = false;
    private Collider coll;
    Vector3 previousPosition;
    Vector3 desiredPosition;
    Quaternion previousRotation;
    Quaternion desiredRotation;
    public Transform god;

    Player player;
    public float timeCount = 0.0f;
    // Door teleport
    bool currentlyInDoor = false;
    Vector3 tpCoords;
    Transform destinationRoom;

    public void setDestinationRoom(Transform o)
    {
        destinationRoom = o;
    }

    // List of rooms whose keys have been colected by the player
    public List<GameObject> keysAvailable;

    public void addKey(GameObject door)
    {
        if (!keysAvailable.Contains(door))
        {
            keysAvailable.Add(door);
        }
    }
    public bool checkKeys(GameObject door)
    {
        return keysAvailable.Contains(door);
    }

    void Start()
    {
        player = GetComponent<Player>();
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    void Update()
    {
        Movement();
        SaveAndSwitch();
        checkTP();
    }

    void FixedUpdate()
    {
        // Determine movement speed
        float moveSpeed = mRunning ? mSpeedUp : 1;
        if (isLerpingTurn)
        {
            if (reset)  // direction change
            {
                timeCount = 0.0f;
                reset = false;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, timeCount);
            timeCount = timeCount + Time.deltaTime * smoothness * moveSpeed;
            if (transform.rotation == desiredRotation)
            {
                previousRotation = desiredRotation;
                isLerpingTurn = false;
                right = false;
                left = false;
                timeCount = 0.0f;
            }
        }
        else if (isLerpingFront)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.fixedDeltaTime * mPace * moveSpeed);
            if (transform.position == desiredPosition)
            {
                previousPosition = desiredPosition;
                isLerpingFront = false;
                forward = false;
                backward = false;
            }
        }
    }

    void Movement()
    {
        // Check if we want to speed up or not
        if (Input.GetButtonDown("SpeedUp"))
        {
            mRunning = true;
        }
        if (Input.GetButtonUp("SpeedUp"))
        {
            mRunning = false;
        }

        // User movement
        if (Input.GetButton("Right") && !isLerpingFront)
        {
            if (!isLerpingTurn)
            {
                previousRotation = transform.rotation;
                desiredRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
                player.currentFace = (player.currentFace + 1) % 4;
                if (player.mSwitch) UpdateFrontFace();
            }
            else if (left) // currently turning left
            {
                left = false;
                reset = true;
                Quaternion tempRot = desiredRotation;
                desiredRotation = previousRotation;
                previousRotation = tempRot;
                player.currentFace = (player.currentFace + 1) % 4;
                if (player.mSwitch) UpdateFrontFace();
            }
            else if (Quaternion.Angle(transform.rotation, desiredRotation) < mSideContinue) // continue to turn
            {
                previousRotation = desiredRotation;
                desiredRotation = previousRotation * Quaternion.Euler(0, 90, 0);
                player.currentFace = (player.currentFace + 1) % 4;
                if (player.mSwitch) UpdateFrontFace();
                timeCount = 0.1f * smoothness;
            }
            isLerpingTurn = true;
            right = true;
        }
        else if (Input.GetButton("Left") && !isLerpingFront)
        {
            if (!isLerpingTurn)
            {
                previousRotation = transform.rotation;
                desiredRotation = transform.rotation * Quaternion.Euler(0, -90, 0);
                player.currentFace = (4 + player.currentFace - 1) % 4;
                if (player.mSwitch) UpdateFrontFace();
            }
            else if (right) // currently turning right
            {
                right = false;
                reset = true;
                Quaternion tempRot = desiredRotation;
                desiredRotation = previousRotation;
                previousRotation = tempRot;
                player.currentFace = (4 + player.currentFace - 1) % 4;

                if (player.mSwitch) UpdateFrontFace();
            }
            else if (Quaternion.Angle(transform.rotation, desiredRotation) < mSideContinue) // continue to turn
            {
                previousRotation = desiredRotation;
                desiredRotation = previousRotation * Quaternion.Euler(0, -90, 0);
                player.currentFace = (4 + player.currentFace - 1) % 4;
                if (player.mSwitch) UpdateFrontFace();
                timeCount = 0.1f * smoothness;
            }
            isLerpingTurn = true;
            left = true;
        }
        else if (Input.GetButton("Forward") && !isLerpingTurn && player.currentBlock.GetComponent<Block>().walls[player.currentFace].layer == LayerMask.NameToLayer("Blocks"))
        {
            if (!isLerpingFront)
            {
                previousPosition = transform.position;
                desiredPosition = transform.position + transform.forward * 2;
                if (player.mSwitch) UpdateFrontFace();
            }
            else if (backward) // currently backing
            {
                backward = false;
                Vector3 tempPos = desiredPosition;
                desiredPosition = previousPosition;
                previousPosition = tempPos;
                if (player.mSwitch) UpdateFrontFace();
            }
            else if (Vector3.Distance(transform.position, desiredPosition) < mFrontContinue) // continue to walk
            {
                if (player.currentBlock.GetComponent<Block>().walls[player.currentFace].GetComponent<Block>().walls[player.currentFace].layer == LayerMask.NameToLayer("Blocks"))
                {
                    previousPosition = desiredPosition;
                    desiredPosition = previousPosition + transform.forward * 2;
                    if (player.mSwitch) UpdateFrontFace();
                }
            }
            isLerpingFront = true;
            forward = true;
        }
        else if (Input.GetButton("Backward") && !isLerpingTurn && player.currentBlock.GetComponent<Block>().walls[(player.currentFace + 2) % 4].layer == LayerMask.NameToLayer("Blocks"))
        {
            if (!isLerpingFront)
            {
                previousPosition = transform.position;
                desiredPosition = transform.position - transform.forward * 2;
                if (player.mSwitch) UpdateFrontFace();
            }
            else if (forward) // currently going forward
            {
                forward = false;
                Vector3 tempPos = desiredPosition;
                desiredPosition = previousPosition;
                previousPosition = tempPos;
                if (player.mSwitch) UpdateFrontFace();
            }
            else if (Vector3.Distance(transform.position, desiredPosition) < mFrontContinue) // continue to walk
            {
                if (player.currentBlock.GetComponent<Block>().walls[(player.currentFace + 2) % 4].GetComponent<Block>().walls[(player.currentFace + 2) % 4].layer == LayerMask.NameToLayer("Blocks"))
                { 
                    previousPosition = desiredPosition;
                    desiredPosition = previousPosition - transform.forward * 2;
                    if (player.mSwitch) UpdateFrontFace();
                }
            }
            isLerpingFront = true;
            backward = true;
        }
    }

    void SaveAndSwitch()
    {
        // Switch selected camera
        if (Input.GetButtonDown("Switch1"))
        {
            god.GetComponent<EmptyRoomObjectScript>().setSelected("Swap: Blocks(1)");
            player.selectedCamera = 0;
        }
        if (Input.GetButtonDown("Switch2"))
        {
            god.GetComponent<EmptyRoomObjectScript>().setSelected("Swap: Blocks(2)");
            player.selectedCamera = 1;
        }
        if (Input.GetButtonDown("Switch3"))
        {
            god.GetComponent<EmptyRoomObjectScript>().setSelected("Swap: Squares");
            god.GetComponent<EmptyRoomObjectScript>().setText("I can change squared objects now");
            player.selectedCamera = 2;
        }
        if (Input.GetButtonDown("Switch4"))
        {
            god.GetComponent<EmptyRoomObjectScript>().setSelected("Swap: Spheres");
            god.GetComponent<EmptyRoomObjectScript>().setText("I can change spherical objects now");
            player.selectedCamera = 3;
        }

        // swap for cam 0 and 1 only
        if (Input.GetButtonDown("Swap") && player.mSwitch && !isLerpingTurn && !isLerpingFront && (player.selectedCamera == 0 || player.selectedCamera == 1))
        {
            Swap();
        }
    }

    // render what we see in front of us
    void UpdateFrontFace()
    {
        foreach (Transform child in player.currentRoom.transform)
            child.gameObject.GetComponent<Block>().priority = 0;
        player.currentBlock.GetComponent<Block>().priority = 20;

        List<GameObject> listBlocks = new List<GameObject>();
        if (player.currentBlock.GetComponent<Block>().walls[player.currentFace].layer == LayerMask.NameToLayer("Blocks"))
            player.currentBlock.GetComponent<Block>().walls[player.currentFace].GetComponent<Block>().priority = 20;
        LoopBlock(player.currentFace, player.currentBlock.GetComponent<Block>().walls[player.currentFace], ref listBlocks, 20);

        // Old gameObject
        foreach (GameObject item in listBlocks)
        {
            if (!item.GetComponent<Block>().collisionList.Count.Equals(0))
            {
                foreach (GameObject block in item.GetComponent<Block>().collisionList)
                {
                    if (item.GetComponent<Block>().priority < block.GetComponent<Block>().priority)
                    {
                        //item.SetActive(false);
                        Stack<Transform> children = new Stack<Transform>();
                        children.Push(item.transform);
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
                    else
                    {
                        //item.SetActive(true);
                        Stack<Transform> children = new Stack<Transform>();
                        children.Push(item.transform);
                        while (children.Count > 0)
                        {
                            Transform current = children.Pop();
                            Renderer renderer = current.GetComponent<Renderer>();
                            Collider collider = current.GetComponent<Collider>();
                            if (renderer != null)
                            {
                                renderer.enabled = true;
                            }
                            if (collider != null)
                            {
                                collider.enabled = true;
                            }
                            foreach (Transform child in current.transform)
                            {
                                children.Push(child);
                            }
                        }
                    }

                }
            }
            else
            {
                //item.SetActive(true);
                Stack<Transform> children = new Stack<Transform>();
                children.Push(item.transform);
                while (children.Count > 0)
                {
                    Transform current = children.Pop();
                    Renderer renderer = current.GetComponent<Renderer>();
                    Collider collider = current.GetComponent<Collider>();
                    if (renderer != null)
                    {
                        renderer.enabled = true;
                    }
                    if (collider != null)
                    {
                        collider.enabled = true;
                    }
                    foreach (Transform child in current.transform)
                    {
                        children.Push(child);
                    }
                }
            }
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
            player.currentBlock.GetComponent<Block>().walls[player.currentFace].GetComponent<Block>().priority = 20;
            LoopBlock(player.currentFace, player.currentBlock.GetComponent<Block>().walls[player.currentFace], ref listBlocks, 20);

            // Old gameObject
            foreach (GameObject item in listBlocks)
            {
                item.transform.parent = target.transform;

                if (!item.GetComponent<Block>().collisionList.Count.Equals(0))
                {
                    while(!item.GetComponent<Block>().collisionList.Count.Equals(0))
                    {
                        GameObject block = item.GetComponent<Block>().collisionList[0];
                        block.SetActive(true);
                        block.GetComponent<Block>().collisionList.Remove(item);
                        item.GetComponent<Block>().collisionList.Remove(block);
                    }
                }
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

    // TODO make it BSF
    void LoopBlock(int previousFace, GameObject wall, ref List<GameObject> listBlocks, double priority)
    {
        if (wall.layer == LayerMask.NameToLayer("Blocks"))
        {
            listBlocks.Add(wall);
            List<int> direction = new List<int>();
            // Facing block will be rendered
            if (wall.GetComponent<Block>().walls[previousFace].layer == LayerMask.NameToLayer("Blocks")) 
            {
                wall.GetComponent<Block>().walls[previousFace].GetComponent<Block>().priority = priority;
                direction.Add(previousFace);
            }
            if (wall.GetComponent<Block>().walls[(4 + previousFace - 1) % 4].layer == LayerMask.NameToLayer("Blocks"))
            {
                wall.GetComponent<Block>().walls[(4 + previousFace - 1) % 4].GetComponent<Block>().priority = priority / 2;
                direction.Add((4 + previousFace - 1) % 4);
            }
            if (wall.GetComponent<Block>().walls[(previousFace + 1) % 4].layer == LayerMask.NameToLayer("Blocks"))
            {
                wall.GetComponent<Block>().walls[(previousFace + 1) % 4].GetComponent<Block>().priority = priority / 2;
                direction.Add((previousFace + 1) % 4);
            }
            priority -= priority / 4;
            foreach (int i in direction)
            {
                LoopBlock(i, wall.GetComponent<Block>().walls[i], ref listBlocks, priority);
            }
        }
    }


    //---------------------------------------------Teleportation mechanic


    //On collision with a block, check if the block is a door block and change boolean accordingly
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Block>().hasDoor(gameObject))
        {
            currentlyInDoor = true;
            tpCoords = other.GetComponent<Block>().getDoorDestination();
            coll = other;
        }
        else
        {
            currentlyInDoor = false;
        }
    }

    //Teleports the player to the corresponding coordinates
    void tpPlayer()
    {
        if (WallDoorScript.end)
        {
            endGame();
        }
        else
        {
            transform.position = tpCoords;
            if (!destinationRoom.gameObject.GetComponent<Room>().mSwitch)
            {
                god.GetComponent<EmptyRoomObjectScript>().setText("I can't swap blocks in here...");
            }
            else
            {
                god.GetComponent<EmptyRoomObjectScript>().setText("I can swap blocks in this place!");
            }
        }  
    }
    public void endGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Scenes/EndGame");
    }

    //Checks if the player is trying to tp by checking if the player is in a block and is pressing the corresponding input
    void checkTP()
    {
        if (currentlyInDoor && Input.GetButtonDown("Jump") && !isLerpingFront && !isLerpingTurn)
        {
            tpPlayer();
            // Opening and closing door sounds are played only upon successful teleportation through the door
            bool locked = coll.GetComponent<Block>().checkDoorLocked();
            if (!locked)
            {
                AudioSource backgroundMusic = GameObject.Find("Player/Player Camera/HorrorBackgroundMusic").GetComponent<AudioSource>();
                backgroundMusic.mute = !backgroundMusic.mute; // Trying temporary mute
                openDoorSound.Play(); // Play the sound of the door opening
                closeDoorSound.PlayDelayed(2.0f); // Play the sound of the door closing, with a delay of 2 seconds
                backgroundMusic.mute = !backgroundMusic.mute; // Unmute
            }
        }
        else if (!currentlyInDoor && Input.GetButtonDown("Jump"))
        {
            Debug.Log("hit working outside");
        }
    }
}
