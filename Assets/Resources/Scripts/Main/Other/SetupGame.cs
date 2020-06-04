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

    private Transform freezeOnPause;
    private void Start()
    {
        MusicPlayer.Play("thegreatskeleteonshooter.ogg");
    }
    private void Awake()
    {
        
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();

        if (GameObject.FindGameObjectsWithTag("GameController").Length == 0)
            Instantiate(gameController);

        PlayerData.totalPlayerCount = 1;

        #region Create Players

        // Creates player one.
        GameObject newPlayer = Instantiate(player, playerStartPos, Quaternion.identity);
        newPlayer.transform.SetParent(freezeOnPause);

        if (GameController.playerCount == GameController.PlayerCount.Multiplayer)
        {
            // Creates player two.
            GameObject newPlayer2 = Instantiate(player, playerStartPos, Quaternion.identity);
            newPlayer2.transform.SetParent(freezeOnPause);
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
