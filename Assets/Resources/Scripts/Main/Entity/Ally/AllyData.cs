﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyData : EntityData
{
    [Space(10f)]
    public GameObject bone;

    public GameObject target;
    [HideInInspector] public bool isLockedOn;

    public bool canMove = false;

    private void Update()
    {
        if (BonesCurrent <= 0 || BonesCurrent > BonesMax)
            Die();
    }

    public void Die()
    {
        GameObject bones = Instantiate(deathParticles, gameObject.transform.position, gameObject.transform.rotation);
        bones.transform.localScale = gameObject.transform.localScale / 2f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>().allies.Remove(gameObject);
        Destroy(gameObject);
    }
}
