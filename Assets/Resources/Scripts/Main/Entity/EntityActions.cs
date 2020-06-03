using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityActions : MonoBehaviour
{
    protected EntityData data;
    protected Transform freezeOnPause;

    public enum State
    {
        Aiming, Shooting, Summoning
    }
    public State armState;

    [Space(10f)]

    [SerializeField] public List<GameObject> arm;
    [SerializeField] public List<GameObject> armPivot;
    [SerializeField] public GameObject bullet;

    public float bulletLife = 2.5f;
    public float bulletForce = 10f;
    public float bulletRate = 1f;
}
