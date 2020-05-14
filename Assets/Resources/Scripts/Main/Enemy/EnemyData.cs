using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : EntityData
{
    private void Awake()
    {
        BonesMax = 10;
        BonesCurrent = BonesMax;
    }

    private void Update()
    {
        if (BonesCurrent <= 0)
            Die();
    }

    private void Die()
    {
        // Drops 5 bones.
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bone, gameObject.transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
