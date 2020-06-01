﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletOwner
    {
        Player, Enemy, Ally
    }
    public BulletOwner bulletOwner;

    public void SetBulletOwner(int owner)
    {
        bulletOwner = (BulletOwner) owner;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit " + collision.gameObject.name);

        switch (bulletOwner)
        {
            case BulletOwner.Enemy:
                if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ally"))
                {
                    collision.gameObject.GetComponent<EntityData>().RemoveBone(1);
                    GameObject bones = Instantiate(collision.gameObject.GetComponent<EntityData>().loseBoneParticle, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    bones.transform.localScale = collision.gameObject.transform.localScale / 2f;
                    Destroy(gameObject);
                }

                break;

            case BulletOwner.Player:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<EntityData>().RemoveBone(1);
                    GameObject bones = Instantiate(collision.gameObject.GetComponent<EntityData>().loseBoneParticle, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    bones.transform.localScale = collision.gameObject.transform.localScale / 2f;
                    Destroy(gameObject);
                }
                else if (collision.gameObject.CompareTag("Crate"))
                {
                    collision.gameObject.GetComponent<CrateData>().Break();
                    Destroy(gameObject);
                }

                break;

            case BulletOwner.Ally:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<EntityData>().RemoveBone(1);
                    GameObject bones = Instantiate(collision.gameObject.GetComponent<EntityData>().loseBoneParticle, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                    bones.transform.localScale = collision.gameObject.transform.localScale / 2f;
                    Destroy(gameObject);
                }

                break;

            default:
                Debug.Log("Something went wrong.");
                break;
        }
    }
}
