using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import the TextMeshPro namespace

public class Player : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI coinCountText; // Assign your TextMeshProUGUI object in the Unity editor

    public float speed;
    public float jumpForce;
    public Animator anim;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Collider2D attackCollider;
    bool isGrounded;
    public Image[] hearts;
    public int maxHealth;
    int currentHealth;
    bool isGameOver = false;
    bool isImmune = false;
    public float immuneTime = 2f; // The duration of the immunity and blinking effect
    public float blinkInterval = 0.1f; // The interval between each blink
    private int coinCount = 0;
    private bool isDying = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        isGrounded = true;
        currentHealth = maxHealth;
        gameOverText.gameObject.SetActive(false); // Hide the Game Over text
    }

    void Update()
    {
        Movement();
        checkHealthStatus();
        if (isGameOver)
        {
            return;
        }
    }

    void Movement()   //Character movement class
    {
        bool isRunning = false;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            isRunning = true;
            sr.flipX = true;
            // Flip the attack collider
            if (attackCollider.transform.localScale.x > 0)
            {
                attackCollider.transform.localScale = new Vector3(-attackCollider.transform.localScale.x, attackCollider.transform.localScale.y, attackCollider.transform.localScale.z);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            isRunning = true;
            sr.flipX = false;
            // Flip the attack collider
            if (attackCollider.transform.localScale.x < 0)
            {
                attackCollider.transform.localScale = new Vector3(-attackCollider.transform.localScale.x, attackCollider.transform.localScale.y, attackCollider.transform.localScale.z);
            }

        }
        if (Input.GetKey(KeyCode.W))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce);
                isGrounded = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(AttackAnimation());
        }


        anim.SetBool("isRunning", isRunning);

    }

    void OnCollisionEnter2D(Collision2D col)  //Checks characters collisions
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        // Falling reduces the health logic
        if (col.gameObject.tag == "Death")
        {
            // Reduce current health
            currentHealth -= 10; // or any value you want
        }
    }
    IEnumerator RestartGameAfterDelay(float delay)
    {
        //  Time.timeScale = 0; // Pause the game
        yield return new WaitForSecondsRealtime(delay); // Wait for the specified delay
                                                        //  Time.timeScale = 1; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the game
    }
    IEnumerator GameOverTextAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Wait for the specified delay
        isGameOver = true;
        gameOverText.text = "Game Over";
        gameOverText.gameObject.SetActive(true); // Show the Game Over text

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
    }
    public void reduceHealth(int amount)
    {
        if (!isImmune)
        {
            currentHealth -= amount;
            StartCoroutine(StartImmunity());
            if (currentHealth > 0)
            {
                transform.position = startPosition;
                StartCoroutine(StartBlinking());
            }



            else if (currentHealth <= 0)
            {
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

                StartCoroutine(GameOverTextAfterDelay(1));
                StartCoroutine(RestartGameAfterDelay(5)); // Wait for 5 seconds and restart the game
            }

        }
    }
    IEnumerator StartBlinking()
    {
        isImmune = true;
        float endTime = Time.time + immuneTime;
        while (Time.time < endTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        sr.enabled = true;
        isImmune = false;
    }

    IEnumerator StartImmunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(3);
        isImmune = false;
    }
    IEnumerator AttackAnimation()
    {
        attackCollider.enabled = true;
        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("isAttacking", false);
        attackCollider.enabled = false;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (anim.GetBool("isAttacking"))
            {
                //Destroy(other.gameObject);

            }
            else
            {
                // If the player is not attacking, reduce their health
                reduceHealth(1); // replace 1 with the amount of health you want to reduce
            }
        }
    }
    public void IncreaseCoinCount()
    {
        coinCount++;
        Debug.Log(coinCount);
        coinCountText.text = "" + coinCount; // Update the UI Text element
    }

}
