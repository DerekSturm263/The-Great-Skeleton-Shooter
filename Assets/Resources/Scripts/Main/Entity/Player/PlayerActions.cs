using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerActions : EntityActions
{
    private GameObject summonSpot;
    private Image timerCircle;

    public float fireTimer, fireTimeMax, boneFadeSpeed;
    public bool fireActive = false, fullAuto = false;
    public Image boneUI;
    private Rigidbody2D rb;

    //Weapon Rework Stuff
    public List<GameObject> pocketWeapons;
    public GameObject carriedWeapon;
    //This is used only for setting weapon positions
    public GameObject armPivotForWeaponPlacement;

    public float activeWeaponshotRate = 0f, activeWeaponShotForce, activeWeaponShotLife;
    public bool activeWeaponAutoBool, activeWeaponGravEffect;
    public uint activeWeaponDamage;
    public float activeAmmoSize;

    public int activeWeaponFromList = 0;

    public GameObject newAlly;
    public uint summoningBones;
    public bool summoning;
    public GameObject summoningParticles;
    public GameObject summonedParticles;
    public GameObject boneCollecting;
    public GameObject canSummonParticles;
    public bool canSummon;

    private GameObject thisSummoningParticles;

    public List<GameObject> allies;

    public AudioSource shootSound;
    public AudioSource summiningSound;
    public AudioSource SummonedSound;
    public AudioSource DamageSound;
    

    private void Awake()
    {
        carriedWeapon.SetActive(false);
        carriedWeapon = null;
        boneUI = GameObject.Find("Bones - Icon").GetComponent<UnityEngine.UI.Image>();
        data = GetComponent<PlayerData>();
        summonSpot = GameObject.FindGameObjectWithTag("SummonSpot");
        rb = GetComponent<Rigidbody2D>();
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();
        timerCircle = summonSpot.transform.GetChild(0).GetComponent<Image>();
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


        Vector2 aimingVect = new Vector2(Input.GetAxis((data as PlayerData).controls[3]), Input.GetAxis((data as PlayerData).controls[4]));
        if ((Input.GetAxis("Scroll") >=0.1 || Input.GetAxis("Scroll") <= -0.1) && carriedWeapon != null)
        {
            carriedWeapon.SetActive(false);
            activeWeaponFromList++;
            if (activeWeaponFromList >= pocketWeapons.Count)
            {
                activeWeaponFromList = 0;
            }
            carriedWeapon = pocketWeapons[activeWeaponFromList];
            carriedWeapon.SetActive(true);

            activeWeaponshotRate = carriedWeapon.GetComponent<WeaponTemplateScript>().shotRate;
            activeWeaponShotForce = carriedWeapon.GetComponent<WeaponTemplateScript>().shotForce;
            activeWeaponShotLife = carriedWeapon.GetComponent<WeaponTemplateScript>().shotLife;
            activeWeaponAutoBool = carriedWeapon.GetComponent<WeaponTemplateScript>().autoBool;
            activeWeaponGravEffect = carriedWeapon.GetComponent<WeaponTemplateScript>().gravEffect;
            activeWeaponDamage = carriedWeapon.GetComponent<WeaponTemplateScript>().damage;
            activeAmmoSize = carriedWeapon.GetComponent<WeaponTemplateScript>().ammoSize;

            summonSpot.transform.localScale = new Vector2(activeWeaponDamage / 5f + 0.8f, activeWeaponDamage / 5f + 0.8f);
        }

        Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (carriedWeapon != null)
        {
            if (activeWeaponAutoBool)
            {
                ShootAuto(Input.GetButtonDown((data as PlayerData).controls[5]), activeWeaponshotRate, activeWeaponShotForce, activeWeaponShotLife, activeWeaponGravEffect);
            }
            else
            {
                ShootSemiAuto(Input.GetButtonDown((data as PlayerData).controls[5]), activeWeaponShotForce, activeWeaponShotLife, activeWeaponGravEffect);
            }
        }
        StartSummoningAlly(Input.GetButtonDown((data as PlayerData).controls[6]));
        SummonAlly(Input.GetButton((data as PlayerData).controls[6]));

        if (fireTimer < activeWeaponshotRate)
        {
            fireTimer += Time.deltaTime;
        }

        if (carriedWeapon != null) timerCircle.fillAmount = (fireTimer < activeWeaponshotRate) ? (fireTimer / activeWeaponshotRate) : 1f;
        else timerCircle.fillAmount = 0f;
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
            if (fireTimer >= activeWeaponshotRate)
            {
                if (data.BonesCurrent <= 1)
                    return;

                fireTimer = 0f;
                shootSound.Play();
                foreach (GameObject gun in arm)
                {
                    GameObject newBullet = Instantiate(bullet, gun.transform.position + gun.transform.right * 0.25f, Quaternion.identity);
                    newBullet.transform.SetParent(freezeOnPause);
                    newBullet.transform.rotation = gun.transform.rotation;
                    newBullet.GetComponent<Bullet>().damage = activeWeaponDamage;
                    newBullet.transform.localScale = new Vector2(activeAmmoSize, activeAmmoSize);
                    carriedWeapon.GetComponent<ParticleSystem>().Play();
                    newBullet.GetComponent<Rigidbody2D>().AddForce(gun.transform.right * fireForce);

                    if (gravEffect)
                    {
                        newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale / 32;
                        data.RemoveBone(1);
                    }
                    else
                    {
                        newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale * 0;
                        data.RemoveBone(2);
                    }

                    newBullet.GetComponent<Bullet>().SetBulletOwner(0);

                    foreach (GameObject pivot in armPivot)
                    {
                        newBullet.transform.rotation = pivot.transform.rotation;
                    }

                    Destroy(newBullet, fireLife);
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
                newBullet.transform.SetParent(freezeOnPause);
                newBullet.transform.rotation = gun.transform.rotation;
                newBullet.GetComponent<Bullet>().damage = activeWeaponDamage;
                carriedWeapon.GetComponent<ParticleSystem>().Play();
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
            canSummon = true;
            newAlly = Instantiate((data as PlayerData).ally, summonSpot.transform.position, Quaternion.identity);
            newAlly.transform.SetParent(freezeOnPause);
            newAlly.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            newAlly.GetComponent<CapsuleCollider2D>().enabled = false;
            summoningBones = 1;
            thisSummoningParticles = Instantiate(summoningParticles, summonSpot.transform.position, Quaternion.identity);
            summiningSound.Play();
        }
    }
    public void Ouch()
    {
        DamageSound.Play();
    }
    // Called when the player holds the summon button.
    private void SummonAlly(bool input)
    {
        if (input && (data as PlayerData).BonesCurrent > 1 && summoningBones <= 50 && canSummon)
        {
            if (Mathf.Floor(Time.time * 3) % 1 == 0)
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
        else if (input && (data as PlayerData).BonesCurrent == 1)
        {
            summoning = false;
            canSummon = false;

            Destroy(newAlly);
            Destroy(thisSummoningParticles);
            (data as PlayerData).BonesCurrent += summoningBones - 1;
        }
        else
        {
            if (summoning && canSummon)
            {
                summoning = false;
                SummonedSound.Play();
                if (summoningBones > 20)
                {
                    newAlly.GetComponent<AllyData>().canMove = true;
                    newAlly.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    newAlly.GetComponent<AllyData>().BonesMax = (uint) ((float) summoningBones / 1.5f);
                    newAlly.GetComponent<AllyData>().BonesCurrent = newAlly.GetComponent<AllyData>().BonesMax;
                    newAlly.GetComponent<AllyMove>().summoner = gameObject;
                    newAlly.GetComponent<CapsuleCollider2D>().enabled = true;

                    allies.Add(newAlly);

                    Destroy(thisSummoningParticles);

                    Instantiate(summonedParticles, newAlly.transform.position, Quaternion.identity);
                }
                else
                {
                    Destroy(newAlly);
                    Destroy(thisSummoningParticles);
                    (data as PlayerData).BonesCurrent += summoningBones - 1;
                    summiningSound.Stop();
                }
            }
        }
    }

    // Called when the player touches a bone.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bone"))
        {
            data.AddBone(2);

            Instantiate(boneCollecting, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
