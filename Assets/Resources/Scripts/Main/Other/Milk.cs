using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    public uint RestoreValue;
    public GameObject boneCollecting;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerData>().AddBone(RestoreValue);

            Instantiate(boneCollecting, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
