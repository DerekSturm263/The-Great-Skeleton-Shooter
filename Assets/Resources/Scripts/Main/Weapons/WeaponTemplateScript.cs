using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTemplateScript : MonoBehaviour
{
    public string weaponName;
    public bool autoBool, gravEffect;
    public float shotRate, shotForce, shotLife;
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
            if (!collision.gameObject.GetComponent<PlayerActions>().pocketWeapons.Contains(gameObject))
                collision.gameObject.GetComponent<PlayerActions>().pocketWeapons.Add(gameObject);
    }
}
