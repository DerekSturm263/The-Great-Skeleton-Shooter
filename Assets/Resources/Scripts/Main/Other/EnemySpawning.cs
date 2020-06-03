using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject EnemyPrefab, AssociatedCapZone;
    public float spawnTimer, spawnTimerMax;
    public bool forceSpawn, spawnable;

    protected Transform freezeOnPause;

    private void Awake()
    {
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();
    }

    private void Update()
    {
        if (AssociatedCapZone != null)
        {
            if (AssociatedCapZone.GetComponent<ZoneCaptureScript>().PlayersIn >= 1 &&
                AssociatedCapZone.GetComponent<ZoneCaptureScript>().currentEnemies < AssociatedCapZone.GetComponent<ZoneCaptureScript>().maxEnemies || forceSpawn && spawnable)
            {
                if (spawnTimer == 0)
                {
                    GameObject newEnemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
                    AssociatedCapZone.GetComponent<ZoneCaptureScript>().currentEnemies++;
                    newEnemy.transform.SetParent(freezeOnPause);
                    newEnemy.GetComponent<EnemyData>().BonesMax = (uint)(AssociatedCapZone.GetComponent<ZoneCaptureScript>().enemyStrength / 2f);
                    newEnemy.GetComponent<EnemyData>().BonesCurrent = newEnemy.GetComponent<EnemyData>().BonesMax;
                    newEnemy.transform.localScale = new Vector3(AssociatedCapZone.GetComponent<ZoneCaptureScript>().enemyStrength, AssociatedCapZone.GetComponent<ZoneCaptureScript>().enemyStrength, AssociatedCapZone.GetComponent<ZoneCaptureScript>().enemyStrength) / 20f;

                    newEnemy.GetComponent<EnemyData>().target = AssociatedCapZone.GetComponent<ZoneCaptureScript>().playerForSpawner;
                    newEnemy.GetComponent<EnemyData>().isLockedOn = true;
                    newEnemy.GetComponent<EnemyMove>().isLockedOn = true;

                    AssociatedCapZone.GetComponent<ZoneCaptureScript>().AssociatedEnemies.Add(newEnemy);
                    spawnTimer += Time.deltaTime * AssociatedCapZone.GetComponent<ZoneCaptureScript>().spawnRate;
                }
                else if (spawnTimer < spawnTimerMax)
                {
                    spawnTimer += Time.deltaTime * AssociatedCapZone.GetComponent<ZoneCaptureScript>().spawnRate;
                }
                else if (spawnTimer >= spawnTimerMax)
                {
                    spawnTimer = 0;
                }
            }
        }
    }
}
