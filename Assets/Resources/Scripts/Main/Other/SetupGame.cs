using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public GameObject gameController;

    public GameObject player;
    public Vector2 playerStartPos;
    public GameObject fancyGraphics;
    public GameObject fastGraphics;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length == 0)
            Instantiate(gameController);
        PlayerData.totalPlayerCount = 1;

        #region Create Players

        // Creates player one.
        Instantiate(player, playerStartPos, Quaternion.identity);

        if (GameController.playerCount == GameController.PlayerCount.Multiplayer)
        {
            // Creates player two.
            Instantiate(player, playerStartPos, Quaternion.identity);
        }

        #endregion

        #region Setup Graphics

        if (GameController.graphicsType == GameController.GraphicsType.Fast)
        {
            fancyGraphics.SetActive(false);
            fastGraphics.SetActive(true);
        }
        else
        {
            fancyGraphics.SetActive(true);
            fastGraphics.SetActive(false);
        }

        #endregion

        ZoneCaptureScript.currentZone = null;
        ZoneCaptureScript.numCaptured = 0;
    }
}
