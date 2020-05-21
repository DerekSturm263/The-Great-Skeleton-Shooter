using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyActions : EntityActions
{
    private void Awake()
    {
        data = GetComponent<AllyData>();
    }

    private void Start()
    {
        InvokeRepeating("Shoot", 1f, 1f);
    }

    private void Update()
    {
        if ((data as AllyData).isLockedOn)
        {
            Aim((data as AllyData).target);
        }
    }

    // Called when the player moves the mouse or reticle.
    private void Aim(GameObject target)
    {
        Vector3 pos;

        pos.x = target.transform.position.x;
        pos.y = target.transform.position.y;
        pos.z = 0f;

        armPivot[0].transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(pos.y - armPivot[0].transform.position.y, pos.x - armPivot[0].transform.position.x));
    }

    // Called twice every second to try and fire at the player
    private void Shoot()
    {
        if (!(data as AllyData).isLockedOn)
            return;

        GameObject newBullet = Instantiate(bullet, armPivot[0].transform.position + armPivot[0].transform.right * 0.5f, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().AddForce(armPivot[0].transform.right * bulletForce);
        newBullet.GetComponent<Bullet>().SetBulletOwner(1);
        newBullet.transform.rotation = armPivot[0].transform.rotation;

        Destroy(newBullet, bulletLife);
    }
}
