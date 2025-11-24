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
    public Button continueButton; // Continue button reference
    public float speed;
    public float jumpForce;
    public Animator anim;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    //public Collider2D attackCollider;
    bool isGrounded;
    public Image[] hearts;
    public int maxHealth;
    int currentHealth;
    bool isGameOver = false;
    bool isImmune = false;
    public float immuneTime = 2f; // The duration of the immunity and blinking effect
    public float blinkInterval = 0.1f; // The interval between each blink
    public int coinCount = 0;
    private bool isDying = false;
    private Vector3 startPosition;
    private bool isInBalloonRide = false; // Track if player is in balloon ride
    private Collider2D playerCollider; // Store player's collider reference
    private SpriteRenderer playerSpriteRenderer; // Store player's sprite renderer reference
    private Vector3 originalScale; // Store original player scale

    void Start()
    {
        startPosition = transform.position;
        isGrounded = true;
        currentHealth = maxHealth;
        gameOverText.gameObject.SetActive(false); // Hide the Game Over text
        if (continueButton != null)
            continueButton.gameObject.SetActive(false); // Hide the Continue button
            
        // Get Player's collider for balloon ride collision management
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Debug.Log($"🎮 Player collider found: {playerCollider.GetType()}");
        }
        
        // Get Player's sprite renderer for sorting order management
        playerSpriteRenderer = sr; // Use the existing sr reference
        if (playerSpriteRenderer != null)
        {
            Debug.Log($"🎮 Player sprite renderer found, current sorting order: {playerSpriteRenderer.sortingOrder}");
        }
        
        // Store original player scale for balloon ride
        originalScale = transform.localScale;
        Debug.Log($"🎮 Player original scale stored: {originalScale}");
    }

    void Update()
    {
        //Movement();
        checkHealthStatus();
        if (isGameOver)
        {
            return;
        }
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
        
        // Show Continue button
        if (continueButton != null)
            continueButton.gameObject.SetActive(true);
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

                StartCoroutine(GameOverTextAfterDelay(1));
                // Game will continue after clicking Continue button, no automatic restart
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
    /*IEnumerator AttackAnimation()
    {
        attackCollider.enabled = true;
        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("isAttacking", false);
        attackCollider.enabled = false;

    }*/
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

    public void OnContinueClick()
    {
        // Simply restart the entire scene - cleaner and more reliable
        Debug.Log("Restarting scene... Player will start with 3 lives.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void SetBalloonRideStatus(bool inRide)
    {
        isInBalloonRide = inRide;
        if (inRide)
        {
            // Player balloon ride started
            
            // SET PLAYER SORTING ORDER TO -1 (BACK LAYER)
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.sortingOrder = -1;
                // Player moved to back layer
            }
            
            // SCALE PLAYER DOWN TO HALF SIZE (2x SMALLER)
            transform.localScale = originalScale * 0.5f;
            // Player scaled down for balloon ride
            
            // COMPLETELY DISABLE PLAYER COLLISION
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
                // Player collision disabled
            }
            
            // Disable player's rigidbody physics during balloon ride
            if (rb != null)
            {
                rb.isKinematic = true;
                Debug.Log("🎈 Player rigidbody set to kinematic during balloon ride!");
            }
        }
        else
        {
            Debug.Log("🎈 Player balloon ride ended - collision re-enabled!");
            
            // RESTORE PLAYER SORTING ORDER TO NORMAL
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.sortingOrder = 0; // Normal layer
                Debug.Log("🎈 Player sorting order restored to 0 (NORMAL LAYER)!");
            }
            
            // RESTORE PLAYER SCALE TO NORMAL SIZE
            transform.localScale = originalScale;
            Debug.Log($"🎈 Player scale restored to original size! Scale: {transform.localScale}");
            
            // Re-enable player's collision (though this won't happen since scene changes)
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
                Debug.Log("✅ Player collider re-enabled!");
            }
            
            // Re-enable player's rigidbody physics
            if (rb != null)
            {
                rb.isKinematic = false;
                Debug.Log("🎈 Player rigidbody set to non-kinematic after balloon ride!");
            }
        }
    }

    // Animation Event method for HomelessJumpAnim
    public void ResetJumpAfterAnimation()
    {
        if (anim != null)
        {
            anim.SetBool("isJumping", false);
            Debug.Log("ResetJumpAfterAnimation called - isJumping set to false");
        }
        else
        {
            Debug.LogWarning("ResetJumpAfterAnimation called but anim is null!");
        }
    }

}
