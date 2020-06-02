using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject EnemyPrefab, AssociatedCapZone;
    public float spawnRate, spawnTimer, spawnTimerMax;
    public bool forceSpawn, spawnable;

    private void Update()
    {
        if (AssociatedCapZone != null)
        {
            if (AssociatedCapZone.GetComponent<ZoneCaptureScript>().PlayersIn >= 1 && spawnable || forceSpawn && spawnable)
            {
                if (spawnTimer == 0)
                {
                    GameObject newEnemy = GameObject.Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
                    newEnemy.GetComponent<EnemyData>().target = AssociatedCapZone.GetComponent<ZoneCaptureScript>().playerForSpawner;
                    newEnemy.GetComponent<EnemyData>().isLockedOn = true;
                    newEnemy.GetComponent<EnemyMove>().isLockedOn = true;
                    AssociatedCapZone.GetComponent<ZoneCaptureScript>().AssociatedEnemies.Add(newEnemy);
                    spawnTimer += Time.deltaTime * spawnRate;
                    ActiveEnemyManager.activeEnemies++;
                }
                else if (spawnTimer < spawnTimerMax)
                {
                    spawnTimer += Time.deltaTime * spawnRate;
                }
                else if (spawnTimer >= spawnTimerMax)
                {
                    spawnTimer = 0;
                }
            }

            if (ActiveEnemyManager.activeEnemies >= ActiveEnemyManager.sceneEnemyMax)
                spawnable = false;
            else if (ActiveEnemyManager.activeEnemies < ActiveEnemyManager.sceneEnemyMax)
                spawnable = true;
        }
    }
}
