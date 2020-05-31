using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    public GameObject healthBar;
    private GameObject entityHealthBar;

    public Color colorRed;
    public Color colorGreen;

    private void OnEnable()
    {
        entityHealthBar = Instantiate(healthBar, gameObject.transform);
        entityHealthBar.transform.localPosition += new Vector3(0f, 1f);
    }

    private void Update()
    {
        EntityData data = GetComponent<EntityData>();

        entityHealthBar.GetComponent<SpriteRenderer>().color = Color.Lerp(colorRed, colorGreen, (float) data.BonesCurrent / (float) data.BonesMax);
        entityHealthBar.transform.localScale = new Vector2((float) data.BonesCurrent / (float) data.BonesMax * 2f, 0.25f);
    }
}
