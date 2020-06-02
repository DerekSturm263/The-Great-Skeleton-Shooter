using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EntityMove
{
    public bool isLockedOn;
    private float targetDirection;

    private void Awake()
    {
        data = GetComponent<EnemyData>();
        (data as EnemyData).rb2 = GetComponent<Rigidbody2D>();
        dustParticles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
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

    #region Locking On

    // If a player enters the trigger area, this GameObject will lock onto it.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isLockedOn = true;
            (data as EnemyData).isLockedOn = true;
            (data as EnemyData).target = collision.gameObject;
        }
    }

    // If a player exits the trigger area, this GameObject will forget about it.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isLockedOn = false;
            (data as EnemyData).isLockedOn = false;
            (data as EnemyData).target = null;
        }
    }

    #endregion
}
