using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCaptureScript : MonoBehaviour
{
    public int PlayersIn = 0;
    public float capCount, capRate, capLimit, capPercent;
    public GameObject zoneDoor;
    public bool capturing, captured;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayersIn > 0)
        {
            capturing = true;
            capPercent = (capCount / capLimit);
        }
        if (capturing && !captured)
        {
            if (capCount < capLimit)
            {
                capCount += Time.deltaTime * capRate * PlayersIn;
            }else if (capCount >= capLimit)
            {
                capCount = capLimit;
                capturing = false;
                captured = true;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayersIn++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayersIn--;
        }
    }
}
