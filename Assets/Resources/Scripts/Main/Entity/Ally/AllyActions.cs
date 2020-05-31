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
        if ((data as AllyData).isLockedOn && (data as AllyData).canMove)
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
        if (!(data as AllyData).isLockedOn || (data as AllyData).canMove)
            return;

        foreach (GameObject gun in arm)
        {
            GameObject newBullet = Instantiate(bullet, gun.transform.position + gun.transform.right * gun.transform.localScale.x, Quaternion.identity);
            newBullet.transform.localScale = this.gameObject.transform.localScale;
            newBullet.GetComponent<Bullet>().SetBulletOwner(2);
            newBullet.GetComponent<Rigidbody2D>().AddForce(gun.transform.right * bulletForce);
            Destroy(newBullet, bulletLife);
        }
    }
}
