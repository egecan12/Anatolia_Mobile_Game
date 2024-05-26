using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include this namespace
using TMPro; // Import the TextMeshPro namespace

public class Episode3PlayerController : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public float blinkInterval = 0.1f;
    public Image[] hearts;
    public int maxHealth;
    public int currentHealth;
    public Rigidbody2D rb;
    private bool isJumping;
    public Animator anim;
    public InputActionReference jump;
    bool isGameOver = false;
    bool isImmune = false;
    public float immuneTime = 2f;
    private bool isGrounded;
    public SpriteRenderer sr;
    private int baseJumpForce = 30;
    private float extraJumpForce = 50;
    private float pressStartTime;
    private float pressDuration;

    private float maxJumpForce = 50; // Set this to your desired maximum jump force


    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("isRunning", true);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = true;
        currentHealth = maxHealth;
        gameOverText.gameObject.SetActive(false); // Hide the Game Over text


    }

    void Awake()
    {
        jump.action.started += ctx => pressStartTime = Time.time;
        jump.action.canceled += ctx =>
        {
            pressDuration = Time.time - pressStartTime;
            if (isGrounded && currentHealth > 0)
            {
                Jump(pressDuration);
                pressDuration = 0;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (jump.action.triggered && currentHealth > 0)
        {
            if (isGrounded)
            {
                return;
            }
        }

        if (isGameOver)
        {
            Time.timeScale = 0; // This freezes the game
            StartCoroutine(RestartSceneAfterDelay(3)); // Restart the scene after 3 seconds
            return;
        }

        checkHealthStatus();
    }

    public void reduceHealth(int amount)
    {
        if (!isImmune)
        {
            currentHealth -= amount;
            isImmune = true; // PlayerBalloon becomes immune after health is reduced

            // Perform the explosion animation every time when reduceHealth is called
            // if (anim != null)
            // {
            //     anim.SetBool("isDying", true);

            //     // Start a Coroutine to wait for the animation to finish
            //     StartCoroutine(WaitForDieAnim());
            // }

            //death
            StartCoroutine(StartImmunity());
            if (currentHealth > 0)
            {
                StartCoroutine(StartBlinking());
            }
            if (currentHealth <= 0)
            {
                if (rb != null)
                {
                    // Apply a downward force
                    rb.AddForce(new Vector2(0, -1), ForceMode2D.Impulse);
                    // If the player is dead, freeze the X position and rotation
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    anim.SetBool("isDying", true);

                    // Move the enemy a little bit down in the y-axis
                    float moveDownAmount = 0.5f; // Adjust this value as needed
                    transform.position = new Vector3(transform.position.x, transform.position.y - moveDownAmount, transform.position.z);

                    StartCoroutine(GameOverTextAfterDelay(1));
                }
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
    IEnumerator WaitForDieAnim()
    {
        // Wait for the length of the explosion animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // Set isExploding to false
        anim.SetBool("isDying", false);
        isImmune = false;


        // Change the sprite of the object

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

    void Jump(float pressDuration)
    {    // Calculate the jump force based on the press duration
        Debug.Log(pressDuration);
        float jumpForce = baseJumpForce + pressDuration * extraJumpForce;      // Apply the jump force
                                                                               // Ensure that the jump force does not exceed the maximum jump force
        jumpForce = Mathf.Min(jumpForce, maxJumpForce);
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // If the player is not attacking, reduce their health
            reduceHealth(1); // replace 1 with the amount of health you want to reduce
        }
    }
    IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Wait for the specified delay
        Time.timeScale = 1; // Unfreeze the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
