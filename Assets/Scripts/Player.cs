using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public TMPro.TextMeshProUGUI gameOverText;
    public float speed;
    public float jumpForce;
    public Animator anim;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    bool isGrounded;
    public Image[] hearts;
    public int maxHealth;
    int currentHealth;
    bool isGameOver = false;

    void Start()
    {
        isGrounded = true;
        currentHealth = maxHealth;
        gameOverText.gameObject.SetActive(false); // Hide the Game Over text
    }

    void Update()
    {
        Movement();
        displayHealthStatus();
        if (isGameOver)
        {
            return;
        }
        if (currentHealth <= 0)
        {
            isGameOver = true;
            gameOverText.text = "Game Over";
            gameOverText.gameObject.SetActive(true); // Show the Game Over text
            StartCoroutine(RestartGameAfterDelay(5)); // Wait for 5 seconds and restart the game

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
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            isRunning = true;
            sr.flipX = false;

        }
        if (Input.GetKey(KeyCode.W))
        {
            if (isGrounded)
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
        // Falling reduces the health logic
        if (col.gameObject.tag == "Death")
        {
            // Reduce current health
            currentHealth -= 10; // or any value you want

            // Check if health is 0 or less, then game over

        }

    }
    IEnumerator RestartGameAfterDelay(float delay)
    {
        Time.timeScale = 0; // Pause the game
        yield return new WaitForSecondsRealtime(delay); // Wait for the specified delay
        Time.timeScale = 1; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the game
    }
    void displayHealthStatus()
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
    public void ReduceHealth(int amount)
    {
        currentHealth -= amount;
    }
}
