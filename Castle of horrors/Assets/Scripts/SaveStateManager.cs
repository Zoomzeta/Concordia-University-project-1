using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStateManager : MonoBehaviour
{
    public class SaveState {
        public Camera camera;
        public GameObject currentRoom;
        public GameObject currentBlock;
        public int currentFace;

        public SaveState(Camera camera, GameObject currentRoom, GameObject currentBlock, int currentFace)
        {
            this.camera = camera;
            this.currentRoom = currentRoom;
            this.currentBlock = currentBlock;
            this.currentFace = currentFace;
        }
    }

    List<SaveState> cameras = new List<SaveState>(); // List of the four cameras to be switched
    public SaveState selectedCamera;                 // Current state selected


    void Start()
    {
        foreach (Camera child in GetComponentsInChildren<Camera>())
            cameras.Add(new SaveState(child, null, null, 0));
    }

    void Update()
    {
        
    }
}
