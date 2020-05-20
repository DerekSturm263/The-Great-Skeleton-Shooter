using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : EntityActions
{
    private GameObject summonSpot;
    public float fireTimer, fireTimeMax;
    public bool fireActive = false, fullAuto = false;

    private void Awake()
    {
        data = GetComponent<PlayerData>();
        summonSpot = GameObject.FindGameObjectWithTag("SummonSpot");
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

        Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (fullAuto)
        {
            float shotRate = bulletRate;
            float shotForce = bulletForce;
            float shotLife = bulletLife;
            ShootAuto(Input.GetButton((data as PlayerData).controls[5]), shotRate, shotForce, shotLife, false);
        }else
        {
            float shotRate = bulletRate;
            float shotForce = bulletForce;
            float shotLife = bulletLife;
            ShootSemiAuto(Input.GetButtonDown((data as PlayerData).controls[5]), shotForce, shotLife * 3, true);
        }
        SummonAlly(Input.GetButtonDown((data as PlayerData).controls[6]));
    }

    // Called when the player moves the mouse or reticle.
    private void Aim(Vector2 input)
    {
        Vector3 worldPos;
        
        worldPos.x = input.x;
        worldPos.y = input.y;       
        worldPos.z = -Camera.main.transform.position.z;

        summonSpot.transform.position = new Vector3(worldPos.x, worldPos.y, 0);
        armPivot.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(worldPos.y - armPivot.transform.position.y, worldPos.x - armPivot.transform.position.x));
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

                GameObject newBullet = Instantiate(bullet, arm.transform.position + arm.transform.right * 0.25f, Quaternion.identity);

                newBullet.GetComponent<Rigidbody2D>().AddForce(arm.transform.right * fireForce);

                if (gravEffect)
                {
                    newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale / 32;
                }else if (!gravEffect)
                {
                    newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale * 0;
                }

                newBullet.GetComponent<Bullet>().SetBulletOwner(0);
                newBullet.transform.rotation = armPivot.transform.rotation;

                Destroy(newBullet, fireLife);
                data.RemoveBone(1);
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

            GameObject newBullet = Instantiate(bullet, arm.transform.position + arm.transform.right * 0.25f, Quaternion.identity);

            newBullet.GetComponent<Rigidbody2D>().AddForce(arm.transform.right * fireForce);

            if (gravEffect)
            {
                newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale;
            }
            else if (!gravEffect)
            {
                newBullet.GetComponent<Rigidbody2D>().gravityScale = newBullet.GetComponent<Rigidbody2D>().gravityScale * 0;
            }

            newBullet.GetComponent<Bullet>().SetBulletOwner(0);
            newBullet.transform.rotation = armPivot.transform.rotation;

            Destroy(newBullet, fireLife);
            data.RemoveBone(1);
        }
    }

    // Called when the player presses the summon button.
    private void SummonAlly(bool input)
    {
        if (input)
        {
            if (data.BonesCurrent <= 10)
                return;

            Instantiate((data as PlayerData).ally, summonSpot.transform.position, Quaternion.identity);
            data.RemoveBone(10);
        }
    }

    // Called when the player touches a bone.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bone"))
        {
            data.AddBone(1);

            Destroy(collision.gameObject);
        }
    }
}
