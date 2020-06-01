using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMove : MonoBehaviour
{
    protected EntityData data;

    public enum State
    {
        Idle, Walking, Running, Jumping, Falling, Landing, Stopping
    }
    public State bodyState;

    public static float walkSpeed = 7.5f;
    public static float runSpeed = 15f;

    protected float moveSpeed;

    public static float jumpHeight = 15f;
    public static float jumpSpeed = 1.5f;

    protected float jumpVel;

    // Boxcast variables
    protected RaycastHit2D hitInfo;
    protected Vector2 boxSize = new Vector2(0.5f, 0.01f);
    [Space(10f)]
    [SerializeField] protected LayerMask groundLayer;

    protected ParticleSystem dustParticles;

    #region Movement

    // Used to make the GameObject move in a direction based on moveSpeed.
    protected void Move(float input, bool isenemy)
    {
        if (!isenemy)
        {
            float movementX = Mathf.Lerp(0f, moveSpeed * input, Mathf.Abs(input));
            data.rb2.velocity = new Vector2(movementX, data.rb2.velocity.y);
        }
        else
        {
            float movementX = Mathf.Lerp(0f, moveSpeed * input, Mathf.Abs(input));
            data.rb2.velocity = new Vector2(movementX / 2, data.rb2.velocity.y);
        }

        dustParticles.Play();
    }

    // Used to determine the GameObject's moveSpeed.
    protected void Run(bool input)
    {
        moveSpeed = input ? runSpeed : walkSpeed;
    }

    // Used to make the GameObject jump.
    protected void Jump(bool input)
    {
        if (input)
        {
            if (IsGrounded())
            {
                jumpVel = jumpHeight;
                data.rb2.velocity = new Vector2(data.rb2.velocity.x, jumpVel);
            }
            else if (!IsGrounded() && jumpVel >= 8f)
            {
                jumpVel /= jumpSpeed;
                data.rb2.velocity = new Vector2(data.rb2.velocity.x, jumpVel);
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

    // Used to check if the player is grounded.
    protected bool IsGrounded()
    {
        hitInfo = Physics2D.BoxCast((Vector2)transform.position - new Vector2(0f, boxSize.y + 0.51f), boxSize, 0f, Vector2.down, boxSize.y, groundLayer);
        return hitInfo;
    }

    #endregion
}
