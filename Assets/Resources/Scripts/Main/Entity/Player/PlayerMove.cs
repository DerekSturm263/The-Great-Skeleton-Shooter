using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : EntityMove
{
    private void Awake()
    {
        data = GetComponent<PlayerData>();
        (data as PlayerData).rb2 = GetComponent<Rigidbody2D>();
        dustParticles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // controls[0] is moving.
        // controls[1] is running.
        // controls[2] is jumping.

        Move(Input.GetAxis((data as PlayerData).controls[0]), false);
        Run(Input.GetButton((data as PlayerData).controls[1]));
        Jump(Input.GetButton((data as PlayerData).controls[2]));

        if ((data as PlayerData).rb2.velocity.y > 7.5f) (data as PlayerData).rb2.velocity = new Vector2((data as PlayerData).rb2.velocity.x, 7.5f);
    }
}
