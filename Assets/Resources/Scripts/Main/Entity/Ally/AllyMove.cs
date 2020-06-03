using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMove : EntityMove
{
    public GameObject summoner;
    public bool isLockedOn;
    private float targetDirection;

    private void Awake()
    {
        data = GetComponent<AllyData>();
        (data as AllyData).rb2 = GetComponent<Rigidbody2D>();
        dustParticles = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        summoner = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if ((data as AllyData).target == null)
            isLockedOn = false;

        if ((data as AllyData).canMove)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (Vector2.Distance(transform.position, g.transform.position) < 15f)
                    {
                        isLockedOn = true;
                        (data as AllyData).isLockedOn = true;
                        (data as AllyData).target = g;
                    }
                }
            }

            if (isLockedOn)
            {
                // Determines whether the target is to the left or right.
                if ((data as AllyData).target.transform.position.x < transform.position.x)
                    targetDirection = -1f;
                else
                    targetDirection = 1f;

                // Move towards target.
                if (Mathf.Abs((data as AllyData).target.transform.position.x) - 5f > Mathf.Abs(transform.position.x) ||
                    Mathf.Abs((data as AllyData).target.transform.position.x) + 5f < Mathf.Abs(transform.position.x))

                    Move(targetDirection, true);

                Run(false);
                Jump((data as AllyData).target.transform.position.y - 1f > transform.position.y);
            }
            else
            {
                // Determines whether the summoner is to the left or right.
                if (summoner.transform.position.x < transform.position.x)
                    targetDirection = -1f;
                else
                    targetDirection = 1f;

                // Move towards target.
                if (Mathf.Abs(summoner.transform.position.x) - 2.5f > Mathf.Abs(transform.position.x) ||
                    Mathf.Abs(summoner.transform.position.x) + 2.5f < Mathf.Abs(transform.position.x))

                    Move(targetDirection, true);

                Run(false);
                Jump(summoner.transform.position.y - 1f > transform.position.y);
            }
        }
    }

    #region Locking On

    // If an enemy enters the trigger area, this GameObject will lock onto it.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isLockedOn = true;
            (data as AllyData).target = collision.gameObject;
        }
    }

    // If an enemy exits the trigger area, this GameObject will forget about it.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isLockedOn = false;
            (data as AllyData).target = null;
        }
    }

    #endregion
}
