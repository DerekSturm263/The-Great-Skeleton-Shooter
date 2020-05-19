using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : EntityData
{
    [HideInInspector] public uint playerNum;
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

    private void Awake()
    {
        BonesMax = 100;
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
