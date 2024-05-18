using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHomeless : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the enemy moves towards the player
   // private Transform player; // Reference to the player's position
    private Animator animator; // Reference to the Animator component
    private Player player;
    public float attackRange = 1.0f;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player by tag and get its transform
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is not null, move towards the player
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();

            // Flip the enemy to face the player
            if (direction.x > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            // Move towards the player
            transform.position += direction * speed * Time.deltaTime;

            // Start the run animation
            animator.SetBool("isRunning", true);
        }
        else
        {
            // If the enemy is not moving, stop the run animation
            animator.SetBool("isRunning", false);
        }

        if(player != null && isAttacking)
        {
            player.reduceHealth(1);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Start the attack animation
            animator.SetBool("isAttacking", true);
            isAttacking = true;
        }
        
        //Player player = collision.gameObject.GetComponent<Player>();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Stop the attack animation
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }
    }

    public void AttackFinished()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            player.reduceHealth(1);
        }
    }
}