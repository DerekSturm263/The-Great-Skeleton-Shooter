using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTemplateScript : MonoBehaviour
{
    public string weaponName;
    public bool autoBool, gravEffect;
    public float shotRate, shotForce, shotLife;
    public GameObject endofbarrel;
    public GameObject collectionParticles;
    public uint damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (!collision.gameObject.GetComponent<PlayerActions>().pocketWeapons.Contains(gameObject))
                collision.gameObject.GetComponent<PlayerActions>().pocketWeapons.Add(gameObject);
                gameObject.transform.SetParent(collision.gameObject.GetComponent<PlayerActions>().armPivotForWeaponPlacement.transform);
                Instantiate(collectionParticles, collision.gameObject.transform.position, Quaternion.identity);
                gameObject.transform.localPosition = new Vector3(0, 0, 0);
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            if (collision.gameObject.GetComponent<PlayerActions>().carriedWeapon != null)
                gameObject.SetActive(false);
            else
            {
                collision.gameObject.GetComponent<PlayerActions>().carriedWeapon = gameObject;

                collision.gameObject.GetComponent<PlayerActions>().activeWeaponshotRate = shotRate;
                collision.gameObject.GetComponent<PlayerActions>().activeWeaponShotForce = shotForce;
                collision.gameObject.GetComponent<PlayerActions>().activeWeaponShotLife = shotLife;
                collision.gameObject.GetComponent<PlayerActions>().activeWeaponAutoBool = autoBool;
                collision.gameObject.GetComponent<PlayerActions>().activeWeaponGravEffect = gravEffect;
                collision.gameObject.GetComponent<PlayerActions>().activeWeaponDamage = damage;
        }
    }
}
