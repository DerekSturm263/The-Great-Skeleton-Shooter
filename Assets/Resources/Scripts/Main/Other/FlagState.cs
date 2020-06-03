using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagState : MonoBehaviour
{
    public enum CaptureState
    {
        Uncaptured, Captured
    }
    public CaptureState captureState;

    public Sprite uncapturedFlag;
    public Sprite capturedFlag;

    public void SetState(CaptureState cs)
    {
        captureState = cs;
        GetComponent<SpriteRenderer>().sprite = (captureState == CaptureState.Captured) ? capturedFlag : uncapturedFlag;
    }
}
