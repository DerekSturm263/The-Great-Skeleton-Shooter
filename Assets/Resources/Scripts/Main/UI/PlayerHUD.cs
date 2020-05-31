using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public PlayerData player1Data;
    public PlayerData player2Data;

    public Image bonesFill;
    public TMPro.TMP_Text bonesCount;
    public TMPro.TMP_Text percentCaptured;

    private void Start()
    {
        GameObject player1 = PlayerData.Player(1);
        player1Data = player1.GetComponent<PlayerData>();

        try
        {
            GameObject player2 = PlayerData.Player(2);
            player2Data = player2.GetComponent<PlayerData>();
        }
        catch { }
    }

    private void Update()
    {
        bonesCount.text = player1Data.BonesCurrent.ToString();
        bonesFill.fillAmount = (float) player1Data.BonesCurrent / (float) player1Data.BonesMax;

        percentCaptured.text = Mathf.Round(ZoneCaptureScript.percentCaptured).ToString() + "% Captured Total";
    }
}
