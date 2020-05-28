using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : EntityData
{
    [Space(10f)]
    public GameObject bone;
    public uint boneDrops;

    public GameObject target;
    [HideInInspector] public bool isLockedOn;

    private void Update()
    {
        if (BonesCurrent <= 0)
            Die();
        if (BonesCurrent > BonesMax)
            BonesCurrent = 0;
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
