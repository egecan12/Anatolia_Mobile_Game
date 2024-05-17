using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Animator anim;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    bool isGrounded;
    public Image[] hearts;
    public int maxHealth;
    int currentHealth;
    
    void Start()
    {
        isGrounded = true;
        currentHealth = maxHealth;
        getHealth();
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

    void getHealth()
    {
        for(int i = 0; i <= hearts.Length - 1; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        for(int i=0; i <= currentHealth; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
}
