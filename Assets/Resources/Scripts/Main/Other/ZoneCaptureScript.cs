using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneCaptureScript : MonoBehaviour
{
    public static GameObject currentZone;
    public static float percentCaptured;

    private Animator zoneCaptureUI;
    public int PlayersIn = 0;
    public float capCount, capRate, capLimit, capPercent;
    public GameObject zoneDoor;
    public bool capturing, captured;
    public GameObject playerForSpawner;
    private GameObject[] Players;
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
            currentZone = this.gameObject;
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
            zoneCaptureUI.SetBool("disappear", true);
            gameObject.SetActive(false);
            Debug.Log("Captured!");
            percentCaptured += 1f / 6f * 100f;
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
}
