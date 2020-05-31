﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerActions : EntityActions
{
    private GameObject summonSpot;
    public float fireTimer, fireTimeMax, boneFadeSpeed;
    public bool fireActive = false, fullAuto = false;
    public Image boneUI;
    private Rigidbody2D rb;

    public List<GameObject> pocketWeapons;
    public GameObject carriedWeapon;

    public float activeWeaponshotRate, activeWeaponShotForce, activeWeaponShotLife;
    public bool activeWeaponAutoBool, activeWeaponGravEffect;

    public GameObject newAlly;
    public uint summoningBones;
    public bool summoning;
    public GameObject summoningParticles;
    public GameObject summonedParticles;
    public GameObject boneCollecting;
    public GameObject canSummonParticles;

    private GameObject thisSummoningParticles;

    private void Awake()
    {
        boneUI = GameObject.Find("Bones - Icon").GetComponent<UnityEngine.UI.Image>();
        data = GetComponent<PlayerData>();
        summonSpot = GameObject.FindGameObjectWithTag("SummonSpot");
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        // controls[3] is aimingX.
        // controls[4] is aimingY.
        // controls[5] is shooting.
        // controls[6] is summoning.

        // Disabled these debugs so I could focus on testing multiplayer.

        //Debug.Log(Input.GetAxis(data.controls[3]));
        //Debug.Log(Input.GetAxis(data.controls[4]));

        /*activeWeaponshotRate = carriedWeapon.GetComponent<WeaponTemplateScript>().shotRate;
        activeWeaponShotForce = carriedWeapon.GetComponent<WeaponTemplateScript>().shotForce;
        activeWeaponShotLife = carriedWeapon.GetComponent<WeaponTemplateScript>().shotLife;
        activeWeaponAutoBool = carriedWeapon.GetComponent<WeaponTemplateScript>().autoBool;
        activeWeaponGravEffect = carriedWeapon.GetComponent<WeaponTemplateScript>().gravEffect;*/

        Vector2 aimingVect = new Vector2(Input.GetAxis((data as PlayerData).controls[3]), Input.GetAxis((data as PlayerData).controls[4]));
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (pocketWeapons.Contains(carriedWeapon))
            {
                
            }
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
        Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (activeWeaponAutoBool)
        {
            ShootAuto(Input.GetButton((data as PlayerData).controls[5]), activeWeaponshotRate, activeWeaponShotForce, activeWeaponShotLife, activeWeaponGravEffect);
        }else
        {
            ShootSemiAuto(Input.GetButtonDown((data as PlayerData).controls[5]), activeWeaponShotForce, activeWeaponShotLife, activeWeaponGravEffect);
        }
        StartSummoningAlly(Input.GetButtonDown((data as PlayerData).controls[6]));
        SummonAlly(Input.GetButton((data as PlayerData).controls[6]));
    }

    // Called when the player moves the mouse or reticle.
    private void Aim(Vector2 input)
    {
        Vector3 worldPos;
        
        worldPos.x = input.x;
        worldPos.y = input.y;       
        worldPos.z = -Camera.main.transform.position.z;

        summonSpot.transform.position = new Vector3(worldPos.x, worldPos.y, 0);
        foreach (GameObject pivot in armPivot)
        {
         
                pivot.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(worldPos.y - pivot.transform.position.y, worldPos.x - pivot.transform.position.x));
          
            // pivot.transform.right = summonSpot.transform.position - pivot.transform.position;
            //pivot.transform.LookAt(summonSpot.transform.position);
        }
    }

    // Called when the player presses the shoot button.
    private void ShootAuto(bool input, float firingRate, float fireForce, float fireLife, bool gravEffect)
    {
        if (input)
        {
            if (!fireActive)
            {
                fireActive = true;
                if (data.BonesCurrent <= 1)
                    return;
                foreach (GameObject gun in arm)
                {
                    GameObject newBullet = Instantiate(bullet, gun.transform.position + gun.transform.right * 0.25f, Quaternion.identity);
                    newBullet.GetComponent<Rigidbody2D>().AddForce(gun.transform.right * fireForce);

                    if (gravEffect)
                    {
                        newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale / 32;
                    }
                    else if (!gravEffect)
                    {
                        newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale * 0;
                    }

                    newBullet.GetComponent<Bullet>().SetBulletOwner(0);

                    foreach (GameObject pivot in armPivot)
                    {
                        newBullet.transform.rotation = pivot.transform.rotation;
                    }

                    Destroy(newBullet, fireLife);
                    data.RemoveBone(1);
                }
            }else if (fireActive)
            {
                if (fireTimer < fireTimeMax)
                {
                    fireTimer += Time.deltaTime;
                }else if (fireTimer >= fireTimeMax)
                {
                    fireTimer = 0;
                    fireActive = false;
                }
            }
        }
    }

    private void ShootSemiAuto(bool input, float fireForce, float fireLife, bool gravEffect)
    {
        if (input)
        {
            if (data.BonesCurrent <= 1)
                return;
            foreach (GameObject gun in arm)
            {
                GameObject newBullet = Instantiate(bullet, gun.transform.position + gun.transform.right * 0.25f, Quaternion.identity);
                newBullet.GetComponent<Rigidbody2D>().AddForce(gun.transform.right * fireForce);

                if (gravEffect)
                {
                    newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale;
                }
                else if (!gravEffect)
                {
                    newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale * 0;
                }

                newBullet.GetComponent<Bullet>().SetBulletOwner(0);

                foreach (GameObject pivot in armPivot)
                {
                    newBullet.transform.rotation = pivot.transform.rotation;
                }

                Destroy(newBullet, fireLife);
                data.RemoveBone(1);
            }
        }
    }

    // Called when the player presses the summon button.
    private void StartSummoningAlly(bool input)
    {
        if (input)
        {
            newAlly = Instantiate((data as PlayerData).ally, summonSpot.transform.position, Quaternion.identity);
            newAlly.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            summoningBones = 1;
            thisSummoningParticles = Instantiate(summoningParticles, summonSpot.transform.position, Quaternion.identity);
        }
    }

    // Called when the player holds the summon button.
    private void SummonAlly(bool input)
    {
        if (input && (data as PlayerData).BonesCurrent > 1 && summoningBones <= 50)
        {
            if (Time.frameCount % 5 == 0)
            {
                data.RemoveBone(1);
                summoningBones++;

                if (summoningBones == 21)
                    Instantiate(canSummonParticles, newAlly.transform.position, Quaternion.identity);
            }


            newAlly.transform.position = summonSpot.transform.position;
            newAlly.transform.localScale = new Vector2(summoningBones, summoningBones) / 50f;
            thisSummoningParticles.transform.position = newAlly.transform.position;

            summoning = true;
        }
        else
        {
            if (summoning)
            {
                summoning = false;

                if (summoningBones > 20)
                {
                    newAlly.GetComponent<AllyData>().canMove = true;
                    newAlly.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    newAlly.GetComponent<AllyData>().BonesMax = (uint) ((float) summoningBones / 1.5f);
                    newAlly.GetComponent<AllyData>().BonesCurrent = newAlly.GetComponent<AllyData>().BonesMax;
                    newAlly.GetComponent<AllyMove>().summoner = gameObject;

                    Destroy(thisSummoningParticles);

                    Instantiate(summonedParticles, newAlly.transform.position, Quaternion.identity);
                }
                else
                {
                    Destroy(newAlly);
                    Destroy(thisSummoningParticles);
                    (data as PlayerData).BonesCurrent += summoningBones - 1;
                }
            }
        }
    }

    // Called when the player touches a bone.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bone"))
        {
            data.AddBone(1);

            Destroy(collision.gameObject);
            Instantiate(boneCollecting, collision.gameObject.transform.position, Quaternion.identity);
        }
    }
}
