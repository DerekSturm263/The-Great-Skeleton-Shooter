using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyActions : EntityActions
{
    private AllyMove move;

    private void Awake()
    {
        data = GetComponent<AllyData>();
        move = GetComponent<AllyMove>();
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();
    }

    private void Start()
    {
        InvokeRepeating("Shoot", 0.5f, 0.5f);
    }

    private void Update()
    {
        if ((data as AllyData).canMove && move.isLockedOn)
        {
            Aim((data as AllyData).target);
        }
    }

    private void Aim(GameObject target)
    {
        Vector3 pos;

        pos.x = target.transform.position.x;
        pos.y = target.transform.position.y;
        pos.z = 0f;

        armPivot[0].transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(pos.y - armPivot[0].transform.position.y, pos.x - armPivot[0].transform.position.x));
    }

    private void Shoot()
    {
        if (!move.isLockedOn || !(data as AllyData).canMove)
            return;

        foreach (GameObject gun in arm)
        {
            GameObject newBullet = Instantiate(bullet, gun.transform.position + gun.transform.right * gun.transform.localScale.x, Quaternion.identity);
            newBullet.transform.SetParent(freezeOnPause);
            newBullet.transform.localScale = gameObject.transform.localScale;
            newBullet.GetComponent<Bullet>().SetBulletOwner(2);
            newBullet.GetComponent<Rigidbody2D>().AddForce(gun.transform.right * bulletForce);
            Destroy(newBullet, bulletLife);
        }
    }
}
