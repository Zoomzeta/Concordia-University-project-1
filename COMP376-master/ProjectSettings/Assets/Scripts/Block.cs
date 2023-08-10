using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public List<GameObject> walls;    // Element 0 : UP
                                      // Element 1: Right
                                      // Element 2: Down
                                      // Element 3: Left

    void Awake()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            // prevent overlap by not rendering them
            //foreach (Renderer r in col.gameObject.GetComponentsInChildren<Renderer>())
            //    r.enabled = false;
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
            }
        }
    }
}
