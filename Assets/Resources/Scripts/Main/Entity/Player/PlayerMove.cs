using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : EntityMove
{
    private void Awake()
    {
        data = GetComponent<PlayerData>();
        (data as PlayerData).rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // controls[0] is moving.
        // controls[1] is running.
        // controls[2] is jumping.

        Move(Input.GetAxis((data as PlayerData).controls[0]), false);
        Run(Input.GetButton((data as PlayerData).controls[1]));
        Jump(Input.GetButton((data as PlayerData).controls[2]));
    }
}
