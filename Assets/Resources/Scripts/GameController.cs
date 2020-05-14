using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GraphicsType
    {
        Fancy = -1, Fast = 1
    }
    public enum PlayerCount
    {
        Singleplayer = 1, Multiplayer
    }

    public static bool isFullscreen;
    public static bool hasMusic;
    public static bool hasSoundEffects;
    public static GraphicsType graphicsType;

    public static PlayerCount playerCount = PlayerCount.Singleplayer;

    public static void SetFullscreen(bool fullscreen)
    {
        isFullscreen = fullscreen;
    }

    public static void SetMusic(bool music)
    {
        hasMusic = music;
    }

    public static void SetSoundEffects(bool soundEffect)
    {
        hasSoundEffects = soundEffect;
    }

    public static void SetGraphics(GraphicsType newGraphicsType)
    {
        graphicsType = newGraphicsType;
    }

    public static void SetPlayerCount(int value)
    {
        playerCount = (PlayerCount) value;
    }
}
