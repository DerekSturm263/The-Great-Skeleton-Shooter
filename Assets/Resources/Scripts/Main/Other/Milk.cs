using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    public uint RestoreValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            uint newHealth = collision.gameObject.GetComponent<PlayerData>().BonesCurrent + RestoreValue;
            if (newHealth > collision.gameObject.GetComponent<PlayerData>().BonesMax)
            {
                newHealth = collision.gameObject.GetComponent<PlayerData>().BonesMax;
            }
            collision.gameObject.GetComponent<PlayerData>().BonesCurrent = newHealth;
            Destroy(gameObject);
        }
    }
}
