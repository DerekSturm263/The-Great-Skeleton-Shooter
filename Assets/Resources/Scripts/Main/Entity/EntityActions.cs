using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityActions : MonoBehaviour
{
    protected EntityData data;

    public enum State
    {
        Aiming, Shooting, Summoning
    }
    public State armState;

    [Space(10f)]

    [SerializeField] protected GameObject arm;
    [SerializeField] protected GameObject armPivot;
    [SerializeField] protected GameObject bullet;

    public static float bulletLife = 2.5f;
    public static float bulletForce = 10f;
}
