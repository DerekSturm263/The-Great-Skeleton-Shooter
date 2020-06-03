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

    public void AddBone(uint amount)
    {
        if (BonesCurrent + amount <= BonesMax)
            BonesCurrent += amount;
        else
            BonesCurrent = BonesMax;
    }

    public void RemoveBone(uint amount)
    {
        if ((int) BonesCurrent - amount > 0)
            BonesCurrent -= amount;
        else
            BonesCurrent = 0;
    }
}
