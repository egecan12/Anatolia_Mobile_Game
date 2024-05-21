using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : MonoBehaviour
{
    private int currentHealth = 1;
    private float moveSpeed = 5f; // The speed at which the bird moves
    public BirdSpawner enemySpawner;
    private Animator anim; // Define anim
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Assign a value to enemySpawner
        enemySpawner = FindObjectOfType<BirdSpawner>();

    }

    // Update is called once per frame
    void Update()
    {
        // Move the bird to the left
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        if (currentHealth <= 0)
        {
            Die();
        }

        // Assuming rb is the Rigidbody2D component of the bird
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        else
        {
            Debug.LogError("Rigidbody2D component missing from this gameobject");
        }

    }
    void OnTriggerEnter2D(Collider2D other)  //Checks characters collisions
    {
        // Falling reduces the health logic
        if (other.gameObject.tag == "Death")
        {
            Debug.Log("triggered");

            // Reduce current health
            currentHealth -= 10; // or any value you want

        }

        // Check if the bird has collided with the player
        if (other.gameObject.tag == "Player")
        {
            // Get the Animator component
            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                // Set the isExploding animation parameter to true
                anim.SetBool("isExploding", true);

                // Start a Coroutine to wait for the animation to finish
                StartCoroutine(WaitForExplosionAnimation());
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        // Falling reduces the health logic
        if (collision.gameObject.tag == "Death")
        {
            // Reduce current health
            currentHealth -= 10; // or any value you want
        }
    }
    private void Die()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            if (enemySpawner != null)
            {
                enemySpawner.SpawnEnemyAfterDelay();
            }

        }

    }
    IEnumerator WaitForExplosionAnimation()
    {
        // Wait for the length of the explosion animation
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}