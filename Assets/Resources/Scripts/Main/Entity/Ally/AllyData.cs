using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyData : EntityData
{
    [Space(10f)]
    public GameObject bone;

    [HideInInspector] public GameObject target;
    [HideInInspector] public bool isLockedOn;

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
