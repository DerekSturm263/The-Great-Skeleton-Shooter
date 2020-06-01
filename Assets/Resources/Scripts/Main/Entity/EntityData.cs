using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb2;

    [Header("Stats")]
    public uint BonesMax;
    public uint BonesCurrent;

    public GameObject deathParticles;
    public GameObject loseBoneParticle;

    public SpriteRenderer[] childrenSpriteRenderers;

    public void OnEnable()
    {
        childrenSpriteRenderers = GetComponentsInChildren(typeof(SpriteRenderer)) as SpriteRenderer[];
    }

    public void AddBone(uint amount)
    {
        BonesCurrent += amount;
    }

    public void RemoveBone(uint amount)
    {
        BonesCurrent -= amount;
    }

    public void FlashRed()
    {
        foreach (SpriteRenderer sprtRndr in childrenSpriteRenderers)
        {
            sprtRndr.color = Color.Lerp(Color.white, Color.red, 1f);
        }
    }
}
