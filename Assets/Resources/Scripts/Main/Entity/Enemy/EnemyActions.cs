using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : EntityActions
{
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
        if ((data as EnemyData).isLockedOn)
        {
            Aim((data as EnemyData).target);
        }
    }

    // Called when the player moves the mouse or reticle.
    private void Aim(GameObject target)
    {
        Vector3 pos;

        pos.x = target.transform.position.x;
        pos.y = target.transform.position.y;
        pos.z = 0f;

        armPivot.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(pos.y - armPivot.transform.position.y, pos.x - armPivot.transform.position.x));
    }

    // Called twice every second to try and fire at the player
    private void Shoot()
    {
        if (!(data as EnemyData).isLockedOn)
            return;

        GameObject newBullet = Instantiate(bullet, armPivot.transform.position + armPivot.transform.right * 0.5f, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().AddForce(armPivot.transform.right * bulletForce);
        newBullet.GetComponent<Bullet>().SetBulletOwner(1);
        newBullet.transform.rotation = armPivot.transform.rotation;

        Destroy(newBullet, bulletLife);
    }
}
