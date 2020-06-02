using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EntityMove
{
    public bool isLockedOn;
    private float targetDirection;
    private GameObject player;

    private void Awake()
    {
        data = GetComponent<EnemyData>();
        (data as EnemyData).rb2 = GetComponent<Rigidbody2D>();
        dustParticles = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        (data as EnemyData).target = player;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 15f)
        {
            isLockedOn = true;
            (data as EnemyData).isLockedOn = true;
        }

        if (isLockedOn)
        {
            // Determines whether the target is to the left or right.
            if ((data as EnemyData).target.transform.position.x < transform.position.x)
                targetDirection = -1f;
            else
                targetDirection = 1f;

            // Move towards target.
            if (Mathf.Abs((data as EnemyData).target.transform.position.x) - 7.5f > Mathf.Abs(transform.position.x) ||
                Mathf.Abs((data as EnemyData).target.transform.position.x) + 7.5f < Mathf.Abs(transform.position.x))

                Move(targetDirection, true);

            Run(false);
            Jump((data as EnemyData).target.transform.position.y - 1f > transform.position.y);
        }
    }
}
