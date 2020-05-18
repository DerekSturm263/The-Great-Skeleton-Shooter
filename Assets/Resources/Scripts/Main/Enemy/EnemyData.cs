using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : EntityData
{
    public uint boneDrops;
    
    private void Update()
    {
        if (BonesCurrent <= 0)
            Die();
    }

    private void Die()
    {
        // Drops boneDrops bones.
        for (int i = 0; i < boneDrops; i++)
        {
            Instantiate(bone, gameObject.transform.position, Quaternion.identity);
        }
        GameObject.Find("ActiveEnemyManager").GetComponent<ActiveEnemyManager>().ActiveEnemies -= 1;
        Destroy(gameObject);
    }
}
