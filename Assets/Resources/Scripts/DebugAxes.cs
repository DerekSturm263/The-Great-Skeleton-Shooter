using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAxes : MonoBehaviour
{
    private void Update()
    {
        Debug.Log("(" + Input.GetAxis("P2Horizontal") + ", " + Input.GetAxis("P2Vertical") + ")");
    }
}
