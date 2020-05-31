using System.Collections;
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

    private void Die()
    {
        Destroy(gameObject);
        Instantiate(deathParticles, gameObject.transform.position, gameObject.transform.rotation);
    }
}
