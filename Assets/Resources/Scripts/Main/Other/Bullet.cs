using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletOwner
    {
        Player, Enemy
    }
    public BulletOwner bulletOwner;

    public void SetBulletOwner(int owner)
    {
        bulletOwner = (BulletOwner) owner;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bulletOwner == BulletOwner.Enemy && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerData>().RemoveBone(1);
            Destroy(gameObject);
        }
        else if (bulletOwner == BulletOwner.Player && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyData>().RemoveBone(1);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
