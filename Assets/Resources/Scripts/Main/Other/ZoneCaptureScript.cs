using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneCaptureScript : MonoBehaviour
{
    public static GameObject currentZone;
    public static float numCaptured;

    private Animator zoneCaptureUI;
    public int PlayersIn = 0;
    public float capCount, capRate, capLimit, capPercent;
    public GameObject zoneDoor;
    public bool capturing, captured;
    public GameObject playerForSpawner;
    private GameObject[] Players;

    public List<GameObject> AssociatedEnemies;
    public List<GameObject> associatedFlags;
    public GameObject respawnPoint;

    public bool loadTitleOnCap;
    public Material zoneCaptureMaterial;
    SpriteRenderer getMat;

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
                uint newHealths = player.GetComponent<PlayerData>().BonesCurrent + (player.GetComponent<PlayerData>().BonesMax / 2);
                if (newHealths > player.GetComponent<PlayerData>().BonesMax)
                {
                    newHealths = player.GetComponent<PlayerData>().BonesMax;
                }
                player.GetComponent<PlayerData>().BonesCurrent = newHealths;
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
        capRate = 0.2f;
        capLimit = 5f;
        capPercent = 0f;
        capturing = false;
        captured = false;
    }
}
