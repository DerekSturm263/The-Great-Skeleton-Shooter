using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject EnemyPrefab, AssociatedCapZone, ActiveEnemyManagement;
    public float spawnRate, spawnTimer, spawnTimerMax;
    public bool forceSpawn, spawnable;

    // Start is called before the first frame update
    void Start()
    {
        ActiveEnemyManagement = GameObject.Find("ActiveEnemyManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (AssociatedCapZone.GetComponent<ZoneCaptureScript>().PlayersIn >= 1 && spawnable || forceSpawn && spawnable)
        {
            if (spawnTimer == 0)
            {
                GameObject newEnemy = GameObject.Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
                newEnemy.GetComponent<EnemyData>().target = AssociatedCapZone.GetComponent<ZoneCaptureScript>().playerForSpawner;
                newEnemy.GetComponent<EnemyData>().isLockedOn = true;
                newEnemy.GetComponent<EnemyMove>().isLockedOn = true;
                spawnTimer += Time.deltaTime * spawnRate;
                ActiveEnemyManagement.GetComponent<ActiveEnemyManager>().ActiveEnemies++;
            }
            else if (spawnTimer < spawnTimerMax)
            {
                spawnTimer += Time.deltaTime * spawnRate;
            }else if (spawnTimer >= spawnTimerMax)
            {
                spawnTimer = 0;
            }
        }
        if (ActiveEnemyManagement.GetComponent<ActiveEnemyManager>().ActiveEnemies >= ActiveEnemyManagement.GetComponent<ActiveEnemyManager>().SceneEnemyMax)
        {
            spawnable = false;
        }else if (ActiveEnemyManagement.GetComponent<ActiveEnemyManager>().ActiveEnemies < ActiveEnemyManagement.GetComponent<ActiveEnemyManager>().SceneEnemyMax)
        {
            spawnable = true;
        }
    }
}
