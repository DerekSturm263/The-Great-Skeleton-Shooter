using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject armPivot;
    public GameObject bullet;
    private GameObject target;

    [Range(1f, 10f)] public float bulletLife;
    [Range(1f, 10f)] public float bulletForce;

    private bool isLockedOn;

    private void Start()
    {
        InvokeRepeating("Shoot", 1f, 1f);
    }

    private void Update()
    {
        target = GetComponent<EnemyMove>().target;

        Aim();
    }

    // Called when the player moves the mouse or reticle.
    private void Aim()
    {
        if (target != null)
        {
            Vector3 pos;

            pos.x = target.transform.position.x;
            pos.y = target.transform.position.y;
            pos.z = 0f;

            armPivot.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(pos.y - armPivot.transform.position.y, pos.x - armPivot.transform.position.x));
        }
    }

    // Called twice every second to try and fire at the player
    private void Shoot()
    {
        if (!isLockedOn)
            return;

        GameObject newBullet = Instantiate(bullet, armPivot.transform.position + armPivot.transform.right * 0.5f, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().AddForce(armPivot.transform.right * bulletForce);
        newBullet.GetComponent<Bullet>().SetBulletOwner(1);
        newBullet.transform.rotation = armPivot.transform.rotation;

        Destroy(newBullet, bulletLife);
    }

    // If the player enters the Trigger area, the enemy will lock onto it.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isLockedOn = true;
            target = collision.gameObject;
        }
    }

    // If the player exits the Trigger area, the enemy will lock off of it.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
             isLockedOn = false;
             target = null;
        }
    }
}
