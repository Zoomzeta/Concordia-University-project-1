using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    List<Block> blocks = new List<Block>();

    void Awake()
    {
        foreach (Block child in GetComponentsInChildren<Block>())
            blocks.Add(child);

        // TODO: store room such that you can delete

        // Grid system used for blocks and see if overlap
        // Collider on blocks to have offset when swap is allowed
    }

    void Update()
    {

    }
}
