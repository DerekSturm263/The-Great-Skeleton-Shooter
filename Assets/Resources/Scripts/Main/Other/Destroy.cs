using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float lifeTime;

    private void OnEnable()
    {
        Invoke("DestroyThis", lifeTime);
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
