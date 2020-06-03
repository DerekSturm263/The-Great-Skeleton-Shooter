using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneCaptureScript : MonoBehaviour
{
    public static GameObject currentZone;
    public static float numCaptured;
    public static GameObject lastZoneCaptured;

    private Animator zoneCaptureUI;
    [HideInInspector] public int PlayersIn = 0;
    [HideInInspector] public float capCount;
    [HideInInspector] public float capLimit = 5;
    [HideInInspector] public float capPercent;
    public GameObject zoneDoor;
    [HideInInspector] public bool capturing, captured;
    [HideInInspector] public GameObject playerForSpawner;
    [HideInInspector] private GameObject[] Players;

    [HideInInspector] public List<GameObject> AssociatedEnemies;
    public List<GameObject> associatedFlags;
    public GameObject respawnPoint;

    public Material zoneCaptureMaterial;
    private SpriteRenderer getMat;
    [HideInInspector] public uint currentEnemies;

    [Header("Difficulty Settings")]
    public float capRate;
    public float enemyStrength;
    public uint maxEnemies;
    public float enemyCaptureBoost;
    public float spawnRate;
    public uint bonesRestore;

    private void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        getMat = GetComponent<SpriteRenderer>();
        zoneCaptureUI = GameObject.FindGameObjectWithTag("ZoneCaptureUI").GetComponent<Animator>();
        // Material = getMat.material.;
    }

    private void Update()
    {
        getMat.material.SetFloat("Fill", capPercent);
        if (PlayersIn > 0)
        {
            currentZone = gameObject;
            capturing = true;
            capPercent = (capCount / capLimit);
        }

        if (capturing && !captured)
        {
            if (capCount < capLimit)
            {
                capCount += Time.deltaTime * capRate * PlayersIn;
            }
            else if (capCount >= capLimit)
            {
                capCount = capLimit;
                capturing = false;
                captured = true;
            }
        }
        if (captured)
        {
            lastZoneCaptured = gameObject;
            AssociatedEnemies.RemoveAll((x) => x == null);

            foreach (GameObject enemy in AssociatedEnemies)
            {
                enemy.GetComponent<EnemyData>().Die();
            }

            foreach (GameObject flag in associatedFlags)
            {
                flag.GetComponentInChildren<FlagState>().SetState(FlagState.CaptureState.Captured);
            }

            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerData>().AddBone(bonesRestore);
            }

            zoneCaptureUI.SetBool("disappear", true);
            gameObject.SetActive(false);
            Debug.Log("Captured!");
            numCaptured++;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        zoneCaptureUI.SetBool("disappear", false);

        if (collision.gameObject.tag == "Player")
        {
            PlayersIn++;
            playerForSpawner = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayersIn--;
            playerForSpawner = null;
        }
    }

    public void Reset()
    {
        PlayersIn = 0;
        capCount = 0;
        capPercent = 0f;
        capturing = false;
        captured = false;
        playerForSpawner = null;
    }
}
