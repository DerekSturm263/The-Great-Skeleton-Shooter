using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EntityMove
{
    public bool isLockedOn;
    private float targetDirection;
    private GameObject player;
    private PlayerActions playerActions;

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
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if ((data as EnemyData).target == null)
            isLockedOn = false;

        if (Vector2.Distance(transform.position, player.transform.position) < 15f)
        {
            isLockedOn = true;
            (data as EnemyData).isLockedOn = true;
            (data as EnemyData).target = player;
        }

        if (playerActions.allies.Count > 0)
        {
            foreach (GameObject g in playerActions.allies)
            {
                if (Vector2.Distance(transform.position, g.transform.position) < 15f && g.GetComponent<AllyData>().canMove)
                {
                    isLockedOn = true;
                    (data as EnemyData).isLockedOn = true;
                    (data as EnemyData).target = g;
                }
            }
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
            Jump((data as EnemyData).target.transform.position.y + 7.5f > transform.position.y);
            Debug.Log(gameObject.name + " " + ((data as EnemyData).target.transform.position.y + 7.5f > transform.position.y));
        }
    }
}
