using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateData : MonoBehaviour
{
    public GameObject[] contents;
    public GameObject breakingParticles;

    public void Break()
    {
        Instantiate(breakingParticles, gameObject.transform.position, Quaternion.identity);

        foreach (GameObject g in contents)
        {
            SpawnItem(g);
        }

        Destroy(this.gameObject);
    }

    private void SpawnItem(GameObject g)
    {
        GameObject newItem = Instantiate(g, gameObject.transform.position, gameObject.transform.rotation);
        newItem.GetComponent<Rigidbody2D>().SetRotation(Random.Range(0f, 360f));
        newItem.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 5f);
    }
}
