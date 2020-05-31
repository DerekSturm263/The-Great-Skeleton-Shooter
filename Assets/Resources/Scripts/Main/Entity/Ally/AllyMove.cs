﻿using System.Collections;
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
    }

    private void Update()
    {
        if ((data as AllyData).canMove)
        {
            if (isLockedOn)
            {
                // Determines whether the target is to the left or right.
                if ((data as AllyData).target.transform.position.x < transform.position.x)
                    targetDirection = -1f;
                else
                    targetDirection = 1f;

                // Move towards target.
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
