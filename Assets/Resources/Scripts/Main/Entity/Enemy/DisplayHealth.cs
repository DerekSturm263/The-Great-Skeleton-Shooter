using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    public GameObject healthBar;
    private GameObject enemyHealthBar;

    private void OnEnable()
    {
        enemyHealthBar = Instantiate(healthBar, gameObject.transform);
    }

    private void Update()
    {
        enemyHealthBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, GetComponent<EnemyData>().BonesCurrent / GetComponent<EnemyData>().BonesMax);
        enemyHealthBar.transform.localScale = new Vector2(GetComponent<EnemyData>().BonesCurrent / GetComponent<EnemyData>().BonesMax * 2f, 0.25f);

        transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
    }
}
