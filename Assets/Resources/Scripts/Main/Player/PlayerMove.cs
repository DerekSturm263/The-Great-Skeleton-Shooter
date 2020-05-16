using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerData data;

    private enum State
    {
        Idle, Walking, Running, Jumping, Falling, Landing, Stopping
    }

    private bool Grounded;
    private Rigidbody2D rb2;

    [Range(0f, 10f)] public float walkSpeed;
    [Range(0f, 20f)] public float runSpeed;
    private float moveSpeed;

    [Range(0f, 50)] public float jumpHeight;
    [Range(0f, 5f)] public float jumpSpeed;
    private float jumpVel;

    private void Awake()
    {
        data = GetComponent<PlayerData>();
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // controls[0] is horizontal movement.
        // controls[1] is running.
        // controls[2] is jumping.

        Move(Input.GetAxis(data.controls[0]));
        Run(Input.GetButton(data.controls[1]));
        Jump(Input.GetButton(data.controls[2]));
    }

    // Called when the player moves the horizontal axis.
    private void Move(float input)
    {
        float movementX = Mathf.Lerp(0f, moveSpeed * input, Mathf.Abs(input));
        rb2.velocity = new Vector2(movementX, rb2.velocity.y);
    }

    // Called when the player presses the run button.
    private void Run(bool input)
    {
        moveSpeed = input ? runSpeed : walkSpeed;
    }

    // Called when the player presses the jump button.
    private void Jump(bool input)
    {
        if (input)
        {
            if (Grounded)
            {
                jumpVel = jumpHeight;
                rb2.velocity = new Vector2(rb2.velocity.x, jumpVel);
            }
            else if (!Grounded && jumpVel >= 8f)
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

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position - new Vector3(0f, 0.7f, 0f), new Vector2(0.1f, 0.1f), 0f, Vector2.down, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = false;
        }
    }
}
