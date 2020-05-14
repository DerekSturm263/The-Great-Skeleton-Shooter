using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    
    [Range(1f, 10f)] public float speed;
    [HideInInspector] public float zOffset;

    private void Start()
    {
        zOffset = transform.position.z;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        transform.position = (Vector3) Vector2.Lerp(transform.position, target.transform.position, Time.deltaTime * speed) + new Vector3(0, 0, zOffset);
    }
}
