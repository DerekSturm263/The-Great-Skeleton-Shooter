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

        if (gameObject.CompareTag("Ally"))
            entityHealthBar.transform.localPosition += new Vector3(0.5f, 2.5f);
        else
            entityHealthBar.transform.localPosition += new Vector3(0f, 1f);
    }

    private void Update()
    {
        EntityData data = GetComponent<EntityData>();

        entityHealthBar.GetComponent<SpriteRenderer>().color = Color.Lerp(colorRed, colorGreen, (float) data.BonesCurrent / (float) data.BonesMax);
        
        if (data is AllyData)
            entityHealthBar.transform.localScale = new Vector2((float) data.BonesCurrent / (float) data.BonesMax * 2f, 0.25f); // (2.5f, 0.3f)
        else
            entityHealthBar.transform.localScale = new Vector2((float) data.BonesCurrent / (float) data.BonesMax * 2f, 0.25f); // (2f, 0.25f)
    }
}
