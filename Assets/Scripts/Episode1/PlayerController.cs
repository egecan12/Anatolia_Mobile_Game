using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    private Vector2 moveDirection;
    public InputActionReference move;
    public Animator animator;
    public SpriteRenderer spriteRenderer; // Add this line

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        // Flip the sprite based on the direction of movement
        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Set isRunning to true if the player is moving, false otherwise
        if (Mathf.Abs(moveDirection.x) > 0.01f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }
}