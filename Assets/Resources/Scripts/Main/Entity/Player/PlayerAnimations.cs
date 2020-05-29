using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerAnimations : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject player;
    [SerializeField] protected LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo;
        Vector2 boxSize = new Vector2(0.5f, 0.01f);
        hitInfo = Physics2D.BoxCast((Vector2)transform.position - new Vector2(0f, boxSize.y + 0.51f), boxSize, 0f, Vector2.down, boxSize.y, groundLayer);
        
        if (rb.velocity.x <= -0.3)
        {
            //player.transform.localScale = new Vector2(-1,player.transform.localScale.y);
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(rb.velocity.x >= 0.3)
        {
            //player.transform.localScale = new Vector2(1, player.transform.localScale.y);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x) );
        Debug.Log(rb.velocity.x);
        
        
        anim.SetBool("Jump", !hitInfo);
    }
}
