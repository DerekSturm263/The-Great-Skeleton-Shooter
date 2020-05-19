using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb2;

    [Header("Stats")]
    public uint BonesMax;
    public uint BonesCurrent;

    public void AddBone(uint amount)
    {
        BonesCurrent += amount;
    }

    public void RemoveBone(uint amount)
    {
        BonesCurrent -= amount;
    }
}
