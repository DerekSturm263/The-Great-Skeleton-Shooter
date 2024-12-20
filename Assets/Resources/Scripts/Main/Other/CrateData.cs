﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateData : MonoBehaviour
{
    public GameObject[] contents;
    public GameObject breakingParticles;
    protected Transform freezeOnPause;
    public AudioSource boomb;

    private void Awake()
    {
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();
    }

    public void Break()
    {
        Instantiate(breakingParticles, gameObject.transform.position, Quaternion.identity);

        foreach (GameObject g in contents)
        {
            SpawnItem(g);
        }

        Destroy(gameObject);
    }
    public void boom()
    {
        boomb.Play();
    }
    private void SpawnItem(GameObject g)
    {
        GameObject newItem = Instantiate(g, gameObject.transform.position, gameObject.transform.rotation);
        newItem.transform.SetParent(freezeOnPause);

        if (newItem.CompareTag("Ally")) newItem.GetComponent<AllyData>().canMove = true;

        if (newItem.GetComponent<Rigidbody2D>() != null)
        {
            newItem.GetComponent<Rigidbody2D>().SetRotation(Random.Range(0f, 360f));
            newItem.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 5f);
        }
    }
}
