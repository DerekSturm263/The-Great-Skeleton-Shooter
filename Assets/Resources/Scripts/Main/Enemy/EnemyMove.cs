using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private enum State
    {
        Idle, Walking, Running, Jumping, Falling, Landing, Stopping
    }

    private Rigidbody2D rb2;
    public GameObject target;

    [Range(0f, 5f)] public float walkSpeed;
    [Range(0f, 10f)] public float runSpeed;
    private float moveSpeed;

    [Range(0f, 100f)] public float jumpHeight;
    [Range(0f, 2f)] public float jumpSpeed;
    private float jumpVel;

    private float playerDirection;

    private bool isLockedOn;

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveSpeed = walkSpeed;

        if (target != null)
        {
            if (target.transform.position.x < transform.position.x)
                playerDirection = -1f;
            else
                playerDirection = 1f;
        }

        Move(playerDirection);
        Jump();
    }

    private void Move(float input)
    {
        if (!isLockedOn)
            return;

        float movementX = Mathf.Lerp(0f, moveSpeed * input, Mathf.Abs(input));
        rb2.velocity = new Vector2(movementX, rb2.velocity.y);
    }

    // Called when the enemy tries to jump.
    private void Jump()
    {
        if (!isLockedOn)
            return;

        if (target.transform.position.y - 0.25f > transform.position.y)
        {
            if (IsGrounded())
            {
                jumpVel = jumpHeight;
                rb2.velocity = new Vector2(rb2.velocity.x, jumpVel);
            }
            else if (!IsGrounded() && jumpVel >= 8f)
            {
                jumpVel /= jumpSpeed;
                rb2.velocity = new Vector2(rb2.velocity.x, jumpVel);
            }
            else
            {
                jumpVel = 0f;
            }

        }
        else
        {
            jumpVel = 0f;
        }
    }

    // If the player enters the Trigger area, the enemy will lock onto it.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isLockedOn = true;
            target = collision.gameObject;
        }
    }

    // If the player exits the Trigger area, the enemy will lock off of it.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isLockedOn = false;
            target = null;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position - new Vector3(0f, 0.7f, 0f), new Vector2(0.1f, 0.1f), 0f, Vector2.down, 0f, 8);
    }
}
