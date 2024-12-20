﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerData : EntityData
{
    [HideInInspector] public uint playerNum = 0;
    public static uint totalPlayerCount = 1;
    
    [Space(10f)]
    public string[] controls = new string[7];

    // controls[0] is horizontal movement.
    // controls[1] is running.
    // controls[2] is jumping.
    // controls[3] is aimingX.
    // controls[4] is aimingY.
    // controls[5] is shooting.
    // controls[6] is summoning.

    [Space(10f)]
    public GameObject ally;

    public GameObject playerManager;
    public AudioSource deathsound;
    private void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        BonesCurrent = BonesMax;

        if (GameController.playerCount == GameController.PlayerCount.Singleplayer)
        {
            controls[0] = "Horizontal";
            controls[1] = "Run";
            controls[2] = "Jump";
            controls[3] = "AimX";
            controls[4] = "AimY";
            controls[5] = "Shoot";
            controls[6] = "Summon";
        }
        else
        {
            switch (totalPlayerCount)
            {
                case 1:
                    controls[0] = "Horizontal";
                    controls[1] = "Run";
                    controls[2] = "Jump";
                    controls[3] = "AimX";
                    controls[4] = "AimY";
                    controls[5] = "Shoot";
                    controls[6] = "Summon";

                    break;

                case 2:
                    controls[0] = "P2Horizontal";
                    controls[1] = "P2Run";
                    controls[2] = "P2Jump";
                    controls[3] = "P2AimX";
                    controls[4] = "P2AimY";
                    controls[5] = "P2Shoot";
                    controls[6] = "P2Summon";

                    break;
            }
        }

        playerNum = totalPlayerCount;
        totalPlayerCount++;
        Debug.Log("players: " + totalPlayerCount);
        Debug.Log(playerNum);
        gameObject.name = playerNum.ToString();
    }

    private void OnEnable()
    {
        BonesCurrent = BonesMax;
    }

    private void Update()
    {
        if (BonesCurrent <= 0 || BonesCurrent > BonesMax)
            Die();
    }

    public void Die()
    {
        GameObject bones = Instantiate(deathParticles, gameObject.transform.position, gameObject.transform.rotation);
        bones.transform.localScale = gameObject.transform.localScale / 2f;

        if (GetComponent<PlayerActions>().allies.Count > 0)
        {
            try
            {
                GetComponent<PlayerActions>().allies.RemoveAll((x) => x == null);
            } catch { }
            GetComponent<PlayerActions>().allies.ForEach((x) => x.GetComponent<AllyData>().Die());
        }
        deathsound.Play();  
        playerManager.GetComponent<PlayerManager>().RespawnPlayer();
        gameObject.SetActive(false);
    }

    public static GameObject Player(uint num)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerData>().playerNum == num)
            {
                Debug.Log(player.GetComponent<PlayerData>().playerNum);
                Debug.Log(num);

                return player;
            }
        }
        
        return null;
    }
}
