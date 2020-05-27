﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : EntityActions
{
    public bool shoot;
    private void Awake()
    {
        data = GetComponent<EnemyData>();
    }

    private void Start()
    {
        InvokeRepeating("Shoot", 1f, 1f);
    }

    private void Update()
    {
        if ((data as EnemyData).target != null)
        {
            if ((data as EnemyData).BonesCurrent != 0)
            {
                Aim((data as EnemyData).target);
            }
        }
    }

    // Called when the player moves the mouse or reticle.
    private void Aim(GameObject target)
    {
        Vector3 pos;

        pos.x = target.transform.position.x;
        pos.y = target.transform.position.y;
        pos.z = 0f;

        foreach (GameObject pivot in armPivot)
        {
            pivot.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(pos.y - pivot.transform.position.y, pos.x - pivot.transform.position.x));
        }

        shoot = true;
    }

    // Called twice every second to try and fire at the player
    private void Shoot()
    {
        if (shoot)
        {
            foreach (GameObject gun in arm)
            {
                GameObject newBullet = Instantiate(bullet, gun.transform.position + gun.transform.right * gun.transform.localScale.x, Quaternion.identity);
                newBullet.GetComponent<Rigidbody2D>().AddForce(gun.transform.right * bulletForce);
                Destroy(newBullet, bulletLife);
                data.RemoveBone(1);
            }
            shoot = false;
        }
    }
}
