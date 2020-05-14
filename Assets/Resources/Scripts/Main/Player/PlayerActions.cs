using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private PlayerData data;

    [Header("Shooting")]
    public GameObject arm;
    public GameObject armPivot;
    public GameObject bullet;
    [Range(1f, 10f)] public float bulletLife;
    [Range(1f, 10f)] public float bulletForce;

    [Header("Ally Summon")]
    public GameObject ally;
    private GameObject summonSpot;

    private Vector2 mousePos;

    private void Awake()
    {
        data = GetComponent<PlayerData>();
    }

    private void Start()
    {
        summonSpot = GameObject.FindGameObjectWithTag("SummonSpot");
    }

    private void Update()
    {
        // controls[3] is aimingX.
        // controls[4] is aimingY.
        // controls[5] is shooting.
        // controls[6] is summoning.

        //disabled these debugs so I could focus on testing Multiplayer

        //Debug.Log(Input.GetAxis(data.controls[3]));
        //Debug.Log(Input.GetAxis(data.controls[4]));

        Vector2 aimingVect = new Vector2(Input.GetAxis(data.controls[3]), Input.GetAxis(data.controls[4]));

        Aim(Input.mousePosition);
        Shoot(Input.GetButtonDown(data.controls[5]));
        SummonAlly(Input.GetButtonDown(data.controls[6]));
    }

    // Called when the player moves the mouse or reticle.
    private void Aim(Vector2 input)
    {
        Vector3 worldPos;

        worldPos.x = input.x;
        worldPos.y = input.y;
        worldPos.z = -Camera.main.transform.position.z;

        mousePos = Camera.main.ScreenToWorldPoint(worldPos);

        summonSpot.transform.position = mousePos;
        armPivot.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(mousePos.y - armPivot.transform.position.y, mousePos.x - armPivot.transform.position.x));
    }

    // Called when the player presses the shoot button.
    private void Shoot(bool input)
    {
        if (input)
        {
            if (data.BonesCurrent <= 1)
                return;

            GameObject newBullet = Instantiate(bullet, arm.transform.position + arm.transform.right * 0.25f, Quaternion.identity);

            newBullet.GetComponent<Rigidbody2D>().AddForce(arm.transform.right * bulletForce);
            newBullet.GetComponent<Bullet>().SetBulletOwner(0);
            newBullet.transform.rotation = armPivot.transform.rotation;

            Destroy(newBullet, bulletLife);
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

            Instantiate(ally, transform.position, Quaternion.identity);
            data.RemoveBone(10);
        }
    }

    // Called when the player touches a bone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bone"))
        {
            data.AddBone(1);

            Destroy(collision.gameObject);
        }
    }
}
