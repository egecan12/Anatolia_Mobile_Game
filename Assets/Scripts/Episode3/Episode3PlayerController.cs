using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Include this namespace
public class Episode3PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    private bool isJumping;
    public float jumpForce = 600f; // The upward force
    public Animator anim;
    public InputActionReference jump;
    public int currentHealth;
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("isRunning", true);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (jump.action.triggered && currentHealth > 0)
        {
            if (isGrounded)
            {
                Jump();
            }

        }

    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce));
        isJumping = true;
        isGrounded = false;
        anim.SetBool("isJumping", isJumping);
    }

    public void WaitForAnimation()
    {
        isJumping = false;
        anim.SetBool("isJumping", isJumping);
    }

    void OnCollisionEnter2D(Collision2D col)  //Checks characters collisions
    {

        if (col.gameObject.tag == "Ground")
        {
            Debug.Log("grounded true");
            isGrounded = true;
        }
    }
}
