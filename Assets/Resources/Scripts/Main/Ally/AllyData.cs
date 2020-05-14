using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyData : EntityData
{
    private void Update()
    {
        if (BonesCurrent <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
