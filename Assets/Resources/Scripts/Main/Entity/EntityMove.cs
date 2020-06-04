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

    public static float jumpHeight = 5f;
    public static float jumpSpeed = 1.5f;

    protected float jumpVel;

    // Boxcast variables
    protected RaycastHit2D hitInfo;
    [SerializeField] protected Vector2 boxSize;
    [SerializeField] protected Vector2 boxOffset;
    [Space(10f)]
    [SerializeField] protected LayerMask groundLayer;

    protected ParticleSystem dustParticles;

    private bool canJump;

    #region Movement

    // Used to make the GameObject move in a direction based on moveSpeed.
    protected void Move(float input, bool isenemy)
    {
        float movementX;

        if (!isenemy)
        {
            movementX = Mathf.Lerp(0f, moveSpeed * input, Mathf.Abs(input));
            data.rb2.velocity = new Vector2(movementX, data.rb2.velocity.y);
        }
        else
        {
            movementX = Mathf.Lerp(0f, moveSpeed * input, Mathf.Abs(input));
            data.rb2.velocity = new Vector2(movementX / 2, data.rb2.velocity.y);
        }

        if (IsGrounded() && Mathf.Abs(data.rb2.velocity.x) > 0.1f)
        {
            if (Mathf.Floor(Time.time * 5) % 1 == 0) dustParticles.Play(false);
            else dustParticles.Stop(false);
        }
    }

    // Used to determine the GameObject's moveSpeed.
    protected void Run(bool input)
    {
        moveSpeed = input ? runSpeed : walkSpeed;
    }

    // Used to make the GameObject jump.
    protected void Jump(bool input)
    {
        Debug.Log(IsGrounded());

        if (IsGrounded())
            canJump = true;
        else
            StartCoroutine(DisableJump());

        if (input)
        {
            if (canJump)
            {
                canJump = false;
                jumpVel = jumpHeight;
                data.rb2.AddForce(new Vector2(0f, jumpVel), ForceMode2D.Impulse);
            }
            else if (!canJump && jumpVel > 0.1f)
            {
                jumpVel /= jumpSpeed;
                data.rb2.AddForce(new Vector2(0f, jumpVel), ForceMode2D.Impulse);
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
        hitInfo = Physics2D.BoxCast((Vector2) transform.position - new Vector2(boxOffset.x, boxOffset.y), boxSize, 0f, Vector2.down, boxSize.y, groundLayer);
        return hitInfo;
    }

    private IEnumerator DisableJump()
    {
        yield return new WaitForSeconds(0.15f);
        canJump = false;
    }

    #endregion
}
