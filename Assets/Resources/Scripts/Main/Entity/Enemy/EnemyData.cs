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

    public void Die()
    {
        GameObject bones = Instantiate(deathParticles, gameObject.transform.position, gameObject.transform.rotation);
        bones.transform.localScale = gameObject.transform.localScale / 2f;
        Destroy(gameObject);
        SpawnBones();
        ActiveEnemyManager.activeEnemies--;
        ZoneCaptureScript.currentZone.GetComponent<ZoneCaptureScript>().capCount += 0.75f;
    }

    private void SpawnBones()
    {
        for (int i = 0; i < boneDrops; i++)
        {
            GameObject newBone = Instantiate(bone, gameObject.transform.position, gameObject.transform.rotation);
            newBone.GetComponent<Rigidbody2D>().SetRotation(Random.Range(0f, 360f));
            newBone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, 2.5f), 5f);
        }
    }
}
