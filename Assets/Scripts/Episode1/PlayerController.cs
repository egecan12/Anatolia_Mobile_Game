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

    // Start is called before the first frame update
    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
