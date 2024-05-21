using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // You need to import UnityEngine.UI to use Image

public class BalloonControl : MonoBehaviour
{
    public Image[] hearts;
    public int maxHealth;
    int currentHealth;
    private bool isDying = false; // Define isDying

    bool isImmune = false;
    public float immuneTime = 2f; // The duration of the immunity and blinking effect
    public float blinkInterval = 0.1f; // The interval between each blink
    public float upForce = 200f; // The upward force
    private Rigidbody2D rb; // The balloon's rigidbody
    private Vector3 startPosition; // Define startPosition
    private Animator anim; // Define anim
    private bool isRising;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Get the Animator component
        startPosition = transform.position; // Set the start position
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If the space bar is pressed...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ...apply an upward force to the balloon
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, upForce));
            // Set isRising to true
            isRising = true;
            // Update the isRising parameter in the Animator
            anim.SetBool("isRising", isRising);
            // Start a Coroutine to wait for the animation to finish
            StartCoroutine(WaitForAnimation());
        }

        checkHealthStatus();
    }

    IEnumerator WaitForAnimation()
    {
        // Wait for the length of the animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // Set isRising to false
        isRising = false;
        // Update the isRising parameter in the Animator
        anim.SetBool("isRising", isRising);
    }

    public void reduceHealth(int amount)
    {
        if (!isImmune)
        {
            currentHealth -= amount;
            // StartCoroutine(StartImmunity()); // Commented out as it's not defined
            if (currentHealth > 0)
            {
                transform.position = startPosition;
                // StartCoroutine(StartBlinking()); // Commented out as it's not defined
            }

            //death
            else if (currentHealth <= 0)
            {
                // If the player is dead, freeze the position
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                isDying = true;
                if (anim != null)
                {
                    anim.SetBool("isDying", true);
                }
                if (rb != null)
                {
                    rb.simulated = false;
                }

                // Move the enemy a little bit down in the y-axis
                float moveDownAmount = 0.5f; // Adjust this value as needed
                transform.position = new Vector3(transform.position.x, transform.position.y - moveDownAmount, transform.position.z);

                // StartCoroutine(GameOverTextAfterDelay(1)); // Commented out as it's not defined
                // StartCoroutine(RestartGameAfterDelay(5)); // Commented out as it's not defined
            }
        }
    }

    void checkHealthStatus()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < currentHealth; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }

        if (currentHealth <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void OnCollisionEnter2D(Collision2D col)  //Checks characters collisions
    {
        // Falling reduces the health logic
        if (col.gameObject.tag == "Death")
        {
            // Reduce current health
            currentHealth -= 10; // or any value you want
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // If the player is not attacking, reduce their health
            reduceHealth(1); // replace 1 with the amount of health you want to reduce
        }
    }
}