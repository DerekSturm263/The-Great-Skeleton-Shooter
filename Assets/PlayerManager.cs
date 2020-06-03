using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RespawnPlayer()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        if (ZoneCaptureScript.lastZoneCaptured != null)
        {
            player.transform.position = ZoneCaptureScript.lastZoneCaptured.GetComponent<ZoneCaptureScript>().respawnPoint.transform.position;
            ZoneCaptureScript.currentZone.GetComponent<ZoneCaptureScript>().Reset();
            ZoneCaptureScript.lastZoneCaptured.SetActive(true);
            ZoneCaptureScript.lastZoneCaptured.GetComponent<ZoneCaptureScript>().Reset();

            foreach (GameObject flag in ZoneCaptureScript.lastZoneCaptured.GetComponent<ZoneCaptureScript>().associatedFlags)
            {
                flag.GetComponentInChildren<FlagState>().SetState(FlagState.CaptureState.Uncaptured);
            }
            foreach (GameObject flag in ZoneCaptureScript.currentZone.GetComponent<ZoneCaptureScript>().associatedFlags)
            {
                flag.GetComponentInChildren<FlagState>().SetState(FlagState.CaptureState.Uncaptured);
            }
        }
        else
        {
            player.transform.position = new Vector2(-10f, -4f);
        }

        if (ZoneCaptureScript.numCaptured > 0) ZoneCaptureScript.numCaptured--;

        yield return new WaitForSeconds(1f);
        player.SetActive(true);
    }
}
