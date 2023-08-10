using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 3f;
    // Start is called before the first frame update
    public Vector3 playerMovement;
    public CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        playerMovement = new Vector3(horz, 0, vert).normalized * speed * Time.deltaTime;
        cc.Move(playerMovement);
    }
}
