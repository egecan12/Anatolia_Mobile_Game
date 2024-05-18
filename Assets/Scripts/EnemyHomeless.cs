using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHomeless : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the enemy moves towards the player
    private Animator animator; // Reference to the Animator component
    private Player player;
    public float attackRange = 1.0f;
    private bool isAttacking = false;
    public float health = 1f; // The enemy's health
    private bool isDying = false; // Whether the enemy is dying
    public EnemySpawner enemySpawner;

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
        // Assign a value to enemySpawner
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyMovement();

        if (player != null && isAttacking)
        {
            player.reduceHealth(1);
        }
        if (health <= 0f && !isDying)
        {
            Die();
        }
    }

    public void enemyMovement()
    {
        // If the player is not null, move towards the player
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            float distanceToPlayer = direction.magnitude;
            direction.y = 0; // This line ensures that the enemy does not move in the Y direction
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
            // If the enemy is not within attack range, move towards the player
            if (distanceToPlayer > attackRange)
            {
                transform.position += direction * speed * Time.deltaTime;
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);
            }
            else
            {
                // If the enemy is within attack range, stop moving and start attacking
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", true);
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
    }
    public void reduceEnemyHealth(float amount)
    {
        health -= amount;

        // If the enemy's health reaches zero and it's not dying yet, start the dying animation
        if (health <= 0f && !isDying)
        {
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Animator playerAnim = other.gameObject.GetComponent<Animator>();
            if (playerAnim != null && playerAnim.GetBool("isAttacking"))
            {
                // If the player is attacking, reduce the enemy's health
                reduceEnemyHealth(1); // replace 1 with the amount of health you want to reduce
            }
        }
    }

    private void Die()
    {
        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemyAfterDelay();
        }
        isDying = true;
        if (animator != null)
        {
            animator.SetBool("isDying", true);
            StartCoroutine(DestroyAfterAnimation());
        }
        // Disable the Rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }

        // Move the enemy a little bit down in the y-axis
        float moveDownAmount = 0.8f; // Adjust this value as needed
        transform.position = new Vector3(transform.position.x, transform.position.y - moveDownAmount, transform.position.z);

    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the length of the dying animation before destroying the enemy
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
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