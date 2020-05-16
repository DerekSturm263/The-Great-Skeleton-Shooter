using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    public GameObject EnemyPrefab, AssociatedCapZone;
    public float spawnRate, spawnTimer, spawnTimerMax;
    public bool forceSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AssociatedCapZone.GetComponent<ZoneCaptureScript>().PlayersIn >= 1 || forceSpawn)
        {
            if (spawnTimer == 0)
            {
                GameObject newEnemy = GameObject.Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
                newEnemy.GetComponent<EnemyMove>().target = AssociatedCapZone.GetComponent<ZoneCaptureScript>().playerForSpawner;
                spawnTimer += Time.deltaTime * spawnRate;
            }
            else if (spawnTimer < spawnTimerMax)
            {
                spawnTimer += Time.deltaTime * spawnRate;
            }else if (spawnTimer >= spawnTimerMax)
            {
                spawnTimer = 0;
            }
        }
    }
}
