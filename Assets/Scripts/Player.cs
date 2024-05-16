using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Animator anim;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    bool isGrounded;
    
    void Start()
    {
        isGrounded = true;
    }

    void Update()
    {
        Movement();
    }

    void Movement()   //Character movement class
    {
        bool isRunning = false;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            isRunning = true;
            sr.flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            isRunning = true;
            sr.flipX = false;

        }
        if (Input.GetKey(KeyCode.W))
        {
            if(isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;   
            }
        }

        anim.SetBool("isRunning", isRunning);

    }

    void OnCollisionEnter2D(Collision2D col)  //Checks characters collisions
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
