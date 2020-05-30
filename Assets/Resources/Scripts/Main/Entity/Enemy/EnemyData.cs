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
        if (BonesCurrent <= 0 || BonesCurrent > BonesMax)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
        Instantiate(deathParticles, gameObject.transform.position, gameObject.transform.rotation);
        SpawnBones();
        ActiveEnemyManager.activeEnemies--;
    }

    private void SpawnBones()
    {
        for (int i = 0; i < boneDrops; i++)
        {
            GameObject newBone = Instantiate(bone, gameObject.transform.position, gameObject.transform.rotation);
            newBone.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4, 4), 4));
        }
    }
}
